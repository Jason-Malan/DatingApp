using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoDataManager : IPhotoDataManager
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public PhotoDataManager(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<List<Photo>> GetPhotosAsync()
        {
            var photos = await context.Photos!.ToListAsync();
            return photos;
        }

        public async Task<List<Photo>> GetPhotosByUserId(int id)
        {
            List<Photo> usersPhotos = await context.Photos!.Where(x => x.PlatformUserId == id).ToListAsync();

            return usersPhotos;
        }

        public async Task<int> CountAsync()
        {
            return await context.Photos!.CountAsync();
        }

        public async Task<PhotoDto> SavePhotoAsync(Photo photo)
        {
            context.Photos?.Add(photo);
            if (await context.SaveChangesAsync() > 0)
            {
                return mapper.Map<PhotoDto>(photo);
            }
            return new PhotoDto();
        }
    }
}
