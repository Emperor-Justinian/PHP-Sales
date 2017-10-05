using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Log : BaseEntity
    {
        [Required(ErrorMessage = "A Time Stamp is required")]
        [DisplayName("Time")]
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "A Product Name is required")]
        [DisplayName("Product ID")]
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "An update message is required")]
        [DisplayName("StockChange")]
        public decimal QTY { get; set; }
    }
}
