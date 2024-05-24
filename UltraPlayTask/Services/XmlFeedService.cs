using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using UltraPlayTask.Infrastructure;
using UltraPlayTask.Infrastructure.Models;

namespace UltraPlayTask.Services
{
    public class XmlFeedService
    {
        private readonly UltraPlayTaskDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public XmlFeedService(UltraPlayTaskDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task FetchAndProcessFeedAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7");
            var xmlDoc = XDocument.Parse(response);

            // Parse XML and update the database
            // Example for parsing Sports
            foreach (var sportElement in xmlDoc.Descendants("Sport"))
            {
                var sportName = sportElement.Attribute("Name")?.Value;
                var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Name == sportName);

                if (sport == null)
                {
                    sport = new Sport { Name = sportName };
                    _context.Sports.Add(sport);
                    await _context.SaveChangesAsync();
                }

                // Parse and process events
                foreach (var eventElement in sportElement.Descendants("Event"))
                {
                    var eventName = eventElement.Attribute("Name")?.Value;
                    var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Name == eventName && e.SportId == sport.Id);

                    if (eventEntity == null)
                    {
                        eventEntity = new Event { Name = eventName, SportId = sport.Id };
                        _context.Events.Add(eventEntity);
                        await _context.SaveChangesAsync();
                    }

                    // Parse and process matches
                    foreach (var matchElement in eventElement.Descendants("Match"))
                    {
                        var matchName = matchElement.Attribute("Name")?.Value;
                        var startDate = DateTime.Parse(matchElement.Attribute("StartDate")?.Value);
                        var matchType = matchElement.Attribute("MatchType")?.Value;
                        var match = await _context.Matches.FirstOrDefaultAsync(m => m.Name == matchName && m.EventId == eventEntity.Id);

                        if (match == null)
                        {
                            match = new Match
                            {
                                Name = matchName,
                                StartDate = startDate,
                                MatchType = matchType,
                                EventId = eventEntity.Id
                            };
                            _context.Matches.Add(match);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            match.StartDate = startDate;
                            match.MatchType = matchType;
                            _context.Matches.Update(match);
                            await _context.SaveChangesAsync();
                        }

                        // Parse and process bets
                        foreach (var betElement in matchElement.Descendants("Bet"))
                        {
                            var betName = betElement.Attribute("Name")?.Value;
                            var isLive = bool.Parse(betElement.Attribute("IsLive")?.Value);
                            var bet = await _context.Bets.FirstOrDefaultAsync(b => b.Name == betName && b.MatchId == match.Id && b.IsLive == isLive);

                            if (bet == null)
                            {
                                bet = new Bet
                                {
                                    Name = betName,
                                    IsLive = isLive,
                                    MatchId = match.Id
                                };
                                _context.Bets.Add(bet);
                                await _context.SaveChangesAsync();
                            }

                            // Parse and process odds
                            foreach (var oddElement in betElement.Descendants("Odd"))
                            {
                                var oddValue = decimal.Parse(oddElement.Attribute("Value")?.Value);
                                var specialBetValue = oddElement.Attribute("SpecialBetValue") != null
                                    ? (decimal?)decimal.Parse(oddElement.Attribute("SpecialBetValue")?.Value)
                                    : null;

                                var odd = await _context.Odds.FirstOrDefaultAsync(o => o.BetId == bet.Id && o.Value == oddValue && o.SpecialBetValue == specialBetValue);

                                if (odd == null)
                                {
                                    odd = new Odd
                                    {
                                        Value = oddValue,
                                        SpecialBetValue = specialBetValue,
                                        BetId = bet.Id
                                    };
                                    _context.Odds.Add(odd);
                                }
                                else
                                {
                                    odd.Value = oddValue;
                                    _context.Odds.Update(odd);
                                }
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
