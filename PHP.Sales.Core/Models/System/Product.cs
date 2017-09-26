using System;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal QTY { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public bool Discontinued { get; set; }
    }
}
