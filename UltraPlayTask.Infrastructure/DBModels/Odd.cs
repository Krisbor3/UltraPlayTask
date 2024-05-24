
namespace UltraPlayTask.Infrastructure.Models
{
    public class Odd
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int BetId { get; set; }
        public Bet Bet { get; set; }
        public decimal? SpecialBetValue { get; set; }
    }
}
