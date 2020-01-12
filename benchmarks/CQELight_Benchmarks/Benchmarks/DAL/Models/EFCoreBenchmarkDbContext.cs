using CQELight.DAL.EFCore;
using Microsoft.EntityFrameworkCore;

namespace CQELight_Benchmarks.Benchmarks.DAL.Models
{
    class EFCoreBenchmarkDbContext : BaseDbContext
    {

        #region Ctor

        public EFCoreBenchmarkDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        #endregion
    }
}
