using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PHP.Sales.Web.ViewModels
{
    public class ProductListViewModel
    {
        public Guid ProductId { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}