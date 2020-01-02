using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.AzureServiceBus;
using CQELight.Buses.AzureServiceBus.Client;
using CQELight.Buses.AzureServiceBus.Common;
using CQELight.Buses.AzureServiceBus.Subscriber;
using CQELight.IoC;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQELight
{
    public static class BootstrapperExt
    {
        #region Private members

        private static AzureServiceBusSubscriber s_subscriber;
        internal static Task s_postbootstrappStartTask;

        #endregion
        #region Public static methods        

        /// <summary>
        /// Use AzureServiceBus to publish events.
        /// </summary>
        /// <param name="bootstrapper">Bootstrapper instance</param>
        /// <param name="configuration">Azure service bus configuration</param>
        /// <returns>Bootstrapper instance</returns>
        [Obsolete("Use the UseAzureServiceBus method that have AzureServiceBusConfiguration parameter instead. Will be removed in 2.0")]
        public static Bootstrapper UseAzureServiceBus(this Bootstrapper bootstrapper, AzureServiceBusClientConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentException("Bootstrapper.UseAzureServiceBus() : Configuration should be provided.", nameof(configuration));
            }

            bootstrapper.AddIoCRegistration(new InstanceTypeRegistration(
                configuration,
                typeof(AzureServiceBusClientConfiguration)));

            bootstrapper.AddIoCRegistration(new TypeRegistration(
                typeof(AzureServiceBusClient),
                typeof(AzureServiceBusClient),
                typeof(IDomainEventBus)));

            bootstrapper.AddIoCRegistration(new FactoryRegistration(() =>
                new QueueClient(configuration.ConnectionString, "CQELight"), typeof(QueueClient), typeof(IQueueClient)));

            return bootstrapper;
        }

        /// <summary>
        /// Use Azure Service Bus as message bus.
        /// </summary>
        /// <param name="bootstrapper">Bootstrapper instance</param>
        /// <param name="configuration">Azure service bus configuration</param>
        /// <param name="subscriberConf">Configuration lambda for specific subscriber needs</param>
        /// <returns>Bootstrapper instance</returns>
        public static Bootstrapper UseAzureServiceBus(
            this Bootstrapper bootstrapper,
            AzureServiceBusConfiguration configuration,
            Action<AzureServiceBusSubscriberConfiguration> subscriberConf = null)
        {
            if (configuration == null)
            {
                throw new ArgumentException("Bootstrapper.UseAzureServiceBus() : Configuration must be provided.", nameof(configuration));
            }
            var service = new AzureServiceBusBootstrapperService();

            var subscriberConfiguration = new AzureServiceBusSubscriberConfiguration(configuration);

            service.BootstrappAction = ctx =>
            {
                subscriberConf?.Invoke(subscriberConfiguration);

                if (ctx.IsServiceRegistered(BootstrapperServiceType.IoC))
                {
                    bootstrapper.AddIoCRegistration(
                        new InstanceTypeRegistration(
                            configuration,
                            RegistrationLifetime.Singleton,
                            typeof(AzureServiceBusClientConfiguration)));

                    bootstrapper.AddIoCRegistration(new InstanceTypeRegistration(
                        subscriberConfiguration,
                        RegistrationLifetime.Singleton,
                        typeof(AzureServiceBusSubscriberConfiguration)));
                }
            };

            bootstrapper.OnPostBootstrapping += ctx =>
            {
                if (ctx.Scope != null)
                {
                    s_subscriber = ctx.Scope.Resolve<AzureServiceBusSubscriber>();
                }
                else
                {
                    var loggerFactory = new LoggerFactory();
                    loggerFactory.AddProvider(new DebugLoggerProvider());
                    s_subscriber = new AzureServiceBusSubscriber(
                        subscriberConfiguration,
                        loggerFactory,
                        null);
                }
                s_subscriber.Start();
            };

            bootstrapper.AddService(service);

            return bootstrapper;
        }

        #endregion

    }
}