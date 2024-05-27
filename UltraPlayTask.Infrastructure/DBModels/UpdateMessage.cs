namespace UltraPlayTask.Infrastructure.Models
{
    public class UpdateMessage
    {
        public int Id { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
