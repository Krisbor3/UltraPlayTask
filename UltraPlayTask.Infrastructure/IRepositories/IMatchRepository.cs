using UltraPlayTask.Infrastructure.DTOs;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Infrastructure.IRepositories
{
    public interface IMatchRepository
    {
        Task<List<MatchModel>> GetMatchesStartingInNext24Hours();
        Task<Match?> GetMatchById(int id);
    }
}
