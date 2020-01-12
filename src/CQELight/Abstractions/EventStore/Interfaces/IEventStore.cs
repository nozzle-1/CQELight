namespace CQELight.Abstractions.EventStore.Interfaces
{
    /// <summary>
    /// A global interface for complete eventStore.
    /// </summary>
    public interface IEventStore : IReadEventStore, IWriteEventStore
    {
    }
}
