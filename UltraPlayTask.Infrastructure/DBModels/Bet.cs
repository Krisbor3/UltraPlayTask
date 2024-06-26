﻿namespace UltraPlayTask.Infrastructure.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsLive { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public List<Odd> Odds { get; set; }
    }
}
