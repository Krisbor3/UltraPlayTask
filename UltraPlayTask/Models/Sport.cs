using Microsoft.Extensions.Logging;

namespace UltraPlayTask.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Event> Events { get; set; }
    }
}
