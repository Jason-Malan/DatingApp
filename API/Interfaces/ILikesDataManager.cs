using API.DTOs;
using API.Entities;
using API.Models.Entities;

namespace API.Interfaces
{
    public interface ILikesDataManager
    {
        Task<UserLike> GetUserLike(int sourceUserId, int LikedUserId);
        Task<FrontendUserDto> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
        public Task<bool> SaveAllAsync();
        public Task<UserLike> SaveUserLikeAsync(UserLike userLike);
    }
}
