using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoDataManager : IPhotoDataManager
    {
        private readonly DataContext context;

        public PhotoDataManager(DataContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            var photos = await context.Photos!.ToListAsync();
            return photos;
        }

        public async Task<List<Photo>> GetPhotosByUserId(int id)
        {
            List<Photo> usersPhotos = await context.Photos!.Where(x => x.PlatformUserId == id).ToListAsync();

            return usersPhotos;
        }
    }
}
