using UltraPlayTask.Infrastructure.DTOs;

namespace UltraPlayTask.Infrastructure.IRepositories
{
    public interface IMatchRepository
    {
        Task<List<MatchModel>> GetMatchesStartingInNext24Hours();
    }
}
