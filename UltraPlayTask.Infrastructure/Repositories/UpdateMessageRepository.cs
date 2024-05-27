using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.Repositories
{
    public class UpdateMessageRepository : IUpdateMessageRepository
    {
        private UltraPlayTaskDBContext _db;

        public UpdateMessageRepository(UltraPlayTaskDBContext db)
        {
            _db = db;
        }
        public async Task UpdateMessages(UpdateMessage? updateMessage)
        {
            if (updateMessage != null)
            {
                _db.UpdateMessages.Add(updateMessage);
                await _db.SaveChangesAsync();
            }
        }
    }
}
