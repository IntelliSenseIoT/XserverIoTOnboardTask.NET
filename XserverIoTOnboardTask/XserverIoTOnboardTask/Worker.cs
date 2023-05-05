using Microsoft.Extensions.Hosting.Systemd;

namespace XserverIoTOnboardTask
{
    public class Worker : BackgroundService
    {
        #region Service Logger
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        #endregion

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Start initializing
            Initialize();

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Start Core Service initialize
        /// </summary>
        private async void Initialize()
        {


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}