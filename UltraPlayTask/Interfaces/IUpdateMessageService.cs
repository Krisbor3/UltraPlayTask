namespace UltraPlayTask.Interfaces
{
    public interface IUpdateMessageService
    {
        Task AddUpdateMessageAsync(string entityType, int entityId, string action);
    }
}
