using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses
{
    /// <summary>
    /// Base class for bus configuration
    /// </summary>
    public class BaseBusConfiguration
    {
        #region Properties

        /// <summary>
        /// Service name will be used for identifying message To/From fields.
        /// </summary>
        public string ServiceName { get;  }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="BaseBusConfiguration"/> instance.
        /// </summary>
        /// <param name="serviceName">Name of the current service.</param>
        public BaseBusConfiguration(
            string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("BaseBusConfiguration.ctor() : Application name should be provided", nameof(serviceName));
            }

            ServiceName = serviceName;
        }

        #endregion
    }
}
