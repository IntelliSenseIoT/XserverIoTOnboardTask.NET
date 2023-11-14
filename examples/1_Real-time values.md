## Example 1 (Real-time - Access Sources and Quantites):

From OnboardTask supports to read and write the Quantites values of Sources with realtime methods of [XserverIoTCommon API](https://github.com/IntelliSenseIoT/XserverIoTOnboardTask.NET/blob/main/XserverIoTCommon.NET.md).

![](/images/RTtopology.png)

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
            await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Start initializing...");

            try
            {
                
                ...
               
                bool exit = false;
                while (exit == false)
                {
                    var com = await Services.ComIsInitialized();
                    var data = await Services.DataIsInitialized();
                    var core = await Services.CoreIsInitialized();
                    _logger.LogInformation("#Debug: Checked services");
                    if (com.Initialized == true && data.Initialized == true && core.Initialized == true)
                    {
                        exit = true;
                    }
                    await Task.Delay(5000);
                }

                #region Login to Xserver.IoT Service
                var res = await Authentication.Login("operator", "operator");
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

            await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Finished initialization.");
        }
        
        ...

        /// <summary>
        /// Periodic Task
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var Temp = await RObj.GetValue("TempSensor", "Temperature");
                   
                    if (Temp.Value <10)
                    {
                        var writeresult = await RObj.WriteValue("TempSensor", "SetPointValue", 20);
                    }
                    if (Temp.Value >= 10)
                    {
                        var writeresult = await RObj.WriteValue("TempSensor", "SetPointValue", 5);
                    }

                    _logger.LogInformation("Debug, Temp:" + Temp.Value.ToString());

                }
                catch (Exception ex)
                {
                    await EventLogging.AddLogMessage(MessageType.ExceptionError, this.GetType().Name + " - " + ServiceDisplayName + " - " + "OnboardTask exception error! Error: " + ex.Message);
                }

                await Task.Delay(TaskHandlerPeriod, stoppingToken);
            }
        }

        ...
        
