namespace UltraPlayTask.Infrastructure.DTOs
{
    public class MatchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public List<BetDto> Bets { get; set; }
    }
}