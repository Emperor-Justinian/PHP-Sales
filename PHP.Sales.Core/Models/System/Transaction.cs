using System;
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

        public virtual ICollection<Sale> Sales { get; set; }
        public PaymentType PayMethod { get; set; }
        public DateTime SaleTime { get; set; }
    }
}
