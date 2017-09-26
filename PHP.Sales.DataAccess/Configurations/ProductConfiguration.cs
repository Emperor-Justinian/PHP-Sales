using PHP.Sales.Core.Models.System;
using System.Data.Entity.ModelConfiguration;

namespace PHP.Sales.DataAccess.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            Property(x => x.QTY)
                .HasPrecision(10, 3)
                .IsRequired();
            Property(x => x.Price)
                .HasPrecision(8, 2)
                .IsRequired();
        }
    }
}
