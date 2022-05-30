using API.DTOs;
using API.Interfaces;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Entities;
#nullable disable

namespace API.Data
{
    public class LikesDataManager : ILikesDataManager
    {
        private readonly DataContext context;
        private readonly IPlatformUserDataManager platformUserDataManager;
        private readonly IPhotoDataManager photoDataManager;

        public LikesDataManager(DataContext context,
                                IPlatformUserDataManager platformUserDataManager,
                                IPhotoDataManager photoDataManager)
        {
            this.context = context;
            this.platformUserDataManager = platformUserDataManager;
            this.photoDataManager = photoDataManager;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int LikedUserId)
        {
            try
            {
                return await context.UserLike.FindAsync(sourceUserId, LikedUserId);
            } catch (Exception ex)
            {
                return null;
            }
        }

        // Get list of users current user has liked or users that have liked current user.
        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var foundUsers = new List<PlatformUser>();

            if (predicate == "liked")
            {
                var liked = await context.UserLike.AsNoTracking().Where(like => like.SourceUserId == userId).ToListAsync();
                var userIdsToGet = liked.Select(like => like.LikedUserId);
                foreach (var id in userIdsToGet)
                {
                    var user = await context.PlatformUsers.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id);
                    if (user != null)
                    {
                        foundUsers.Add(user);
                    }
                }
            }

            if (predicate == "likedBy")
            {
                var liked = await context.UserLike.AsNoTracking().Where(like => like.LikedUserId == userId).ToListAsync();
                var userIdsToGet = liked.Select(like => like.SourceUserId);
                foreach (var id in userIdsToGet)
                {
                    var user = await context.PlatformUsers.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id);
                    if (user != null)
                    {
                        foundUsers.Add(user);
                    }
                }
            }

            var likeDtos = foundUsers.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.Value.CalculateAge(),
                PhotoUrl = null,
                City = user.City,
                Id = user.Id.Value,
            }).ToList();

            // Get PhotoUrl foreach likeDto
            foreach (var likeDto in likeDtos)
            {
                var urlToAdd = await photoDataManager.GetMainPhotoByUserId(likeDto.Id);
                likeDto.PhotoUrl = urlToAdd.Url;
            }

            return likeDtos;
        }

        public async Task<FrontendUserDto> GetUserWithLikes(int userId)
        {
            var user = await platformUserDataManager.GetUserByIdAsync(userId);
            var frontendUserDto = await platformUserDataManager.MapPlatformUserToFrontendUser(user);
            var userLikes = await context.UserLike.Where(x => x.SourceUserId == userId).ToListAsync();
            frontendUserDto.Likes = userLikes;
            return frontendUserDto;
        }

        public async Task<UserLike> SaveUserLikeAsync(UserLike userLike)
        {
            context.UserLike?.Add(userLike);
            if (await context.SaveChangesAsync() > 0)
            {
                return userLike;
            }
            return null;
        }
    }
}
