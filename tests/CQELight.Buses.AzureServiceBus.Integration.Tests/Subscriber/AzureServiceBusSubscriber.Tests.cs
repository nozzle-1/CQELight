using CQELight.Abstractions.CQS.Interfaces;
using CQELight.Abstractions.DDD;
using CQELight.Abstractions.Events;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.AzureServiceBus.Subscriber;
using CQELight.Buses.InMemory.Commands;
using CQELight.Buses.InMemory.Events;
using CQELight.TestFramework;
using CQELight.TestFramework.IoC;
using CQELight.Tools.Extensions;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CQELight.Buses.AzureServiceBus.Integration.Tests.Subscriber
{
    public class AzureServiceBusSubscriberTests : BaseUnitTestClass
    {
        private const string QueuePath = "CQELight";
        #region Ctor & members

        private string ConnectionString;

        private readonly InMemoryEventBus eventBus = new InMemoryEventBus();
        private readonly InMemoryCommandBus commandBus = new InMemoryCommandBus();

        private readonly TestScopeFactory scopeFactory;
        private readonly ILoggerFactory loggerFactory;

        private readonly ManagementClient client;


        public AzureServiceBusSubscriberTests()
        {
            ConnectionString = new ConfigurationBuilder().AddEnvironmentVariables().Build()["CQELight_AzureServiceBusConnectionString"];
            client = new ManagementClient(ConnectionString);
            loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new DebugLoggerProvider());

            CleanQueues().GetAwaiter().GetResult();
            scopeFactory = new TestScopeFactory(new TestScope(new Dictionary<Type, object>
            {
                { typeof(InMemoryEventBus), eventBus },
                { typeof(InMemoryCommandBus), commandBus } }
            ));
            AzureServiceBusTestEventHandler.isCalled = false;
        }

        private AzureServiceBusSubscriberConfiguration GetConfiguration()
            => new AzureServiceBusSubscriberConfiguration(new Common.AzureServiceBusConfiguration(ConnectionString, QueuePath));

        private async Task CleanQueues()
        {
            if (await client.QueueExistsAsync(QueuePath))
            {
                var queue = await client.GetQueueAsync(QueuePath);
                if (queue != null)
                {
                    var receiver = new MessageReceiver(ConnectionString, QueuePath);
                    while (await receiver.PeekAsync() != null)
                    {
                        var m = await receiver.ReceiveAsync();
                        await receiver.CompleteAsync(m.SystemProperties.LockToken);
                    }
                }
            }
        }

        #endregion

        #region Events

        private class AzureServiceBusTestEvent : BaseDomainEvent
        {
            public string Data { get; set; }
        }

        private class AzureServiceBusTestEventHandler : IDomainEventHandler<AzureServiceBusTestEvent>
        {
            public static bool isCalled;
            public Task<Result> HandleAsync(AzureServiceBusTestEvent domainEvent, IEventContext context = null)
            {
                isCalled = true;
                return Result.Ok();
            }
        }

        [Fact]
        public async Task Event_Start_Should_Make_Subscriber_Begin_Listening_Callback()
        {
            try
            {
                bool eventReceived = false;
                var configuration = GetConfiguration();
                configuration.EventCustomCallback = (evt) => eventReceived = evt is AzureServiceBusTestEvent;
                var subscriber = new AzureServiceBusSubscriber(
                    configuration,
                    new LoggerFactory(),
                    scopeFactory);

                subscriber.Start();

                var queueClient = new QueueClient(ConnectionString, "CQELight");
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(new Enveloppe(new AzureServiceBusTestEvent
                {
                    Data = "TestData"
                }, "CQELight").ToJson()))
                {
                    ContentType = typeof(AzureServiceBusTestEvent).AssemblyQualifiedName
                });

                int awaitedTime = 0;
                while (!eventReceived && awaitedTime <= 2000)
                {
                    if (eventReceived) break;
                    await Task.Delay(500);
                    awaitedTime += 500;
                }

                eventReceived.Should().BeTrue();
            }
            finally
            {
                await CleanQueues();
            }
        }

        [Fact]
        public async Task Event_Start_Should_Make_Subscriber_Begin_Listening_ToHandler()
        {
            try
            {
                bool eventReceived = false;
                var configuration = GetConfiguration();
                configuration.DispatchInMemory = true;
                var subscriber = new AzureServiceBusSubscriber(
                    configuration,
                    new LoggerFactory(),
                    scopeFactory);

                subscriber.Start();

                var queueClient = new QueueClient(ConnectionString, "CQELight");
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(new Enveloppe(new AzureServiceBusTestEvent
                {
                    Data = "TestData"
                }, "CQELight").ToJson()))
                {
                    ContentType = typeof(AzureServiceBusTestEvent).AssemblyQualifiedName
                });

                int awaitedTime = 0;
                while (!eventReceived && awaitedTime <= 2000)
                {
                    if (eventReceived) break;
                    await Task.Delay(500);
                    awaitedTime += 500;
                }

                AzureServiceBusTestEventHandler.isCalled.Should().BeTrue();
            }
            finally
            {
                await CleanQueues();
            }
        }

        #endregion

        #region Commands

        private class AzureServiceBusTestCommand : ICommand
        {
            public string Data { get; set; }
        }

        private class AzureServiceBusTestCommandHandler : ICommandHandler<AzureServiceBusTestCommand>
        {
            public static bool isCalled;
            public Task<Result> HandleAsync(AzureServiceBusTestCommand command, ICommandContext context = null)
            {
                isCalled = true;
                return Result.Ok();
            }
        }

        [Fact]
        public async Task Command_Start_Should_Make_Subscriber_Begin_Listening_Callback()
        {
            try
            {
                bool commandReceived = false;
                var configuration = GetConfiguration();
                configuration.CommandCustomCallback = (cmd) => commandReceived = cmd is AzureServiceBusTestCommand;
                var subscriber = new AzureServiceBusSubscriber(
                    configuration,
                    new LoggerFactory(),
                    scopeFactory);

                subscriber.Start();

                var queueClient = new QueueClient(ConnectionString, "CQELight");
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(new Enveloppe(new AzureServiceBusTestCommand
                {
                    Data = "TestData"
                }, "CQELight").ToJson()))
                {
                    ContentType = typeof(AzureServiceBusTestCommand).AssemblyQualifiedName
                });

                int awaitedTime = 0;
                while (!commandReceived && awaitedTime <= 2000)
                {
                    if (commandReceived) break;
                    await Task.Delay(500);
                    awaitedTime += 500;
                }

                commandReceived.Should().BeTrue();
            }
            finally
            {
                await CleanQueues();
            }
        }

        [Fact]
        public async Task Command_Start_Should_Make_Subscriber_Begin_Listening_ToHandler()
        {
            try
            {
                bool commandReceived = false;
                var configuration = GetConfiguration();
                configuration.DispatchInMemory = true;
                var subscriber = new AzureServiceBusSubscriber(
                    configuration,
                    new LoggerFactory(),
                    scopeFactory);

                subscriber.Start();

                var queueClient = new QueueClient(ConnectionString, "CQELight");
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(new Enveloppe(new AzureServiceBusTestCommand
                {
                    Data = "TestData"
                }, "CQELight").ToJson()))
                {
                    ContentType = typeof(AzureServiceBusTestCommand).AssemblyQualifiedName
                });

                int awaitedTime = 0;
                while (!commandReceived && awaitedTime <= 2000)
                {
                    if (commandReceived) break;
                    await Task.Delay(500);
                    awaitedTime += 500;
                }

                AzureServiceBusTestCommandHandler.isCalled.Should().BeTrue();
            }
            finally
            {
                await CleanQueues();
            }
        }

        #endregion
    }
}
