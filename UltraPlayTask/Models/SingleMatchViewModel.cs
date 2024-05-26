namespace UltraPlayTask.Models
{
    internal class SingleMatchViewModel
    {
        public SingleMatchViewModel()
        {
        }

        public string MatchName { get; set; }
        public DateTime StartDate { get; set; }
        public List<Bet> Bets { get; set; }
    }
}