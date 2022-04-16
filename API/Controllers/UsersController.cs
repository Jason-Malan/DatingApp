using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private DataContext _dataContext;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UsersController(ILogger<UsersController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformUser>>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }

        /// <summary>
        /// Returns a single user based on the user Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformUser>> GetUser(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }
    }

}