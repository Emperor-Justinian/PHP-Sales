using PHP.Sales.Core.Models.System;
using System.Data.Entity.ModelConfiguration;

namespace PHP.Sales.DataAccess.Configurations
{
    public class SaleConfiguration : EntityTypeConfiguration<Sale>
    {
        public SaleConfiguration()
        {
            Property(x => x.TransactionID)
                .HasColumnName("TID")
                .IsRequired();
            Property(x => x.ProductID)
                .HasColumnName("PID")
                .IsRequired();
            Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            Property(x => x.QTY)
                .HasPrecision(10, 3)
                .IsRequired();
            Property(x => x.Price)
                .HasPrecision(8, 2)
                .IsRequired();
            Property(x => x.Void).IsRequired();
            Property(x => x.GST).IsRequired();
        }
    }
}
