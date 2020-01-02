using CQELight.Abstractions.CQS.Interfaces;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.RabbitMQ.Common;
using CQELight.Buses.RabbitMQ.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses.RabbitMQ.Subscriber
{
    /// <summary>
    /// Configuration for RabbitMQ subscriber.
    /// </summary>
    public class RabbitSubscriberConfiguration : BaseSubscriberConfiguration
    {
        #region Properites

        /// <summary>
        /// Informations for connection to RabbitMQ.
        /// </summary>
        public RabbitConnectionInfos ConnectionInfos { get; set; }

        /// <summary>
        /// Informations about the network configuration within RabbitMQ.
        /// </summary>
        public RabbitNetworkInfos NetworkInfos { get; set; }

        /// <summary>
        /// Strategy to consider for ack.
        /// </summary>
        public AckStrategy AckStrategy { get; set; } = AckStrategy.AckOnSucces;

        #endregion
    }
}
