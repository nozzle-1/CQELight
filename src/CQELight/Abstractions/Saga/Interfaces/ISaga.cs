using CQELight.Abstractions.CQS.Interfaces;
using CQELight.Abstractions.Events.Interfaces;
using System;

namespace CQELight.Abstractions.Saga.Interfaces
{
    /// <summary>
    /// Contract interface for a Saga.
    /// </summary>
    public interface ISaga : IEventContext, ICommandContext
    {
        /// <summary>
        /// Unique Id of Saga.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Flag to indicates if saga is completed.
        /// </summary>
        bool Completed { get; }
    }
}
