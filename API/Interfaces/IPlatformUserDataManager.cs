using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPlatformUserDataManager
    {
        public void Update(User user);
        public Task<bool> SaveAllAsync();
        public Task<PagedList<User>> GetUsersAsync(UserParams userParams);
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<FrontendUserDto> MapPlatformUserToFrontendUser(User user);
        public Task<PagedList<FrontendUserDto>> MapPlatformUserListToFrontendUserList(PagedList<User> users, UserParams userParams);
        public Task<FrontendUserDto> GetFrontendUserByUsernameAsync(string username);
    }
}
