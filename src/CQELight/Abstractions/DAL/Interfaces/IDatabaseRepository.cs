using System;

namespace CQELight.Abstractions.DAL.Interfaces
{
    /// <summary>
    /// Contract interface for repository upon database.
    /// </summary>
    public interface IDatabaseRepository : IDataReaderRepository, IDataUpdateRepository, IDisposable
    {
    }
}
