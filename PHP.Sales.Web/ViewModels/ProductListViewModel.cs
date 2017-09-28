using PHP.Sales.Core.Models.System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PHP.Sales.Web.ViewModels
{
    public class ProductListViewModel
    {
        public List<int> ProductIds { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}