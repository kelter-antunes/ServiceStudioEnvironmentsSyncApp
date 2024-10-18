using System;
using System.Threading;
using System.Windows.Forms;

namespace ServiceStudioEnvironmentsSyncApp
{
    static class Program
    {
        // Define a unique name for the mutex. It's good practice to include your application's GUID or a unique string.
        private static readonly string MutexName = "Global\\ServiceStudioEnvironmentsSyncApp_844c1581-4bb0-4d29-96f2-d2a1dc6bf493";

        [STAThread]
        static void Main()
        {
            bool isNewInstance;

            // Attempt to create and acquire the Mutex
            using (Mutex mutex = new Mutex(true, MutexName, out isNewInstance))
            {
                if (isNewInstance)
                {
                    // No other instance is running
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    using (var context = new TrayApplicationContext())
                    {
                        Application.Run(context);
                    }

                    // Release the Mutex when the application exits
                }
                else
                {
                    // Another instance is already running
                    // Optionally, you can bring the existing instance to the foreground or notify the user

                    // For a tray application, you might not need to do anything.
                    MessageBox.Show("Another instance of the application is already running.", "Instance Already Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}