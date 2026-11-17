
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Innolux.EFCore
{
    /// <summary>
    /// public class InnoluxDbContext :
    /// </summary>
    public class SQLserverDbContext : DbContext
    {
        /// <summary>
        /// SmoothHR連線
        /// </summary>
        /// <param name="options"></param>
        public SQLserverDbContext(DbContextOptions<SQLserverDbContext> options) :
            base(options)
        {
        }
    }
    /// <summary>
    /// public class InnoluxDbContext :
    /// </summary>
    public class InnoluxDbContext : DbContext
    {
        /// <summary>
        /// OQCDb連線
        /// </summary>
        /// <param name="options"></param>
        public InnoluxDbContext(DbContextOptions<InnoluxDbContext> options) :
            base(options)
        {
        }

    }

    public class LCMDBRDbContext : DbContext
    {
        /// <summary>
        /// OQCDb連線
        /// </summary>
        /// <param name="options"></param>
        public LCMDBRDbContext(DbContextOptions<LCMDBRDbContext> options) :
            base(options)
        {
        }

    }

    public class PHAMRTDbConnection : DbContext
    {
        /// <summary>
        /// OQCDb連線
        /// </summary>
        /// <param name="options"></param>
        public PHAMRTDbConnection(DbContextOptions<PHAMRTDbConnection> options) :
            base(options)
        {
        }

    }




    

    public class MBSDA1DbContext : DbContext
    {
       
        public MBSDA1DbContext(DbContextOptions<MBSDA1DbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    ConfigHelper config = new ConfigHelper();
        //    config.GetDbBuilder(optionsBuilder, "MCSDA1");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly, x => x.BaseType.Name == "MBSDA1DbConfig");
        }

    }

    public class MCSDA1DbContext : DbContext
    {
       
        public MCSDA1DbContext(DbContextOptions<MCSDA1DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly, x => x.BaseType.Name == "MCSDA1DbConfig");

        }
    }


  




}
