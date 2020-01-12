using System;

namespace CQELight.DAL.Interfaces
{
    /// <summary>
    /// Contract interface for repository upon database.
    /// </summary>
    /// <typeparam name="T">Type of database entity.</typeparam>
    [Obsolete("This IDatabaseRepository is not supported anymore. Migrate to DatabaseRepository with IDataReaderAdapter and IDataWriterAdapter")]
    public interface IDatabaseRepository<T> : IDataReaderRepository<T>, IDataUpdateRepository<T>, IDisposable
        where T : IPersistableEntity
    {
    }
}
