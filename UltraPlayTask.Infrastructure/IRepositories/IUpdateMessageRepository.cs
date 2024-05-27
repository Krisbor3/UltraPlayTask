using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.IRepositories
{
    public interface IUpdateMessageRepository
    {
        Task UpdateMessages(UpdateMessage? updateMessage);
    }
}
