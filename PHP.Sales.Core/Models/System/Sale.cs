using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Sale : BaseEntity
    {
        /*
         * Transaction Link 
         */
        public Guid TransactionID { get; set; }
        public virtual Transaction Transaction { get; set; }
        
        [Required(ErrorMessage = "A product name is required")]
        [DisplayName("Product ID")]
        public string ProductID { get; set; }

        [Required(ErrorMessage = "A product name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Quantity is required")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [DisplayName("Quanity")]
        public decimal QTY { get; set; }

        [Required(ErrorMessage = "A price is required")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [DisplayName("Price")]
        public decimal Price { get; set; }
        
        [DisplayName("GST")]
        public bool GST { get; set; }
        public bool Void { get; set; }
    }
}
