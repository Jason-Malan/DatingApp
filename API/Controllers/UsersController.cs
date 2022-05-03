using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //[Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IPlatformUserDataManager platformUserDataManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UsersController(ILogger<UsersController> logger, IPlatformUserDataManager platformUserDataManager, IMapper mapper)
        {
            _logger = logger;
            this.platformUserDataManager = platformUserDataManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FrontendUserDto>>> GetUsers()
        {
            var users = await platformUserDataManager.GetUsersAsync();

            var usersToReturn = new List<FrontendUserDto>();

            foreach (var user in users)
            {
                usersToReturn.Add(await platformUserDataManager.MapPlatformUserToFrontendUser(user));
            }

            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns a single user based on the user Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<FrontendUserDto>> GetUser(string username)
        {
            var user = await platformUserDataManager.GetUserByUsernameAsync(username);
            return mapper.Map<FrontendUserDto>(user);
        }
    }

}