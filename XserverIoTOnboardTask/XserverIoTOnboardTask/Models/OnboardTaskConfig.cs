using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XserverIoTOnboardTask
{
    public class OnboardTaskConfig
    {
        /// <summary>
        /// Defines the application's operating mode.
        /// The available values and their behavior depend on the specific Onboard App implementation.
        /// </summary>
        public int MyApp_Settings_Mode { get; set; } = 0;

       
        /// <summary>
        /// Enables verbose diagnostics (additional logs, exceptions, timings).
        /// Use only in development or troubleshooting to avoid log noise.
        /// </summary>
        public bool MyApp_Settings_Debug { get; set; } = false;
    }
}
