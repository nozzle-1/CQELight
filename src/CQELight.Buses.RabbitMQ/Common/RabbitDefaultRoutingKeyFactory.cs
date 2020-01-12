using CQELight.Buses.RabbitMQ.Common.Abstractions;

namespace CQELight.Buses.RabbitMQ.Common
{
    /// <summary>
    /// Default routing key factory that uses conventions.
    /// </summary>
    public sealed class RabbitDefaultRoutingKeyFactory : IRoutingKeyFactory
    {
        #region IRoutingKeyFactory

        public string GetRoutingKeyForCommand(object command)
            => command.GetType().Namespace.Split('.')[0];

        public string GetRoutingKeyForEvent(object @event)
            => "";

        #endregion
    }
}
