using System;

namespace PHP.Sales.Core.Models.System
{
    public class Report : BaseEntity
    {
        public string Name { get; set; }

        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
