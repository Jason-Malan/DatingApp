using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    //[Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly ILogger<UsersController> logger;
        private readonly IPlatformUserDataManager platformUserDataManager;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IPhotoDataManager photoDataManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UsersController(ILogger<UsersController> logger,
                               IPlatformUserDataManager platformUserDataManager,
                               IMapper mapper,
                               IPhotoService photoService,
                               IPhotoDataManager photoDataManager)
        {
            this.logger = logger;
            this.platformUserDataManager = platformUserDataManager;
            this.mapper = mapper;
            this.photoService = photoService;
            this.photoDataManager = photoDataManager;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FrontendUserDto>>> GetUsers()
        {
            var users = await platformUserDataManager.GetUsersAsync();

            var usersToReturn = await platformUserDataManager.MapPlatformUserListToFrontendUserList(users.ToList());

            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns a single user based on the user Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<FrontendUserDto>> GetUser(string username)
        {
            var user = await platformUserDataManager.GetFrontendUserByUsernameAsync(username);
            return Ok(user);   
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await platformUserDataManager.GetUserByUsernameAsync(User.GetUsername());

            mapper.Map(memberUpdateDto, user);
            platformUserDataManager.Update(user);

            if (await platformUserDataManager.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = User.GetUsername();

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            var photoCount = await photoDataManager.CountAsync();

            if (photoCount == 0)
            {
                photo.IsMain = true;
            }

            var photoDto = await photoDataManager.SavePhotoAsync(photo);

            return CreatedAtRoute("GetUser", new { username = User.GetUsername() }, photoDto);

            return BadRequest("Problem adding photo");
        }   
    }

}