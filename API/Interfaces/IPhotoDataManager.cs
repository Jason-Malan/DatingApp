using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoDataManager
    {
        public Task<List<Photo>> GetPhotosAsync();
        public Task<List<Photo>> GetPhotosByUserId(int id);
        public Task<int> CountAsync();
        public Task<PhotoDto> SavePhotoAsync(Photo photo);
    }
}
