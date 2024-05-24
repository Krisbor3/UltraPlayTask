using Microsoft.EntityFrameworkCore;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure
{
    public class UltraPlayTaskDBContext : DbContext
    {
        public UltraPlayTaskDBContext(
            DbContextOptions<UltraPlayTaskDBContext> options) : base(options)
        { }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Odd> Odds { get; set; }

    }
}
