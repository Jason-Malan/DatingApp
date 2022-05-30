using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseAPIController
    {
        private readonly IPlatformUserDataManager platformUserDataManager;
        private readonly ILikesDataManager likesDataManager;

        public LikesController(IPlatformUserDataManager platformUserDataManager, ILikesDataManager likesDataManager)
        {
            this.platformUserDataManager = platformUserDataManager;
            this.likesDataManager = likesDataManager;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await platformUserDataManager.GetUserByUsernameAsync(username);
            var sourceUser = await likesDataManager.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.Username == username) return BadRequest("You cannot like yourself");

            var userLike = await likesDataManager.GetUserLike(sourceUserId, likedUser.Id!.Value);

            if (userLike != null) return BadRequest("You already liked this user");

            userLike = new Models.Entities.UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id!.Value
            };

            if (await likesDataManager.SaveUserLikeAsync(userLike) != null) return Ok();


            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
            var users = await likesDataManager.GetUserLikes(predicate, User.GetUserId());

            return Ok(users);
        }
    }
}
