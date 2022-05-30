using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IPhotoDataManager
    {
        public Task<List<Photo>> GetPhotosAsync();
        public Task<List<Photo>> GetPhotosByUserId(int id);
        public Task<int> CountAsync();
        public Task<PhotoDto> SavePhotoAsync(Photo photo);
        public Task<PhotoDto> UpdatePhotoAsync(Photo photo);
        public Task<Photo> GetPhotoByIdAsync(int id);
        public Task<ActionResult> RemovePhoto(Photo photo);
        public Task<Photo> GetMainPhotoByUserId(int id);
    }
}
