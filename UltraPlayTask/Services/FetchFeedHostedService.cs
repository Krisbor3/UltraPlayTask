namespace UltraPlayTask.Services
{
    public class FetchFeedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _services;
        private readonly ILogger<FetchFeedHostedService> _logger;

        public FetchFeedHostedService(IServiceProvider services, ILogger<FetchFeedHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FetchFeedHostedService started.");
            _timer = new Timer(FetchFeed, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private async void FetchFeed(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var xmlFeedService = scope.ServiceProvider.GetRequiredService<XmlFeedService>();
                _logger.LogInformation("Fetching feed...");
                await xmlFeedService.FetchAndProcessFeedAsync();
                _logger.LogInformation("Feed fetched and processed.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FetchFeedHostedService stopped.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
