using CQELight.Abstractions.CQS.Interfaces;
using CQELight.Abstractions.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses
{
    /// <summary>
    /// Base class for bus subscriber configuration.
    /// </summary>
    public abstract class BaseSubscriberConfiguration : BaseBusConfiguration
    {
        /// <summary>
        /// Creates a new instance of <see cref="BaseSubscriberConfiguration"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the currently running service.</param>
        public BaseSubscriberConfiguration(string serviceName) 
            : base(serviceName)
        {
        }

        /// <summary>
        /// Flag that indicates if dead letter queue shoud be used.
        /// </summary>
        public bool UseDeadLetterQueue { get; set; }

        /// <summary>
        /// Flag that indicates if receveid ressource (event or command) should be dispatched on the in-memory buses.
        /// </summary>
        public bool DispatchInMemory { get; set; } = true;

        /// <summary>
        /// Custom callback when an event is received.
        /// </summary>
        public Action<IDomainEvent> EventCustomCallback { get; set; } = null;

        /// <summary>
        /// Custom callback when a command is received.
        /// </summary>
        public Action<ICommand> CommandCustomCallback { get; set; } = null;
    }
}
