﻿using CQELight.Abstractions.Configuration;
using CQELight.Abstractions.Events;
using CQELight.Abstractions.Events.Interfaces;
using CQELight.Buses.MSMQ.Client;
using CQELight.Events.Serializers;
using CQELight.TestFramework;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CQELight.Buses.MSMQ.Integration.Tests
{
    public class MSMQServerTests : BaseUnitTestClass
    {

        #region Ctor & members

        private Mock<IAppIdRetriever> _serverAppId;
        private Guid _serverGuid = Guid.NewGuid();

        private Mock<IAppIdRetriever> _clientAppId;
        private Guid _clientGuid = Guid.NewGuid();

        public MSMQServerTests()
        {
            _serverAppId = new Mock<IAppIdRetriever>();
            _serverAppId.Setup(m => m.GetAppId())
                .Returns(new Configuration.AppId(_serverGuid));

            _clientAppId = new Mock<IAppIdRetriever>();
            _clientAppId.Setup(m => m.GetAppId())
                .Returns(new Configuration.AppId(_clientGuid));
            
        }

        private class TestEvent : BaseDomainEvent
        {
            public string Data { get; set; }
        }

        #endregion

        #region Working

        [Fact]
        public async Task MSMQServer_Working_Test()
        {
            Tools.CleanQueue();
            bool received = false;
            var server = new MSMQServer(_serverAppId.Object, new LoggerFactory(),
                configuration: new QueueConfiguration(new JsonDispatcherSerializer(), "", callback: o =>
                 {
                     if (o is TestEvent domainEvent)
                     {
                         received = domainEvent != null && domainEvent.Data == "test";
                     }
                 }));

            await server.StartAsync();

            var client = new MSMQClientBus(_clientAppId.Object, new JsonDispatcherSerializer());
            await client.PublishEventAsync(new TestEvent { Data = "test" }).ConfigureAwait(false);

            uint elapsed = 0;

            while (elapsed <= 2000 && !received) // 2sec should be enough
            {
                elapsed += 10;
                await Task.Delay(10);
            }
            received.Should().BeTrue();
        }

        #endregion

    }
}