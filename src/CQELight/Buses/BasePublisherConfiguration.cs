using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.Buses
{
    /// <summary>
    /// Base class for any message publisher.
    /// </summary>
    public abstract class BasePublisherConfiguration : BaseBusConfiguration
    {
        #region Properties

        /// <summary>
        /// Configuration for events lifetimes. 
        /// </summary>
        public IEnumerable<EventLifeTimeConfiguration> EventsLifetime { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Base ctor that contains a collection of lifetime configuraiton.
        /// </summary>
        /// <param name="eventsLifetime">Collection of lifetime configuration.</param>
        public BasePublisherConfiguration(
            IEnumerable<EventLifeTimeConfiguration> eventsLifetime)
            :base ("CQELight_Defaults")
        {
            EventsLifetime = eventsLifetime;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BasePublisherConfiguration"/>.
        /// </summary>
        /// <param name="eventsLifetime">Collection of lifetime configuration.</param>
        /// <param name="serviceName">Name of the service that use this publisher bus.</param>
        public BasePublisherConfiguration(
            IEnumerable<EventLifeTimeConfiguration> eventsLifetime,
            string serviceName)
            : base(serviceName)
        {
            EventsLifetime = eventsLifetime;
        }

        #endregion
    }
}
