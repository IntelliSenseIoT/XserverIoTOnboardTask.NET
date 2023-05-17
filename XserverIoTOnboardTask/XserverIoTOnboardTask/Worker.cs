using IO.NET.SimpleHttpServer;
using Microsoft.Extensions.Hosting.Systemd;
using Service.Common.NET;
using XserverIoTCommon;

namespace XserverIoTOnboardTask
{
    /// <summary>
    /// Use the code below to easily write an OnboardTask to Xserver.IoT framework.
    /// Created by IntelliSense Ltd.
    /// Website: http://www.intellisense-iot.com/
    /// </summary>

    public class Worker : BackgroundService
    {
        #region XServerIoTOnboardTask service settings
        //Service display name
        private const string ServiceDisplayName = "Xserver.OnboardTask";
        //Task Handler Period (ms)
        private const int TaskHandlerPeriod = 1000;
        #endregion

        #region Helpers
        HttpRestServerService RestServer = new HttpRestServerService();
        Application serviceinfo = new Application();
        #endregion

        #region Service Logger
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        #endregion

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var StartupTimeUTC = DateTime.UtcNow;
            _logger.LogInformation("Start "+ ServiceDisplayName + " service - " + StartupTimeUTC);
            var appinfo = serviceinfo.GetInfo();
            _logger.LogInformation("Version: " + appinfo.ApplicationVersion);

            //Start initializing
            Initialize();

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Start OnboardTask Service initialize
        /// </summary>
        private async void Initialize()
        {
            await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Start initializing...");

            try
            {
                //Initialize Http REST server
                await RestServer.HttpRESTServerStart();
                RestServer.ClientEvent += HttpRestServer_ClientRequestEvent;

                await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Checking services...");
                _logger.LogInformation("Checking services...");
                bool exit = false;
                while (exit == false)
                {
                    var com = await Services.ComIsInitialized();
                    var data = await Services.DataIsInitialized();
                    var core = await Services.CoreIsInitialized();
                    if (com.Initialized == true && data.Initialized == true && core.Initialized == true)
                    {
                        exit = true;
                        await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Services are running.");
                        _logger.LogInformation("Services are running.");
                    }
                    await Task.Delay(5000);
                }

                //Todo: Write your initial code here
                //_logger.LogInformation("Debug message");
            }
            catch (Exception ex)
            {
                await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Initialization exception error! Error: " + ex.Message);
            }

            await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Finished initialization.");
        }

        /// <summary>
        /// Periodic Task
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //Todo: Type your task code here
                    //_logger.LogInformation("Debug, Task running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "OnboardTask exception error! Error: " + ex.Message);
                }

                await Task.Delay(TaskHandlerPeriod, stoppingToken);
            }
        }

        private async void HttpRestServer_ClientRequestEvent(object sender, HttpRestServerService.ClientRequestEventArgs e)
        {
            IO.NET.SimpleHttpServer.Result res = new IO.NET.SimpleHttpServer.Result();

            try
            {
                if (e.RequestMethod == RequestMethodType.GET)
                {
                    //Todo: Type your code here
                    //_logger.LogInformation("Debug message");
                    // Example:
                    //if (e.uriString.ToLower() == "/onboardtask/examplegeturi")
                    //{
                    //    string content = JsonConvert.SerializeObject(YourObject);
                    //    res = await RestServer.ServerResponse(HTTPStatusCodes.OK, e.OStream, content);
                    //}
                }
                else if (e.RequestMethod == RequestMethodType.POST)
                {
                    //Todo: Type your code here
                    //_logger.LogInformation("Debug message");
                    // Example:
                    //if (e.uriString.ToLower() == "/onboardtask/exampleposturi")
                    //{  
                    //    YourObject MyObj = JsonConvert.DeserializeObject<YourObject>(e.HttpContent);
                    //    ....
                    //    string content = JsonConvert.SerializeObject(answer);
                    //    res = await RestServer.ServerResponse(HTTPStatusCodes.OK, e.OStream, content);
                    //}
                }
                else
                {
                    res = await RestServer.ServerResponse(HTTPStatusCodes.Not_Found, e.OStream, null);
                }
            }
            catch (Exception ex)
            {
                await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Http REST server exception error! Error: " + ex.Message);
            }
        }
    }
}