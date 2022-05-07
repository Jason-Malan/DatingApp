using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoDataManager
    {
        public Task<List<Photo>> GetPhotosAsync();
        public Task<List<Photo>> GetPhotosByUserId(int id);
    }
}
