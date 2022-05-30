using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#nullable disable

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

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await context.Photos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ActionResult> RemovePhoto(Photo photo)
        {
            try
            {
                context.Photos.Remove(photo);
                if (await context.SaveChangesAsync() > 0) return new OkResult();
                return new BadRequestResult();
            } catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        public async Task<List<Photo>> GetPhotosAsync()
        {
            var photos = await context.Photos!.ToListAsync();
            return photos;
        }

        public async Task<List<Photo>> GetPhotosByUserId(int id)
        {
            List<Photo> usersPhotos = await context.Photos!.AsNoTracking().Where(x => x.PlatformUserId == id).ToListAsync();

            return usersPhotos;
        }

        public async Task<Photo> GetMainPhotoByUserId(int id)
        {
            Photo photo = await context.Photos!.AsNoTracking()
                .Where(x => x.IsMain.Value == 1)
                .FirstOrDefaultAsync(x => x.PlatformUserId == id);

            return photo;
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

        public async Task<PhotoDto> UpdatePhotoAsync(Photo photo)
        {
            context.Photos?.Update(photo);
            if (await context.SaveChangesAsync() > 0)
            {
                return mapper.Map<PhotoDto>(photo);
            }
            return new PhotoDto();
        }
    }
}
