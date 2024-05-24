using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraPlayTask.Infrastructure.DTOs;

namespace UltraPlayTask.Infrastructure.IRepositories
{
    public interface IMatchRepository
    {
        Task<List<MatchModel>> GetMatchesStartingInNext24Hours();
    }
}
