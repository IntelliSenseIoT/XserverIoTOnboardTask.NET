using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XserverIoTOnboardTask
{
    public class OnboardTaskProperties
    {
        /// <summary>
        /// Specifies an example name used by the application.
        /// Replace this property with an application-specific setting.
        /// </summary>
        public string ExampleName { get; set; } = string.Empty;

        /// <summary>
        /// Enables or disables the example functionality.
        /// Replace this property with an application-specific setting.
        /// </summary>
        public bool ExampleEnabled { get; set; }  = true;

       
    } 
}
