﻿using CQELight.Abstractions.Configuration;
using CQELight.Abstractions.Dispatcher;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.MSMQ.Common;
using CQELight.Configuration;
using CQELight.Tools;
using System;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;

namespace CQELight.Buses.MSMQ.Client
{
    /// <summary>
    /// MSMQ client bus instance. It uses specific configuration to push on a 
    /// MSMQ instance.
    /// </summary>
    public class MSMQClientBus : IDomainEventBus
    {

        #region Members

        private static MSMQClientBusConfiguration _configuration;
        private readonly IDispatcherSerializer _serializer;
        private readonly AppId _appId;

        #endregion

        #region Ctor

        internal MSMQClientBus(IAppIdRetriever appIdRetriever, IDispatcherSerializer serializer, MSMQClientBusConfiguration configuration = null)
        {
            if (appIdRetriever == null)
            {
                throw new ArgumentNullException(nameof(appIdRetriever));
            }
            _configuration = configuration ?? MSMQClientBusConfiguration.Default;
            _serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
            _appId = appIdRetriever.GetAppId();
        }

        #endregion

        #region IDomainEventBus

        /// <summary>
        /// Register asynchronously an event to be processed by the bus.
        /// </summary>
        /// <param name="event">Event to register.</param>
        /// <param name="context">Context associated to the event..</param>
        public Task PublishEventAsync(IDomainEvent @event, IEventContext context = null)
        {
            if (@event != null)
            {
                var queue = Helpers.GetMessageQueue(_configuration.QueueName);

                var evtCfg = _configuration.EventsLifetime.FirstOrDefault(t => new TypeEqualityComparer().Equals(t.Type, @event.GetType()));
                TimeSpan? expiration = null;
                if (evtCfg.Expiration.TotalMilliseconds > 0)
                {
                    expiration = evtCfg.Expiration;
                }

                var serializedEvent = _serializer.SerializeEvent(@event);
                var enveloppe = expiration.HasValue
                    ? new Enveloppe(serializedEvent, @event.GetType(), _appId, true, expiration.Value)
                    : new Enveloppe(serializedEvent, @event.GetType(), _appId);

                var message = new Message(enveloppe)
                {
                    TimeToBeReceived = enveloppe.Expiration,
                    Formatter = new JsonMessageFormatter()
                };
                queue.Send(message);
            }
            return Task.CompletedTask;
        }

        #endregion

    }
}