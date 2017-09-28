using PHP.Sales.Core.Models.System;
using System.Data.Entity.ModelConfiguration;

namespace PHP.Sales.DataAccess.Configurations
{
    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        public ReportConfiguration()
        {
            Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            Property(x => x.ProductID)
                .IsRequired();
            Property(x => x.Start)
                .IsRequired();
            Property(x => x.End)
                .IsRequired();
        }
    }
}
