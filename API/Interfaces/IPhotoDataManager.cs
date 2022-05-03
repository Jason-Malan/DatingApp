using API.Entities;

namespace API.Interfaces
{
    public interface IPhotoDataManager
    {
        public Task<IEnumerable<Photo>> GetPhotosAsync();
        public Task<List<Photo>> GetPhotosByUserId(int id);
    }
}
