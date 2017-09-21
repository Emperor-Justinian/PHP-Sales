using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace PHP.Sales.Core.Models.System
{
    public enum PaymentType
    {
        MASTERCARD,
        VISA,
        CASH
    };

    public class Transaction : BaseEntity
    {
        /*
         * Sale Link
         */
        public Transaction()
        {
            Sales = new HashSet<Sale>();
        }

        [DisplayName("Items")]
        public virtual ICollection<Sale> Sales { get; set; }

        [DisplayName("Payment Method")]
        public PaymentType PayMethod { get; set; }

        [DisplayName("Purchase Time")]
        public DateTime? SaleTime { get; set; }
    }
}
