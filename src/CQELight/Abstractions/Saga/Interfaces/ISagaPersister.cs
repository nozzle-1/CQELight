using System;
using System.Threading.Tasks;

namespace CQELight.Abstractions.Saga.Interfaces
{
    /// <summary>
    /// Contract interface to persister of a saga.
    /// </summary>
    public interface ISagaPersister<T> where T : ISaga
    {
        /// <summary>
        /// Persist the saga asynchronously.
        /// </summary>
        /// <param name="saga">Saga to persist.</param>
        /// <param name="onErrorCallback">Callback method to fire if any persistence exception occures.</param>
        Task PersistSagaAsync(T saga, Action<T, Exception> onErrorCallback);
    }
}
