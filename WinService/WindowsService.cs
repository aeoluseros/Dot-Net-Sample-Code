using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Udemy_Windows_Service
{
    public partial class UdemyWindowsService : ServiceBase
    {
        //mLogger is an interface
        private ILog mLogger;

        private Timer mRepeatingTimer;
        private double mCounter;

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

            //if we set set the logging level as 'ERROR' in app.config, then only Error would be logged.
            mLogger.Error("this is an error");

            //public Timer(TimerCallback callback, object state, int dueTime, int period)
            //A TimerCallback delegate representing a method to be executed.
            //An object containing information to be used by the callback method, or null.
            //The amount of time to delay before callback is invoked, in milliseconds. Specify "Timeout.Infinite" to prevent the timer from starting. Specify zero (0) to start the timer immediately.
            //The time interval between invocations of callback, in milliseconds. Specify Timeout.Infinite to disable periodic signaling.
            mRepeatingTimer = new Timer(myTimerCallback, mRepeatingTimer, 1000, 1000);
            //Because the Timer class has the same resolution as the system clock, which is approximately 15 milliseconds 
            //on Windows 7 and Windows 8 systems, the callback delegate executes at intervals defined by the resolution of 
            //the system clock if period is less than the resolution of the system clock. If period is zero (0) or 
            //Timeout.Infinite and dueTime is not Timeout.Infinite, callback is invoked once; the periodic behavior of the 
            //timer is disabled, but can be re-enabled using the Change method.
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

        public void myTimerCallback(object objParam)
        //The method specified for callback should be reentrant, because it is called on ThreadPool threads. 
        //The method can be executed simultaneously on two thread pool threads if the timer interval is less than 
        //the time required to execute the method, or if all thread pool threads are in use and the method is queued 
        //multiple times.
        {
            mLogger.Debug(string.Format("value of counter is: {0}", mCounter++));
            System.Diagnostics.Debug.WriteLine(string.Format("Value of counter is {0}", mCounter));
        }
    }
}
