using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public ILikesRepository LikesRepository { get; }
        public Task<bool> Complete();
        public bool HasChanges();
    }
}
