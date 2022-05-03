using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PlatformUserDataManager : IPlatformUserDataManager
    {
        private readonly DataContext context;
        private readonly IPhotoDataManager photoDataManager;
        private readonly IMapper mapper;

        public PlatformUserDataManager(DataContext context, IPhotoDataManager photoDataManager, IMapper mapper)
        {
            this.context = context;
            this.photoDataManager = photoDataManager;
            this.mapper = mapper;
        }

        public async Task<PlatformUser> GetUserByIdAsync(int id)
        {
            return await context.PlatformUsers.FindAsync(id);
        }

        public async Task<PlatformUser> GetUserByUsernameAsync(string username)
        {
            return await context.PlatformUsers.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<PlatformUser>> GetUsersAsync()
        {
            return await context.PlatformUsers.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(PlatformUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<FrontendUserDto> MapPlatformUserToFrontendUser(PlatformUser user)
        {
            var photos = await photoDataManager.GetPhotosByUserId(user.Id.Value);

            FrontendUserDto userToReturn = mapper.Map<FrontendUserDto>(user);
            userToReturn.Photos = mapper.Map<IList<PhotoDto>>(photos);

            var mainPhotoUrl = photos.Find(x => x.IsMain == true);
            userToReturn.PhotoUrl = mainPhotoUrl.Url;

            return userToReturn;
        }
    }
}
