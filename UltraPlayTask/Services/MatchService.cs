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
        public async Task<List<MatchViewModel>> GetMatchesStartingInNext24Hours()
        {
            var matches = await _matchRepository.GetMatchesStartingInNext24Hours();
            var result = matches.Select(x=>new MatchViewModel() { Name = x.Name, StartDate = x.StartDate}).ToList();
            return result;
        }
    }
}
