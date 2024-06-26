﻿namespace UltraPlayTask.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string MatchType { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public List<Bet> Bets { get; set; }
    }
}
