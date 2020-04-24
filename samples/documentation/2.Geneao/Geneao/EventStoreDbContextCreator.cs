using CQELight.EventStore;
using CQELight.EventStore.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Geneao
{
    public class EventStoreDbContextCreator : IDesignTimeDbContextFactory<EventStoreDbContext>
    {
        public EventStoreDbContext CreateDbContext(string[] args)
        {
            return new EventStoreDbContext(new DbContextOptionsBuilder<EventStoreDbContext>()
                        .UseSqlite("FileName=events.db", opts => opts.MigrationsAssembly(typeof(EventStoreDbContextCreator).Assembly.GetName().Name))
                        .Options, SnapshotEventsArchiveBehavior.Delete);
        }
    }
}
