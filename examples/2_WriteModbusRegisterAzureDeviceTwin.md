## Example 2 (Write Modbus register via Azure Device Twin):

The example below shows how it can be write a Modbus register via Azure Device Twin.

### Prerequisites:

  - Configure IoT Server with IoT Explorer
  - Create OnboardTasks user group with Control privilege and create onboardtask user (password: onboardtask)
  - Create OnboardTask and install to IoT Server
  - Add a property to Device Twin (Syntax: Source>source name|quantity name)
  - Send desired properties to Device Twin and update desired properties on the IoT Device from Device Twin

![](images/ConnectionTechOverview.png)

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
                //Initialize Http REST server
                await RestServer.HttpRESTServerStart();
                RestServer.ClientEvent += HttpRestServer_ClientRequestEvent;

                #region Checking services
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
                #endregion

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

            await EventLogging.AddLogMessage(MessageType.Info, this.GetType().Name + " - " + ServiceDisplayName + " - " + "Finished initialization.");
        }
        
        ...

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
                    //_logger.LogInformation("Received request");

                    if (e.uriString.ToLower() == "/onboardtask/desiredpropertyupdatecallback")
                    {
                        res = await RestServer.ServerResponse(HTTPStatusCodes.OK, e.OStream, null);

                        //Gets Desired properties from IoT Server Onboard storage
                        var DesiredPropertiesFromOnboard = await XserverIoTCommon.DeviceTwin.GetDesiredProperties();
                        //Information: DesiredPropertiesFromContent and DesiredPropertiesFromOnboard are equal, because UpdateCallBack event saves Desired properties into the Onboard storage also.

                        List<DeviceTwinProperty> NewReportedProperties = new List<DeviceTwinProperty>();

                        foreach (var item in DesiredPropertiesFromOnboard.DesiredProperties)
                        {
                            String Source =String.Empty;
                            String Quantity = String.Empty;
                            bool OKw = false;
                            try
                            {
                                if (item.Key.ToLower().IndexOf("source>") == 0)
                                {
                                    var Stype = item.Key.Split('>');
                                    var DevQuan = Stype[1].Split("|");

                                    if (string.IsNullOrEmpty(DevQuan[0]) == false && string.IsNullOrEmpty(DevQuan[1]) == false)
                                    {
                                        Source = DevQuan[0];
                                        Quantity = DevQuan[1];
                                        //_logger.LogInformation("Source:" + Source + " Quantity:" + Quantity);
                                        OKw = true;
                                    }
                                }
                            }
                            catch (Exception ex) 
                            {
                                OKw = false;
                                _logger.LogInformation("Exception [/onboardtask/desiredpropertyupdatecallback]: " + ex.Message);
                            }

                            if (OKw == true) 
                            {
                                var val = Convert.ToDouble(item.Value);
                                var writeresult = await RObj.WriteValue(Source, Quantity, val);
                            }
                            NewReportedProperties.Add(item);
                        }
                        //Saves New Reported properties into the Onboard storage and sends to Azure Device Twin also.
                        await XserverIoTCommon.DeviceTwin.SaveReportedProperties(NewReportedProperties);
                    }
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
        

