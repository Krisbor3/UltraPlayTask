using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UltraPlayTask.Interfaces;
using UltraPlayTask.Models;
using UltraPlayTask.Services;

namespace UltraPlayTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly XmlFeedService _xmlFeedService;
        private readonly IMatchService _matchService;

        public HomeController(ILogger<HomeController> logger,XmlFeedService service, IMatchService matchService)
        {
            _logger = logger;
            _xmlFeedService = service;
            _matchService = matchService;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch the matches starting in the next 24 hours from your service or database.
            var matches = await _matchService.GetMatchesStartingInNext24Hours();
            return View(matches);
        }

        //not used:
        private List<MatchViewModel> GetMatchesStartingInNext24Hours()
        {
            // This is a placeholder. Replace it with actual data fetching logic.
            return new List<MatchViewModel>
            {
            new MatchViewModel { Name = "Match 1", StartDate = DateTime.Now.AddHours(1) },
            new MatchViewModel { Name = "Match 2", StartDate = DateTime.Now.AddHours(2) },
            // Add more sample data as needed.
            };
        }

        public async Task<IActionResult> Privacy()
        {
            await this._xmlFeedService.FetchAndProcessFeedAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
