using System;
using System.Collections.Generic;
using PHP.Sales.Core.Models.System;

namespace PHP.Sales.Web.ViewModels
{
    public class TransactionRowViewModel
    {
        /// <summary>
        ///     Trasnaction ID Editing
        /// </summary>
        public Guid TransactionId { get; set; }
        
        /// <summary>
        /// Payment method
        /// </summary>
        public PaymentType Payment { get; set; }

        /// <summary>
        ///     List of Sales
        /// </summary>
        public List<Sale> SalesList { get; set; }
    }
}