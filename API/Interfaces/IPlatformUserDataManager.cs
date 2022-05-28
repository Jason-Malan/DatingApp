using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPlatformUserDataManager
    {
        public void Update(PlatformUser user);
        public Task<bool> SaveAllAsync();
        public Task<PagedList<PlatformUser>> GetUsersAsync(UserParams userParams);
        public Task<PlatformUser> GetUserByIdAsync(int id);
        public Task<PlatformUser> GetUserByUsernameAsync(string username);
        public Task<FrontendUserDto> MapPlatformUserToFrontendUser(PlatformUser user);
        public Task<PagedList<FrontendUserDto>> MapPlatformUserListToFrontendUserList(PagedList<PlatformUser> users, UserParams userParams);
        public Task<FrontendUserDto> GetFrontendUserByUsernameAsync(string username);
    }
}
