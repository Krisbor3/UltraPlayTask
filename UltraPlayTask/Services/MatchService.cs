using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Interfaces;
using UltraPlayTask.Models;

namespace UltraPlayTask.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Infrastructure.Models.Match?> GetMatchById(int id)
        {
            var match = await _matchRepository.GetMatchById(id);
            return match;
        }

        public async Task<List<MatchViewModel>> GetMatchesStartingInNext24Hours()
        {
            var matches = await _matchRepository.GetMatchesStartingInNext24Hours();
            var result = matches.Select(x => new MatchViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                StartDate = x.StartDate,
                Bets = x.Bets.Select(y => new Bet()
                {
                    Name = y.Name,
                    Odds = y.Odds.Select(o => new Odd { Value = o.Value }).ToList()
                })
                .ToList()
            })
                .ToList();
            return result;
        }
    }
}
