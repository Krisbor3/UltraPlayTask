using Microsoft.EntityFrameworkCore;
using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.Repositories
{
    public class XmlFeedRepository: IXmlFeedRepository
    {
        private UltraPlayTaskDBContext dbContext;

        public XmlFeedRepository(UltraPlayTaskDBContext dBContext)
        {
            dbContext = dBContext;
        }
        public async Task<Sport?> GetSportByName(string sportName)
        {
            return await dbContext.Sports.FirstOrDefaultAsync(s => s.Name == sportName);
        }

        public async Task AddSport(Sport sport)
        {
            dbContext.Sports.Add(sport);
            await SaveChagesAsync();
        }

        public async Task<Event?> GetEventByIdAndName(int id, string eventName)
        {
            return await dbContext.Events.FirstOrDefaultAsync(e => e.Name == eventName && e.SportId == id);
        }
        public async Task AddEvent(Event eventEntity)
        {
            dbContext.Events.Add(eventEntity);
            await SaveChagesAsync();
        }

        public async Task<Match?> GetMatchByIdAndName(int id, string matchName)
        {
            return await dbContext.Matches.FirstOrDefaultAsync(m => m.Name == matchName && m.EventId == id);
        }

        public async Task AddMatch(Match match)
        {
            dbContext.Matches.Add(match);
            await SaveChagesAsync();
        }
        public async Task UpdateMatch(Match match)
        {
            dbContext.Update(match);
            await SaveChagesAsync();
        }

        public async Task<Bet?> GetBet(int id, string betName, bool isLive)
        {
            return await dbContext.Bets.FirstOrDefaultAsync(b => b.Name == betName && b.MatchId == id && b.IsLive == isLive);
        }

        public async Task AddBet(Bet bet)
        {
            dbContext.Bets.Add(bet);
            await SaveChagesAsync();
        }

        public async Task<Odd?> GetOdd(int betId, decimal oddValue, decimal? specialBetValue)
        {
            return await dbContext.Odds.FirstOrDefaultAsync(o => o.BetId == betId && o.Value == oddValue && o.SpecialBetValue == specialBetValue);
        }

        public async Task AddOdd(Odd odd)
        {
            dbContext.Odds.Add(odd);
            await SaveChagesAsync();
        }

        public async Task UpdateOdd(Odd odd)
        {
            dbContext.Odds.Update(odd);
            await SaveChagesAsync();
        }
        public async Task SaveChagesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
