using Microsoft.EntityFrameworkCore;

namespace CQELight.EventStore.EFCore.Common
{
    public class ArchiveEventStoreDbContext : DbContext
    {
        #region Ctor

        public ArchiveEventStoreDbContext(DbContextOptions<ArchiveEventStoreDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        #endregion

        #region Overriden methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EventArchiveEntityTypeConfiguration());
        }

        #endregion

    }
}
