using Microsoft.Extensions.Hosting;

namespace UltraPlayTask.Business.Services
{
    public class FetchFeedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly XmlFeedService _xmlFeedService;

        public FetchFeedHostedService(XmlFeedService xmlFeedService)
        {
            _xmlFeedService = xmlFeedService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(FetchFeed, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private async void FetchFeed(object state)
        {
            await _xmlFeedService.FetchAndProcessFeedAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
