using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraPlayTask.Infrastructure.DTOs;
using UltraPlayTask.Infrastructure.IRepositories;

namespace UltraPlayTask.Infrastructure.Repositories
{
    public class MatchRepository: IMatchRepository
    {
        private UltraPlayTaskDBContext _db;
        public MatchRepository(UltraPlayTaskDBContext db)
        {
            _db = db;
        }

        public async Task<List<MatchModel>> GetMatchesStartingInNext24Hours()
        {
            var matches = await _db.Matches
            .Where(m => m.StartDate >= DateTime.Now && m.StartDate <= DateTime.Now.AddHours(24))
            .Select(m => new MatchModel()
            {
                Name = m.Name,
                StartDate = m.StartDate
            })
            .ToListAsync();

            return matches;
        }
    }
}
