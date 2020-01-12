using System;

namespace CQELight.Buses.AzureServiceBus
{
    class AzureServiceBusBootstrappService : IBootstrapperService
    {
        public BootstrapperServiceType ServiceType => BootstrapperServiceType.Bus;

        public Action<BootstrappingContext> BootstrappAction
        {
            get;
            internal set;
        }
    }
}