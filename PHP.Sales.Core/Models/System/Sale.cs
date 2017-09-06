using System;

namespace PHP.Sales.Core.Models.System
{
    public class Sale : BaseEntity
    {
        /*
         * Transaction Link 
         */
        public Guid TransactionID { get; set; }
        public virtual Transaction Transaction { get; set; }

        public string ProductID { get; set; }
        public string Name { get; set; }
        public decimal QTY { get; set; }
        public decimal Price { get; set; }
        public bool GST { get; set; }
        public bool Void { get; set; }
    }
}
