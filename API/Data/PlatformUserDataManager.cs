using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
#nullable disable

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

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<PagedList<User>> GetUsersAsync(UserParams userParams)
        {
            var query = context.Users.AsNoTracking().AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var earliestDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var mostRecentDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            query = query.Where(u => u.DateOfBirth.Value >= earliestDob && u.DateOfBirth.Value <= mostRecentDob);

            return await PagedList<User>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<PagedList<FrontendUserDto>> MapPlatformUserListToFrontendUserList(PagedList<User> users, UserParams userParams)
        {
            // Need to add photos and main photo url to FrontendUserDto
            List<Photo> photos = await photoDataManager.GetPhotosAsync();
            List<PhotoDto> photoDtos = mapper.Map<List<PhotoDto>>(photos);

            var usersToReturn = mapper.Map<List<FrontendUserDto>>(users.ToList());

            foreach (var user in usersToReturn)
            {
                var photosToAdd = photoDtos.FindAll(x => x.PlatformUserId == user.Id);
                foreach (var photo in photosToAdd)
                {
                    if (photo.IsMain.Value)
                    {
                        user.PhotoUrl = photo.Url;
                    }
                    user.Photos.Add(photo);
                }
            }
           
            var listToReturn = new PagedList<FrontendUserDto>(
                usersToReturn,
                users.CurrentPage,
                users.TotalPages,
                users.PageSize,
                users.TotalCount
                );

            return listToReturn;
        }

        public async Task<FrontendUserDto> MapPlatformUserToFrontendUser(User user)
        {
            var photos = await photoDataManager.GetPhotosByUserId(user.Id.Value);

            FrontendUserDto userToReturn = mapper.Map<FrontendUserDto>(user);
            userToReturn.Photos = mapper.Map<IList<PhotoDto>>(photos);

            var mainPhotoUrl = photos.Find(x => x.IsMain == 1);
            userToReturn.PhotoUrl = mainPhotoUrl?.Url;

            return userToReturn;
        }

        public async Task<FrontendUserDto> GetFrontendUserByUsernameAsync(string username)
        {
            var user = context.Users.ToList().Find(x => x.UserName == username);
            var userToReturn = await MapPlatformUserToFrontendUser(user);
            return userToReturn;
        }
    }
}
