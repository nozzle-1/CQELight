using CQELight.Buses.RabbitMQ.Common;
using RabbitMQ.Client;

namespace RabbitSample.Common
{
    public static class ConnectionInfosHelper
    {
        public static RabbitConnectionInfos GetConnectionInfos(string service)
            => RabbitConnectionInfos.FromConnectionFactory(new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            }, service);
    }
}
