using JackSite.Identity.Server.Interfaces;

namespace JackSite.Identity.Server.Services.BackgroundServices
{
    public class UserSyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserSyncBackgroundService> _logger;
        private readonly TimeSpan _syncInterval;

        public UserSyncBackgroundService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger<UserSyncBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;

            _syncInterval = TimeSpan.FromMinutes(
                minutes: _configuration.GetValue<int>("ExternalUserSync:IntervalMinutes", 60));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("User sync background service is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Starting scheduled user sync");

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var syncService = scope.ServiceProvider.GetRequiredService<IUserSyncService>();
                    await syncService.SyncAllExternalUsersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during scheduled user sync");
                }

                _logger.LogInformation("Scheduled user sync completed. Next sync in {SyncInterval} minutes",
                    _syncInterval.TotalMinutes);

                await Task.Delay(_syncInterval, stoppingToken);
            }

            _logger.LogInformation("User sync background service is stopping");
        }
    }
}