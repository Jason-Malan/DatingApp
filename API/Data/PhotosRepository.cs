using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class PhotosRepository : IPhotosRepository
    {
        private readonly DataContext context;

        public PhotosRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task ApproveAllPhotos()
        {
            var photos = await context.Photos.Where(x => !x.isApproved).ToListAsync();

            foreach (var photo in photos)
            {
                photo.isApproved = true;
            }
        }

        public async Task<ICollection<Photo>> GetPhotosForModeration()
        {
            var result = await context.Photos.Where(x => !x.isApproved).ToArrayAsync();

            if (result.Any())
            {
                return result;
            }

            return null;
        }
    }
}
