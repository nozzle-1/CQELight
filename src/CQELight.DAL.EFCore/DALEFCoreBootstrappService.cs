using System;

namespace CQELight.DAL.EFCore
{
    internal class DALEFCoreBootstrappService : IBootstrapperService
    {
        #region IBoostrapperService

        public BootstrapperServiceType ServiceType => BootstrapperServiceType.DAL;

        public Action<BootstrappingContext> BootstrappAction { get; internal set; }

        #endregion
    }
}
