using CefSharp;
using CefSharp.WinForms;
using log4net;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Browser;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Extensions;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Properties;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Services.Browser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Views
{
    public partial class BrowserForm : Form, IBrowserService
    {
        /// <summary>
        /// The logger used.
        /// </summary>
        private ILog Log = LogManager.GetLogger(typeof(BrowserForm));

        /// <summary>
        /// Chromium browser control.
        /// </summary>
        private readonly ChromiumWebBrowser browser;

        /// <summary>
        /// The service listener.
        /// </summary>
        private readonly IKimsufiAvaibilityCheckerServiceListener serviceListener;

        /// <summary>
        /// Initializes a new instance of <see cref="BrowserForm"/> class.
        /// </summary>
        public BrowserForm(IKimsufiAvaibilityCheckerServiceListener serviceListener)
        {
            InitializeComponent();

            this.serviceListener = serviceListener;

            Text = "CefSharp";
            Visible = true;

            browser = new ChromiumWebBrowser("about:blank") {};
            FormClosed += new FormClosedEventHandler(BrowserForm_FormClosed);


            // Register BrowserCallbackForJs class as javascript callback.
            browser.RegisterJsObject("dotnetcallback", new BrowserCallbackForJs(this));

            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
        }

        /// <summary>
        /// Event receiver for the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BrowserForm_FormClosed(object sender, EventArgs e)
        {
            Log.Debug("Form closed.");
        }

        #region Browser

        /// <summary>
        /// Executes a javascript script on the browser.
        /// </summary>
        /// <param name="scriptPath">The path of the script.</param>
        private void LoadAndExecuteScript(string scriptPath)
        {
            string script;
            using (TextReader tr = new StreamReader(scriptPath))
            {
                script = tr.ReadToEnd();
            }

            browser.ExecuteScriptAsync(script);
        }

        /// <summary>
        /// Event handler for incoming javascript console messages.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        /// <summary>
        /// Event handler for incoming http messages.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        /// <summary>
        /// Event handler for browser loaded changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);


            // Inject scripts if we are on the correct Uri.
            // TODO: Find a way to send the URI in WinForm.Settings.
            if (!args.IsLoading && args.Browser.MainFrame.Url.StartsWith("https://www.kimsufi.com/fr/serveurs.xml"))
            {
                // Update Status.
                serviceListener.SetProgramStatus(Properties.Resources.StatusMessageDownloaded);

                // Inject prerequisites.
                LoadAndExecuteScript(@"Scripts/jQuery.js");

                // Inject Payload.
                LoadAndExecuteScript(@"Scripts/Payload.js");

                // Update status
                serviceListener.SetProgramStatus(Properties.Resources.StatusMessageParsing);
            }

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        /// <summary>
        /// Event handler for browser webpage title change.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            //this.InvokeOnUiThreadIfRequired(() => Text = string.Format("EtherSecret Scanner : {0} addresses parsed. {1} ", BrowserCallbackForJs.pages * 25, args.Title));
        }

        /// <summary>
        /// Event handler for browser webpage address change.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        /// <summary>
        /// Enables or Disables the 'Go back' button.
        /// </summary>
        /// <param name="canGoBack">If true, button is enabled.</param>
        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        /// <summary>
        /// Enables or Disables the 'Go forward' button.
        /// </summary>
        /// <param name="canGoBack">If true, button is enabled.</param>
        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        /// <summary>
        /// Enables or Disables the 'Status' indicator.
        /// </summary>
        /// <param name="canGoBack">If true, indicator is red.</param>
        private void SetIsLoading(bool isLoading)
        {
            goButton.Text = isLoading ? "Stop" : "Go";
            goButton.Image = isLoading ? Properties.Resources.nav_plain_red : Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        /// <summary>
        /// Sets the output message of the console.
        /// </summary>
        /// <param name="output">The output message.</param>
        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        /// <summary>
        /// Event handler for Resizing the HandleToolStripLayout control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        /// <summary>
        /// Resizes the HandleToolStripLayout control.
        /// </summary>
        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        /// <summary>
        /// Event handler for the Go button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        /// <summary>
        /// Event handler for the Go Back button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        /// <summary>
        /// Event handler for the Go Forward button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        /// <summary>
        /// Event handler for the Url textbox for keystrokes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        #endregion Browser

        #region IBrowserService implementation

        /// <summary>
        /// Loads an url if valid into the browser.
        /// </summary>
        /// <param name="url">The url of the page to load.</param>
        public void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                browser.Load(url);
            }
        }

        /// <summary>
        /// Updates the list of <see cref="ServerStatus"/> of the Kimsufi servers.
        /// </summary>
        /// <param name="serverStatus"></param>
        public void SetData(IList<ServerStatus> serverStatus)
        {
            serviceListener.SetProgramStatus(Resources.StatusMessageParsed);
            this.serviceListener.SetKimsufiServerStatus(serverStatus);
        }

        #endregion IBrowser implementation

        #region Menu

        /// <summary>
        /// Event handler for the Exit button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        /// <summary>
        /// Event handler for the View Console menu button
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void viewConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        #endregion Menu
    }
}
