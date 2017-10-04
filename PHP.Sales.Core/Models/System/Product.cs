using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Product : BaseEntity
    {

        [Required(ErrorMessage = "A product name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A quantity is required")]
        [DisplayName("QTY")]
        public decimal QTY { get; set; }

        [Required(ErrorMessage = "A quantity is required")]
        [DisplayName("Low Stock Threshold")]
        public decimal LowWarn { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "A unit price is required")]
        [DisplayName("Unit Price")]
        public decimal Price { get; set; }

        [DisplayName("Discontinued")]
        public bool Discontinued { get; set; }
    }
}
