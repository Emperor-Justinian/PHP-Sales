using PHP.Sales.Core.Models.System;
using System.Collections.Generic;

namespace PHP.Sales.Web.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}