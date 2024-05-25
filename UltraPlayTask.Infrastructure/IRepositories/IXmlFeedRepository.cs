using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.IRepositories
{
    public interface IXmlFeedRepository
    {
        Task<Sport?> GetSportByName(string sportName);
        Task AddSport(Sport sport);
        Task<Event?> GetEventByIdAndName(int id, string eventName);
        Task AddEvent(Event eventEntity);
        Task<Match?> GetMatchByIdAndName(int id, string matchName);
        Task AddMatch(Match match);
        Task UpdateMatch(Match match);
        Task<Bet?> GetBet(int id, string betName, bool isLive);
        Task AddBet(Bet bet);
        Task<Odd?> GetOdd(int betId, decimal oddValue, decimal? specialBetValue);
        Task AddOdd(Odd odd);
        Task UpdateOdd(Odd odd);
    }
}
