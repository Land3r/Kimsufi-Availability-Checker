using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Transversal.Exceptions
{
    /// <summary>
    /// Application Exception.
    /// </summary>
    [Serializable]
    public class AppException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        public AppException() { }

        /// <summary>
        /// Initializes a new instanc of the <see cref="AppException"/> class with a message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AppException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class with a message and an exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The exception exception.</param>
        public AppException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Intializes a new instance of the <see cref="AppException"/> class with SerializationInfo and StreamingContext.
        /// </summary>
        /// <param name="info">The serialization informations.</param>
        /// <param name="context">The streaming context.</param>
        protected AppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
