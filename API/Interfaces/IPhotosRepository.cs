using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IPhotosRepository
    {
        public Task<ICollection<Photo>> GetPhotosForModeration();
        public Task ApproveAllPhotos();
    }
}
