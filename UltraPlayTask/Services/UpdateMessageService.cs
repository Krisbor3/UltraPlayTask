using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Models;
using UltraPlayTask.Interfaces;

namespace UltraPlayTask.Services
{
    public class UpdateMessageService: IUpdateMessageService
    {
        private readonly IUpdateMessageRepository _repo;

        public UpdateMessageService(IUpdateMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task AddUpdateMessageAsync(string entityType, int entityId, string action)
        {
            var updateMessage = new UpdateMessage
            {
                EntityType = entityType,
                EntityId = entityId,
                Action = action,
                Timestamp = DateTime.UtcNow
            };

            await _repo.UpdateMessages(updateMessage);
        }
    }
}
