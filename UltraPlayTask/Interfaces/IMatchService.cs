using UltraPlayTask.Models;

namespace UltraPlayTask.Interfaces
{
    public interface IMatchService
    {
        Task<List<MatchViewModel>> GetMatchesStartingInNext24Hours();
    }
}
