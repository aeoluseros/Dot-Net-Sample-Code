using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Udemy_Windows_Service
{
    public partial class UdemyWindowsService : ServiceBase
    {
        //mLogger is an interface
        private ILog mLogger;

        public UdemyWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Be default, every windows service is capable of writing the event to the event log.
            EventLog.WriteEntry("Udemy Windows Service by Robin is starting", EventLogEntryType.Information);

            ConfigureLog4Net();

            //launch the just-in-time debugger on the run-time.
            //to debug the windows service, the installed one needs to be in "stopped" status, and use "start" to start debugger.
            //System.Diagnostics.Debugger.Launch(); // we need to build the project in Debug mode.

            int i = 0;
            do
            {
                //output in the 'Output' window.
                System.Diagnostics.Debug.WriteLine(string.Format("Value of i is {0}", i));

                //mLogger is an interface
                mLogger.Info(string.Format("Value of i is: {0}", i));
                mLogger.Warn(string.Format("Value of i is: {0}", i));
                if (mLogger.IsDebugEnabled)
                {
                    mLogger.Debug(string.Format("Value of i is: {0}", i));
                }
            } while (i++ < 5);

            //if we set set the logging level as 'ERROR' in app.config
            mLogger.Error("this is an error");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("Udemy Windows Service by Robin is stopping", EventLogEntryType.Information);
        }

        private void ConfigureLog4Net()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();  //read setting from app.config and load to memory
                mLogger = LogManager.GetLogger("servicelog"); //the logger name defined in app.config
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
