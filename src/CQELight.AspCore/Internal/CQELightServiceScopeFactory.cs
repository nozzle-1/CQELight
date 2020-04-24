using CQELight.Abstractions.IoC.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQELight.AspCore.Internal
{
    class CQELightServiceScopeFactory : IServiceScopeFactory
    {
        #region Members

        private readonly IScopeFactory scopeFactory;

        #endregion

        #region Ctor

        public CQELightServiceScopeFactory(IScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        #endregion

        #region IServiceScopeFactory methods

        public IServiceScope CreateScope()
            => new CQELightServiceScope(scopeFactory);

        #endregion
    }
}
