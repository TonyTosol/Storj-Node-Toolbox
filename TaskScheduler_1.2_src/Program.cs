using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;

namespace TaskScheduler
{
    static class Program
    {
        private static void ShowHelp()
        {
            MessageBox.Show("Valid parameters:" + Environment.NewLine +
                "-i, --install: \tInstall service" + Environment.NewLine +
                "-u, --uninstall: \tRemove service" + Environment.NewLine +
                "-s, --start: \t\tStart service" + Environment.NewLine +
                "-t, --stop: \t\tStop service" + Environment.NewLine +
                "-h, --help: \tShow this message",
                "Task Scheduler", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            bool install = false, uninstall = false, start = false, stop = false,  service = false;
            bool runConfiguration = true;
            try
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                        case "--install":
                            install = true; break;
                        case "-u":
                        case "--uninstall":
                            uninstall = true; break;
                        case "-s":
                        case "--start":
                            start = true; break;
                        case "-t":
                        case "--stop":
                            stop = true; break;
                        case "--service":
                            service = true; break;
                        default:
                            ShowHelp();
                            return 0;
                    }
                }

                if (uninstall)
                {
                    runConfiguration = false;
                    TaskSchedulerServiceAssistant.Uninstall();
                }

                if (install)
                {
                    runConfiguration = false;
                    TaskSchedulerServiceAssistant.Install();
                }

                if (start)
                {
                    runConfiguration = false;
                    TaskSchedulerServiceAssistant.StartService();
                }

                if (stop)
                {
                    runConfiguration = false;
                    TaskSchedulerServiceAssistant.StopService();
                }

                if (service)
                {
                    runConfiguration = false;
                    ServiceBase[] services = { new TaskSchedulerService() };
                    ServiceBase.Run(services);
                }

                if (runConfiguration)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Demo());
                }

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled Exception: \r\n" + ex.ToString(), "Task Scheduler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
    }
}