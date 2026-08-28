using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XserverIoTOnboardTask
{
    public class GeneralSettings
    {
        //The OnboardTask login name and the password required for login.
        public string OnboardTaskLoginName { get; set; } = string.Empty;
        public string OnboardTaskPassword { get; set; } = string.Empty;
        //Task Handler Period (ms)
        public int TaskHandlerPeriod { get; set; }
      
        //For Debug
        //False = No, True = Test mode
        public bool IsTestMode { get; set; } = false;
        //False=Azure, True = Local
        public bool LocalConnect { get; set; } = false;
        public string IP { get; set; } = string.Empty;
        //Azure IoT Device ID
        public string ConnectedIoTDeviceID { get; set; } = string.Empty;
        //Azure IoT Hub Connection string
        public string IoTHubConnectionString { get; set; } = string.Empty;
    }
}
