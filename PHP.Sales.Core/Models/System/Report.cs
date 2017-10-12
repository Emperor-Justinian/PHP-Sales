using System;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Report : BaseEntity
    {
        public string Name { get; set; }

        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime End { get; set; }
    }
}
