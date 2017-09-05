using PHP.Sales.Core.Models.System;
using System.Data.Entity;

namespace PHP.Sales.DataAccess
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext() 
            :base("sales.db")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
