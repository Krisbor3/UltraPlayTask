using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UltraPlayTask.Interfaces;
using UltraPlayTask.Models;

namespace UltraPlayTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchService _matchService;

        public HomeController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await _matchService.GetMatchesStartingInNext24Hours();
            return View(matches);
        }

        [HttpGet]
        public async Task<IActionResult> Match(int id)
        {
            var match = await _matchService.GetMatchById(id);


            if (match == null)
            {
                return NotFound();
            }

            var result = new SingleMatchViewModel()
            {
                MatchName = match.Name,
                StartDate = match.StartDate,
                Bets = match.Bets.Select(x => new Bet
                {
                    Name = x.Name,
                    Id = x.Id,
                    IsLive = x.IsLive,
                    MatchId = x.MatchId,
                    Odds = x.Odds.Select(o => new Odd { Id = o.Id, SpecialBetValue = o.SpecialBetValue, BetId = o.BetId, Value = o.Value }).ToList()
                }).ToList()
            };

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
