using log4net;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Browser;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email.EmailAlertService;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Extensions;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Views
{
    /// <summary>
    /// MainForm class.
    /// View of the application.
    /// </summary>
    public partial class MainForm : Form, IKimsufiAvaibilityCheckerServiceListener
    {
        /// <summary>
        /// The logger used.
        /// </summary>
        private ILog Log = LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        /// Service for checking kimsufi server avaibility.
        /// </summary>
        protected IKimsufiAvaibilityCheckerService KimsufiService { get; set; }

        /// <summary>
        /// The internal cefsharp browser wrapper.
        /// </summary>
        protected IBrowserService Browser { get; set; }

        /// <summary>
        /// Gets or sets the BrowserForm used to retrieve informations.
        /// </summary>
        private BrowserForm browserForm { get; set; }

        /// <summary>
        /// The internal counter for the timer.
        /// It is used to know when to trigger the retrieval of the data from the webpage.
        /// </summary>
        private int internalTimerCount = 0;

        /// <summary>
        /// The timer used to time the retrieval operations.
        /// </summary>
        private System.Windows.Forms.Timer internalTimer { get; set; }

        /// <summary>
        /// The bindingList used for the bindingSource of <see cref="ServerStatus"/>.
        /// </summary>
        private BindingList<ServerStatus> internalServerStatus = new BindingList<ServerStatus>();
        
        /// <summary>
        /// The bindingsource used for displaying the list of <see cref="ServerStatus"/>.
        /// </summary>
        private BindingSource bsKimsufiServerStatus = new BindingSource() {};

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // Designer.
            InitializeComponent();

            // Initialisation.
            browserForm = new BrowserForm(this);
            KimsufiService = new KimsufiAvaibilityCheckerService(this, browserForm);
            internalTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };

            // Binding.
            Load += new EventHandler(MainForm_Load);
            FormClosed += new FormClosedEventHandler(MainForm_FormClosed);

            propertyGrid1.SelectedObject = Properties.Settings.Default;
            bsKimsufiServerStatus.DataSource = internalServerStatus;
            dataGridView1.DataSource = bsKimsufiServerStatus;

            DataGridViewColumn watchColumn = dataGridView1.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (watchColumn.Name == "Watch")
            {
                watchColumn.ReadOnly = false;
                watchColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            }

            internalTimer.Tick += new EventHandler(OnTimerTick);
            internalTimer.Start();
        }

        /// <summary>
        /// Event receiver for when the form closes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event.</param>
        protected void MainForm_FormClosed(object sender, EventArgs e)
        {
            Log.Debug("Form closed.");
        }

        /// <summary>
        /// Event receiver for when the form loads.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event.</param>
        protected void MainForm_Load(object sender, EventArgs e)
        {
            UpdateServerStatus();
        }

        #region Events

        /// <summary>
        /// Event receiver for the 'Refresh' button click.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        private void OnRefreshBtn_Click(object sender, EventArgs e)
        {
            // Launch the refresh of the server's status.
            this.internalTimerCount = int.Parse(Properties.Settings.Default.CheckAvailibilityEverySeconds) - 1;
            OnTimerTick(null, null);
        }

        /// <summary>
        /// Event receiver for the 'Toggle Browser' button click.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        private void OnToggleBrowser_Click(object sender, EventArgs e)
        {
            // Toggle the visibility (=size in this case.) of the browser form.
            browserForm.WindowState = (browserForm.WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal);
        }

        #endregion Events

        /// <summary>
        /// Triggers the retrieval of KimsufiAvaibility data.
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            this.internalTimerCount = (this.internalTimerCount + 1) % (int.Parse(Properties.Settings.Default.CheckAvailibilityEverySeconds));
            if (this.internalTimerCount == 0)
            {
                UpdateServerStatus();
            }
            else
            {
                SetProgramStatus(string.Format(Resources.StatusMessageWaiting, int.Parse(Properties.Settings.Default.CheckAvailibilityEverySeconds) - internalTimerCount));
            }
        }

        /// <summary>
        /// Updates the server status against the remote source.
        /// </summary>
        private void UpdateServerStatus()
        {
            SetProgramStatus(string.Format(Resources.StatusMessageDownloading, Properties.Settings.Default.KimsufiPageUrl));
            KimsufiService.RetrieveKimsufiData(Properties.Settings.Default.KimsufiPageUrl);
        }

        #region IKimsufiCheckerListener implementation

        /// <summary>
        /// Updates the program status.
        /// </summary>
        /// <param name="status">The status to show to the user.</param>
        public void SetProgramStatus(string status)
        {
            this.InvokeOnUiThreadIfRequired(() => currentOperationLabel.Text = status);
        }

        /// <summary>
        /// Updates the list of <see cref="KimsufiServerStatus"/>.
        /// </summary>
        /// <param name="serverStatus">The list of <see cref="KimsufiServerStatus"/>.</param>
        public void SetKimsufiServerStatus(IList<ServerStatus> serverStatus)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                foreach (ServerStatus item in serverStatus)
                {
                    // Does the item already exists in the binding source ?
                    var listEnumerator = internalServerStatus.GetEnumerator();
                    bool isEdited = false;
                    while (listEnumerator.MoveNext())
                    {
                        ServerStatus colItem = listEnumerator.Current;
                        if (colItem.Name == item.Name && colItem.GeographicZone == item.GeographicZone)
                        {
                            bool shouldAlert = false;
                            // Check alerts (if we watch a specific server, that was previously unavailable and is now available).
                            if (colItem.Watch && colItem.IsAvailable == false && item.IsAvailable == true)
                            {
                                shouldAlert = true;
                            }

                            // Perform item update.
                            colItem.GeographicZone = item.GeographicZone;
                            colItem.IsAvailable = item.IsAvailable;
                            colItem.Memory = item.Memory;
                            colItem.Network = item.Network;
                            colItem.Price = item.Price;
                            colItem.Processor = item.Processor;
                            colItem.ProcessorDetails = item.ProcessorDetails;
                            colItem.Storage = item.Storage;

                            isEdited = true;

                            if (shouldAlert)
                            {
                                AlertFor(colItem);
                            }
                        }
                    }

                    if (!isEdited)
                    {
                        internalServerStatus.Add(item);
                    }
                }
            });
        }

        public void AlertFor(ServerStatus serverStatus)
        {
            if (Properties.Settings.Default.EnableAlerts)
            {
                // Spawning new thread for MessageBox in order not to block parsing thread.
                Thread t = new Thread(() => MessageBox.Show(string.Format("Server {0} is now available.", serverStatus.Name),
                    "Server available",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information));
                t.Start();
            }
            else if (Properties.Settings.Default.EnableEmailAlerts)
            {
                IEmailAlertService emailAlertService = new EmailAlertService();
                emailAlertService.SendEmailAlert(serverStatus, Properties.Settings.Default.Email);
            }
            else
            {
                Log.Info("An alert for " + serverStatus.Name + " was triggered, but no alert was enabled.");
            }
        }

        #endregion IKimsufiCheckerListener implementation

        /// <summary>
        /// Event receiver for a double click on the datagridview for the <see cref="ServerStatus"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;
            internalServerStatus[row].Watch = !internalServerStatus[row].Watch;
            Log.InfoFormat("Alert {0} on server {1}", (internalServerStatus[row].Watch ? "enabled" : "disabled"), internalServerStatus[row].Name);
        }

        /// <summary>
        /// Event receiver for a click on the datagridview for the <see cref="ServerStatus"/>.
        /// Used to handle the alerts.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            // If click on the column 'Watch'
            if (column == 0)
            {
                internalServerStatus[row].Watch = !internalServerStatus[row].Watch;
                Log.InfoFormat("Alert {0} on server {1}", (internalServerStatus[row].Watch ? "enabled" : "disabled"), internalServerStatus[row].Name);
            }
        }
    }
}
