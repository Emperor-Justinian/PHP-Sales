using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess.Configurations;
using System.Data.Entity;

namespace PHP.Sales.DataAccess
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext() 
            :base("sales.db")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SaleConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
