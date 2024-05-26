namespace UltraPlayTask.Models
{
    public class MatchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}
