using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

#nullable disable
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
        public async Task<ActionResult<PagedList<FrontendUserDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUser = await platformUserDataManager.GetUserByUsernameAsync(User.GetUsername());

            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await platformUserDataManager.GetUsersAsync(userParams);

            var usersToReturn = await platformUserDataManager.MapPlatformUserListToFrontendUserList(users, userParams);

            Response.AddPaginationHeader(usersToReturn.CurrentPage, usersToReturn.PageSize, usersToReturn.TotalCount, usersToReturn.TotalPages);

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
            var user = await platformUserDataManager.GetUserByUsernameAsync(User.GetUsername());

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                PlatformUserId = user.Id,
                IsMain = 0,
            };

            var photoCount = await photoDataManager.GetPhotosByUserId(user.Id.Value);

            if (photoCount.Count == 0)
            {
                photo.IsMain = 1;
            }

            var photoDto = await photoDataManager.SavePhotoAsync(photo);

            if (photoDto == null)
            {
                return BadRequest("Problem adding photo");
            }

            return CreatedAtRoute("GetUser", new { username = User.GetUsername() }, photoDto);
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await platformUserDataManager.GetFrontendUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain.Value) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain.Value);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            var update1 = await photoDataManager.UpdatePhotoAsync(mapper.Map<Photo>(currentMain));
            var update2 = await photoDataManager.UpdatePhotoAsync(mapper.Map<Photo>(photo));

            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await platformUserDataManager.GetFrontendUserByUsernameAsync(User.GetUsername());

            var photoDto = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photoDto == null) return NotFound();

            if (photoDto.IsMain.Value) return BadRequest("You cannot delete your main photo");

            var photo = await photoDataManager.GetPhotoByIdAsync(photoDto.Id.Value);

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            var result1 = await photoDataManager.RemovePhoto(photo);

            if (result1 is OkResult okResult)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }

        }
    }

}