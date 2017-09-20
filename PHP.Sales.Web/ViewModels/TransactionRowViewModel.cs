using PHP.Sales.Core.Models.System;
using System;
using System.Collections.Generic;

namespace PHP.Sales.Web.ViewModels
{
    public class TransactionRowViewModel
    {
        /// <summary>
        ///     Trasnaction ID Editing
        /// </summary>
        public Guid TransactionId { get; set; }
        public PaymentType Payment { get; set; }

        /// <summary>
        ///     List of Sales
        /// </summary>
        public List<Sale> SalesList { get; set; }
    }
}