using log4net;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Properties;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Services.Browser;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Views;
using System;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation
{
    /// <summary>
    /// Programm class.
    /// Used to manage all the application lifecycle.
    /// </summary>
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger("Program");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Info("Application started.");

            // Application configuration.
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Main window creation and configuration.
            MainForm form = new MainForm();
            form.Text = Resources.ApplicationTitle;

            // Browser initialisation.
            BrowserService.Init();

            // Email configuration
            EmailService.SmtpServer = Properties.Settings.Default.SMTPIp;
            EmailService.SmtpPort = (int.Parse(Properties.Settings.Default.SMTPPort));
            EmailService.SmtpUsername = Properties.Settings.Default.SMTPUsername;
            EmailService.SmtpPassword = Properties.Settings.Default.SMTPPassword;
            EmailService.SmtpEmailFrom = Properties.Settings.Default.Email;

            // Launch the MainForm.
            System.Windows.Forms.Application.Run(form);

            // Browser Destruction.
            BrowserService.Exit();

            Log.Info("Application ended.");
        }
    }
}
