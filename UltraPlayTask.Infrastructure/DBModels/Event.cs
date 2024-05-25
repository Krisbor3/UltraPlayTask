namespace UltraPlayTask.Infrastructure.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public List<Match> Matches { get; set; }
    }
}
