using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IPlatformUserDataManager
    {
        public void Update(PlatformUser user);
        public Task<bool> SaveAllAsync();
        public Task<IEnumerable<PlatformUser>> GetUsersAsync();
        public Task<PlatformUser> GetUserByIdAsync(int id);
        public Task<PlatformUser> GetUserByUsernameAsync(string username);
        public Task<FrontendUserDto> MapPlatformUserToFrontendUser(PlatformUser user);
    }
}
