using CQELight.Buses.AzureServiceBus.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses.AzureServiceBus.Subscriber
{
    /// <summary>
    /// Azure Service Bus subscriber configuration.
    /// </summary>
    public sealed class AzureServiceBusSubscriberConfiguration : BaseSubscriberConfiguration
    {
        #region Properties

        /// <summary>
        /// Azure Service Bus Global configuration.
        /// </summary>
        public AzureServiceBusConfiguration Configuration { get; internal set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of <see cref="AzureServiceBusSubscriberConfiguration"/>.
        /// </summary>
        /// <param name="configuration">Global service configuration to use.</param>
        public AzureServiceBusSubscriberConfiguration(
            AzureServiceBusConfiguration configuration)
            :base(configuration.ServiceName)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion
    }
}
