using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PHP.Sales.Web.ViewModels
{
    public class ProductListViewModel
    {
        /// <summary>
        /// ID of Product
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// List of the selected products
        /// </summary>
        public IEnumerable<SelectListItem> Products { get; set; }

        /// <summary>
        /// Row ID for listed forms
        /// </summary>
        public int Row { get; set; }
    }
}