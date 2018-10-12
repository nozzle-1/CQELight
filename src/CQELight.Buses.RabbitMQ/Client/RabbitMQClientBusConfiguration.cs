﻿using CQELight.Abstractions.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQELight.Buses.RabbitMQ.Client
{
    /// <summary>
    /// Configuration data for RabbitMQ bus.
    /// </summary>
    public class RabbitMQClientBusConfiguration : AbstractBaseConfiguration
    {
        #region Static members

        /// <summary>
        /// Default configuration that targets localhost for messaging.
        /// </summary>
        public static RabbitMQClientBusConfiguration Default
            => new RabbitMQClientBusConfiguration("localhost", "guest", "guest");
        
        #endregion

        #region Ctor

        /// <summary>
        /// Create a new client configuration on a rabbitMQ server.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="userName">The username to use.</param>
        /// <param name="password">The password to use.</param>
        /// <param name="eventsLifetime">Collection of relation between event type and lifetime. You should fill this collection to 
        /// indicates expiration date for some events.</param>
        public RabbitMQClientBusConfiguration(string host, string userName, string password,
            IEnumerable<EventLifeTimeConfiguration> eventsLifetime = null)
            : base(host, userName, password, eventsLifetime)
        {
        }

        #endregion

    }
}