using Microsoft.EntityFrameworkCore;

namespace CQELight.EventStore.EFCore.Common
{
    class EventArchiveBehaviorInfos
    {

        #region Properties

        public SnapshotEventsArchiveBehavior ArchiveBehavior { get; set; }
        public DbContextOptions<ArchiveEventStoreDbContext> ArchiveDbContextOptions { get; set; }

        #endregion

    }
}
