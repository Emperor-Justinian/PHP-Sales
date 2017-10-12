using PHP.Sales.Core.Models.System;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models
{
    public abstract class LogEntity : BaseEntity
    {
        [Required(ErrorMessage = "A Time Stamp is required")]
        [DisplayName("Time")]
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "A Product Name is required")]
        [DisplayName("Product ID")]
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "An update message is required")]
        [DisplayName("Stock Change")]
        public decimal QTY { get; set; }
    }
}
