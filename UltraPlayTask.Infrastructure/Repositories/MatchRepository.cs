using Microsoft.EntityFrameworkCore;
using UltraPlayTask.Infrastructure.DTOs;
using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private UltraPlayTaskDBContext _db;
        public MatchRepository(UltraPlayTaskDBContext db)
        {
            _db = db;
        }

        public async Task<Match?> GetMatchById(int id)
        {
            var match = await _db.Matches
            .Include(m => m.Bets)
            .ThenInclude(b => b.Odds)
            .FirstOrDefaultAsync(m => m.Id == id);

            return match;
        }

        public async Task<List<MatchModel>> GetMatchesStartingInNext24Hours()
        {
            var now = DateTime.UtcNow;
            var next24Hours = now.AddHours(24);

            var matches = await _db.Matches
                .Where(m => m.StartDate >= now && m.StartDate <= next24Hours)
                .Select(m => new MatchModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    StartDate = m.StartDate,
                    Bets = m.Bets
                        .Where(b => !b.IsLive && (b.Name == "Match Winner" || b.Name == "Map Advantage" || b.Name == "Total Maps Played"))
                        .Select(b => new BetDto
                        {
                            Name = b.Name,
                            Odds = b.Odds
                                .GroupBy(o => o.SpecialBetValue)
                                .Select(g => new OddDto { Value = g.First().Value })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            return matches;
        }
    }
}
