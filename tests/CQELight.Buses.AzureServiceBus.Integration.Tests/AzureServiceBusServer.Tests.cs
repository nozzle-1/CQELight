﻿using CQELight.Abstractions.Events;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.AzureServiceBus.Client;
using CQELight.Buses.AzureServiceBus.Server;
using CQELight.Events.Serializers;
using CQELight.TestFramework;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CQELight.Buses.AzureServiceBus.Integration.Tests
{
    public class AzureServiceBusServerTests : BaseUnitTestClass
    {

        #region Ctor & members

        private const string CONST_APP_ID_SERVER = "BA3F9093-D7EE-4BB8-9B4E-EEC3447A89BA";
        private const string CONST_APP_ID_CLIENT = "AA3F9093-D7EE-4BB8-9B4E-EEC3447A89BA";

        private readonly Mock<ILoggerFactory> _loggerFactory;
        private readonly IConfiguration _configuration;

        private class AzureEvent : BaseDomainEvent
        {
            public string Data { get; set; }
        }

        public AzureServiceBusServerTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _loggerFactory = new Mock<ILoggerFactory>();
        }

        #endregion

        #region Working tests

        [Fact]
        public async Task AzureServiceBusServer_Working_AsExpected()
        {
            var queueClient = new QueueClient(_configuration["ConnectionString"], "cqelight");
            bool finished = false;
            var evtToSend = new AzureEvent { Data = "evt_data" };

            var server = new AzureServiceBusServer(CONST_APP_ID_SERVER, _loggerFactory.Object,
                new AzureServiceBusServerConfiguration(_configuration["ConnectionString"],
                new QueueConfiguration(new JsonDispatcherSerializer(), "cqelight", false,
                o =>
                {
                    if (o is IDomainEvent receivedEvt)
                    {
                        finished = receivedEvt.GetType() == typeof(AzureEvent) && (receivedEvt.As<AzureEvent>().Data) == "evt_data";
                    }
                })));


            var client = new AzureServiceBusClient(CONST_APP_ID_CLIENT, queueClient, new AzureServiceBusClientConfiguration(
                _configuration["ConnectionString"], null, null));
            await client.PublishEventAsync(evtToSend).ConfigureAwait(false);
            int currentWait = 0;
            while (!finished && currentWait <= 2000)
            {
                currentWait += 50;
                await Task.Delay(50).ConfigureAwait(false);
            }
            finished.Should().BeTrue();
        }

        #endregion

    }
}
