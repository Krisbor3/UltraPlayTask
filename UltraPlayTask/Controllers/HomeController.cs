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

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
