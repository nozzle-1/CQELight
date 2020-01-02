using CQELight.Abstractions.CQS.Interfaces;
using CQELight.Abstractions.DDD;
using CQELight.Abstractions.Dispatcher;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Abstractions.IoC.Interfaces;
using CQELight.Buses.AzureServiceBus.Common;
using CQELight.Buses.InMemory.Commands;
using CQELight.Buses.InMemory.Events;
using CQELight.Events.Serializers;
using CQELight.Tools.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQELight.Buses.AzureServiceBus.Subscriber
{
    class AzureServiceBusSubscriber
    {
        #region Members

        private readonly ILogger<AzureServiceBusSubscriber> logger;
        private readonly AzureServiceBusSubscriberConfiguration configuration;
        private readonly IScopeFactory scopeFactory;

        private ManagementClient client;
        private static bool s_started;

        #endregion

        #region Ctor

        public AzureServiceBusSubscriber(
            AzureServiceBusSubscriberConfiguration configuration,
            ILoggerFactory loggerFactory,
            IScopeFactory scopeFactory)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            logger = loggerFactory.CreateLogger<AzureServiceBusSubscriber>();
            this.scopeFactory = scopeFactory;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            if (s_started)
            {
                return;
            }
            s_started = true;
            try
            {
                client = new ManagementClient(configuration.Configuration.ConnectionString);
                if (client != null)
                {
                    if (!client.QueueExistsAsync(configuration.ServiceName).GetAwaiter().GetResult())
                    {
                        client.CreateQueueAsync(configuration.ServiceName).GetAwaiter().GetResult();
                    }
                    var queueClient = new QueueClient(configuration.Configuration.ConnectionString, configuration.ServiceName, ReceiveMode.PeekLock);
                    queueClient.RegisterMessageHandler(async (m, _) =>
                    {
                        try
                        {
                            var result = Result.Ok();
                            var stringBody = Encoding.UTF8.GetString(m.Body);
                            var enveloppe = stringBody.FromJson<Enveloppe>();
                            if (enveloppe != null)
                            {
                                if (enveloppe.Emiter == "")
                                {
                                    await queueClient.CompleteAsync(m.SystemProperties.LockToken).ConfigureAwait(false);
                                    return;
                                }
                                if (!string.IsNullOrWhiteSpace(enveloppe.Data) && !string.IsNullOrWhiteSpace(enveloppe.AssemblyQualifiedDataType))
                                {
                                    var objType = Type.GetType(enveloppe.AssemblyQualifiedDataType);
                                    if (objType != null)
                                    {
                                        var serializer = GetSerializerByContentType(m.ContentType);
                                        if (typeof(IDomainEvent).IsAssignableFrom(objType))
                                        {
                                            var evt = serializer.DeserializeEvent(enveloppe.Data, objType);
                                            try
                                            {
                                                configuration.EventCustomCallback?.Invoke(evt);
                                            }
                                            catch (Exception e)
                                            {
                                                logger.LogError(
                                                    $"Error when executing custom callback for event {objType.AssemblyQualifiedName}. {e}");
                                                result = Result.Fail();
                                            }
                                            if (scopeFactory != null)
                                            {
                                                using (var scope = scopeFactory.CreateScope())
                                                {
                                                    var bus = scope.Resolve<InMemoryEventBus>();
                                                    if (configuration.DispatchInMemory && bus != null)
                                                    {
                                                        result = await bus.PublishEventAsync(evt).ConfigureAwait(false);
                                                    }
                                                }
                                            }
                                        }
                                        else if (typeof(ICommand).IsAssignableFrom(objType))
                                        {
                                            var cmd = serializer.DeserializeCommand(enveloppe.Data, objType);
                                            try
                                            {
                                                configuration.CommandCustomCallback?.Invoke(cmd);
                                            }
                                            catch (Exception e)
                                            {
                                                logger.LogError(
                                                    $"Error when executing custom callback for command {objType.AssemblyQualifiedName}. {e}");
                                                result = Result.Fail();
                                            }
                                            if (scopeFactory != null)
                                            {
                                                using (var scope = scopeFactory.CreateScope())
                                                {
                                                    var bus = scope.Resolve<InMemoryCommandBus>();
                                                    if (configuration.DispatchInMemory && bus != null)
                                                    {
                                                        result = await bus.DispatchAsync(cmd).ConfigureAwait(false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                logger.LogWarning("Retrieved an empty enveloppe from Azure Service Bus message. Message string body = " + stringBody);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.LogError("Erreur when treating message from message bus : " + e.ToString());
                            await queueClient.DeadLetterAsync(m.MessageId, "Exception : " + e.ToString()).ConfigureAwait(false);
                        }
                    },
                    (e) =>
                    {
                        if (e.Exception != null)
                        {
                            logger.LogError("Global exception within Azure Service Bus treatment :  " + e.ToString());
                        }
                        return Task.CompletedTask;
                    });
                }
            }
            catch (Exception e)
            {
                s_started = false;
                logger.LogError("Error when starting CQELight Azure Service Bus Subscriber. " + e.ToString());
                throw;
            }
        }

        #endregion


        #region Private methods

        private IDispatcherSerializer GetSerializerByContentType(string contentType)
        {
            switch (contentType)
            {
                case "text/json":
                default:
                    return new JsonDispatcherSerializer();
            }
        }

        #endregion
    }
}
