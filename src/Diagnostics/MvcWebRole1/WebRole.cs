using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace MvcWebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Directories.ScheduledTransferPeriod = TimeSpan.FromSeconds(30);
            config.Directories.BufferQuotaInMB = 100;

            config.WindowsEventLog.DataSources.Add("Application!*");
            config.WindowsEventLog.DataSources.Add("System!*");
            config.WindowsEventLog.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            config.WindowsEventLog.BufferQuotaInMB = 100;

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);

            return base.OnStart();
        }
    }
}
