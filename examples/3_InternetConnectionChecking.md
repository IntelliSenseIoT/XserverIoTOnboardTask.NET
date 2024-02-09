## Checking Internet connection with IoT server:

The IoT server sets a bit in the PLC to "1" if the Internet connection is working, and to "0" if the Internet connection is interrupted.

### Prerequisites:

  - Configure IoT Server with IoT Explorer (Create the Source. More details: [Field device (Power Meter, PLC) communication settings](https://www.intellisense-iot.com/online-iot-training))
  - Create OnboardTasks user group with Control privilege and create onboardtask user (password: onboardtask)
  - Create OnboardTask and install to IoT Server
  - Add a property to Device Twin (Syntax: Source>source name|quantity name)
  - Send desired properties to Device Twin and update desired properties on the IoT Device from Device Twin


![](/images/IoTExplorerSiemensPLC.png)

![](/images/IoTExplorerInternetCheckingTask.png)

### Code:

        #region Helpers
        //....
        Realtime RObj = new Realtime();
        #endregion

        ...
        
         /// <summary>
         /// Start OnboardTask Service initialize
         /// </summary>
         private async void Initialize()
         {
             _logger.LogInformation(ServiceDisplayName + " - " + "Start initializing...");
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
        
                 #region Login to Xserver.IoT Service
                 var res = await Authentication.Login("onboardtask", "onboardtask");
                 if (res.Success == false)
                 {
                     await EventLogging.AddLogMessage(MessageType.Error, this.GetType().Name + " - " + ServiceDisplayName + " - " + res.ErrorMessage);
                 }
                 #endregion
        
                 #region Gets List of Sources and Quantities
                 var result = await RObj.GetSourcesQuantities();
                 if (result.Success == false)
                 {
                     await EventLogging.AddLogMessage(MessageType.Error, this.GetType().Name + " - " + ServiceDisplayName + " - " + result.ErrorMessage);
                 }
                 #endregion
             }
             catch (Exception ex)
             {
                 await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Initialization exception error! Error: " + ex.Message);
             }
        
             InitFinished = true;
             await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Finished initialization.");
             _logger.LogInformation(ServiceDisplayName + " - " + "Finished initialization.");
         }
        
        ...

        Stopwatch InternetChecking = new Stopwatch();
        /// <summary>
        /// Periodic Task
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (InitFinished)
                    {
                        #region Checking Internet connection
                        if (InternetChecking.IsRunning == false)
                        {
                            InternetChecking.Reset();
                            InternetChecking.Start();
                        }
                        else
                        {
                            if (InternetChecking.Elapsed.TotalSeconds>60)
                            {
                                InternetChecking.Stop();
                                var pingres =await TCPIP.Ping("www.google.com");
                                if (pingres.Success == true)
                                {
                                    var writeresult = await RObj.WriteValue("Siemens PLC", "Internet Status", 1);
                                    _logger.LogInformation("Debug, Internet OK.");
                                }
                                else
                                {
                                    var writeresult = await RObj.WriteValue("Siemens PLC", "Internet Status", 0);
                                    _logger.LogInformation("Debug, Internet error!");
                                }
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "OnboardTask exception error! Error: " + ex.Message);
                }
        
                await Task.Delay(TaskHandlerPeriod, stoppingToken);
            }
        }

        ...
    
        


