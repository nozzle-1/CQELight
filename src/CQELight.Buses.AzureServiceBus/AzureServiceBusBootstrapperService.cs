using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CQELight.Buses.AzureServiceBus
{
    class AzureServiceBusBootstrapperService : IBootstrapperService
    {
        public BootstrapperServiceType ServiceType => BootstrapperServiceType.Bus;

        public Action<BootstrappingContext> BootstrappAction
        {
            get;
            internal set;
        }
    }
}