using System;
using System.Management;

namespace AppEventSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            var startQuery = new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace");
            var exitQuery = new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace");

            using (
                ManagementEventWatcher
                startWatcher = new ManagementEventWatcher(startQuery),
                exitWatcher = new ManagementEventWatcher(exitQuery)
                )
            {
                // subscribe to events
                startWatcher.EventArrived += new EventArrivedEventHandler(ProcessStarted);
                exitWatcher.EventArrived += new EventArrivedEventHandler(ProcessExited);

                startWatcher.Start();
                exitWatcher.Start();

                Console.WriteLine("Listening for process start and exit events. Press any key to exit...");
                Console.ReadKey();

                startWatcher.Stop();
                exitWatcher.Stop();
            }

        }

        private static void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            var log = $"Process {processName} started at {DateTime.Now}";
            Console.WriteLine(log);
        }

        private static void ProcessExited(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            var log = $"Process {processName} exited at {DateTime.Now}";
            Console.WriteLine(log);
        }
    }
}
