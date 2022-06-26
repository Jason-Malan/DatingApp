using API.DTOs;
using API.Helpers;
using API.Models.Entities;

namespace API.Interfaces
{
    public interface IMessageDataManager
    {
        public void AddMessage(Message message);
        public void DeleteMessage(Message message);
        public Task<Message> GetMessageAsync(int id);
        public Task<PagedList<MessageDto>> GetMessageForUser(Message message);
        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);
        public Task<bool> SaveAllAsync();
    }
}
