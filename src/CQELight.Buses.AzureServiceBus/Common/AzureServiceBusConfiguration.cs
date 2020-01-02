using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses.AzureServiceBus.Common
{
    /// <summary>
    /// Type of subscription used when creating Service Bus namespace
    /// </summary>
    public enum SubscriptionType
    {
        /// <summary>
        /// Basic
        /// </summary>
        Basic,
        /// <summary>
        /// Standard or Premium
        /// </summary>
        StandardOrPremium
    }

    /// <summary>
    /// Azure Service Bus Configuration
    /// </summary>
    public class AzureServiceBusConfiguration
    {
        #region Properties

        /// <summary>
        /// Connection string of the Azure Service Bus configuration.
        /// </summary>
        internal string ConnectionString { get; }
        /// <summary>
        /// Type of subscription in Azure Service Bus namespace.
        /// </summary>
        public SubscriptionType SubscriptionType { get; }
        /// <summary>
        /// Name of the current running service.
        /// </summary>
        public string ServiceName { get; }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="AzureServiceBusConfiguration"/> instance.
        /// </summary>
        /// <param name="connectionString">Connection string of the Azure Service Bus configuration.</param>
        /// <param name="serviceName">Name of the service that is currently using Azure Service Bus provider.</param>
        /// <param name="subscriptionType">Type of subscription in Azure Service Bus namespace.</param>
        public AzureServiceBusConfiguration(
            string connectionString,
            string serviceName,
            SubscriptionType subscriptionType = SubscriptionType.Basic
            )
        {
            if (string.IsNullOrWhiteSpace(connectionString) || !connectionString.StartsWith("Endpoint=sb"))
            {
                throw new ArgumentException("Connection string is not well formated", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("Service name must be provided (generally, it is the first part of your application namespace)", nameof(serviceName));
            }

            ConnectionString = connectionString;
            SubscriptionType = subscriptionType;
            ServiceName = serviceName;
        }

        #endregion
    }
}
