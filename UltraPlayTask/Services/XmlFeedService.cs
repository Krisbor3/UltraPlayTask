using System.Xml.Linq;
using UltraPlayTask.Infrastructure.IRepositories;
using UltraPlayTask.Infrastructure.Models;
using UltraPlayTask.Interfaces;

namespace UltraPlayTask.Services
{
    public class XmlFeedService
    {
        private readonly IXmlFeedRepository _xmlFeedRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<XmlFeedService> _logger;
        private readonly IConfiguration _config;
        private readonly IUpdateMessageService _updateMessageService;

        public XmlFeedService(IXmlFeedRepository xmlFeedRepo, IHttpClientFactory httpClientFactory, ILogger<XmlFeedService> logger, IConfiguration config, IUpdateMessageService updateMessageService)
        {
            _xmlFeedRepository = xmlFeedRepo;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = config;
            _updateMessageService = updateMessageService;
        }

        public async Task FetchAndProcessFeedAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync(_config["XmlUrl"]);
                var xmlDoc = XDocument.Parse(response);

                foreach (var sportElement in xmlDoc.Descendants("Sport"))
                {
                    var sportName = sportElement.Attribute("Name")?.Value;
                    var sport = await _xmlFeedRepository.GetSportByName(sportName);

                    if (sport == null)
                    {
                        sport = new Sport { Name = sportName };
                        await _xmlFeedRepository.AddSport(sport);
                    }
                    await FetchEvents(sportElement,sport);
                }
                _logger.LogInformation("Feed successfully processed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching and processing feed.");
            }
        }

        private async Task FetchEvents(XElement sportElement,Sport? sport)
        {
            foreach (var eventElement in sportElement.Descendants("Event"))
            {
                var eventName = eventElement.Attribute("Name")?.Value;
                var eventEntity = await _xmlFeedRepository.GetEventByIdAndName(sport.Id, eventName);

                if (eventEntity == null)
                {
                    eventEntity = new Event { Name = eventName, SportId = sport.Id };
                    await _xmlFeedRepository.AddEvent(eventEntity);
                }
                await FetchMatches(eventElement,eventEntity);
            }
        }

        private async Task FetchMatches(XElement eventElement,Event? eventEntity)
        {
            foreach (var matchElement in eventElement.Descendants("Match"))
            {
                var matchName = matchElement.Attribute("Name")?.Value;
                var startDate = DateTime.Parse(matchElement.Attribute("StartDate")?.Value);
                var matchType = matchElement.Attribute("MatchType")?.Value;
                var match = await _xmlFeedRepository.GetMatchByIdAndName(eventEntity.Id, matchName);

                if (match == null)
                {
                    match = new Match
                    {
                        Name = matchName,
                        StartDate = startDate,
                        MatchType = matchType,
                        EventId = eventEntity.Id
                    };
                    await _xmlFeedRepository.AddMatch(match);
                }
                else
                {
                    match.StartDate = startDate;
                    match.MatchType = matchType;
                    await _xmlFeedRepository.UpdateMatch(match);
                    await _updateMessageService.AddUpdateMessageAsync("Match", match.Id, "Update");
                }
                await FetchBets(matchElement,match);
            }
        }

        private async Task FetchBets(XElement matchElement,Match? match)
        {
            foreach (var betElement in matchElement.Descendants("Bet"))
            {
                var betName = betElement.Attribute("Name")?.Value;
                var isLive = bool.Parse(betElement.Attribute("IsLive")?.Value);
                var bet = await _xmlFeedRepository.GetBet(match.Id, betName, isLive);

                if (bet == null)
                {
                    bet = new Bet
                    {
                        Name = betName,
                        IsLive = isLive,
                        MatchId = match.Id
                    };
                    await _xmlFeedRepository.AddBet(bet);
                }
                await FetchOdds(betElement, bet);
            }
        }

        private async Task FetchOdds(XElement betElement, Bet? bet)
        {
            foreach (var oddElement in betElement.Descendants("Odd"))
            {
                var oddValue = decimal.Parse(oddElement.Attribute("Value")?.Value);
                var specialBetValue = oddElement.Attribute("SpecialBetValue") != null
                    ? (decimal?)decimal.Parse(oddElement.Attribute("SpecialBetValue")?.Value)
                    : null;

                var odd = await _xmlFeedRepository.GetOdd(bet.Id, oddValue, specialBetValue);

                if (odd == null)
                {
                    odd = new Odd
                    {
                        Value = oddValue,
                        SpecialBetValue = specialBetValue,
                        BetId = bet.Id
                    };
                    await _xmlFeedRepository.AddOdd(odd);
                }
                else
                {
                    odd.Value = oddValue;
                    await _xmlFeedRepository.UpdateOdd(odd);
                }
            }
        }
    }
}
