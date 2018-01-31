using System.ComponentModel;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server
{
    /// <summary>
    /// ServerStatus class.
    /// Used to represent the status of a Kimsufi server.
    /// </summary>
    public class ServerStatus : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets whether or not to watch the status of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        private bool watch = false;
        public bool Watch
        {
            get { return watch; }
            set {
                watch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Watch"));
            }
        }


        /// <summary>
        /// Gets or sets the Name of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the GeographicZone of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string GeographicZone { get; set; }

        /// <summary>
        /// Gets or sets the Processor of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Processor { get; set; }

        /// <summary>
        /// Gets or sets the Processor details of the <see cref="KimsufiServerStatus"/> like the number of core, threads and frequency.
        /// </summary>
        public string ProcessorDetails { get; set; }

        /// <summary>
        /// Gets or sets the Memory of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Memory { get; set; }

        /// <summary>
        /// Gets or sets the Storage of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Storage { get; set; }

        /// <summary>
        /// Gets or sets the Network of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Network { get; set; }

        /// <summary>
        /// Gets or sets the Price of the <see cref="KimsufiServerStatus"/>.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets whether the Kimsufi server represented in the <see cref="KimsufiServerStatus"/> is available.
        /// </summary>
        public bool IsAvailable { get; set; }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation
    }
}
