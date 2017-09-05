namespace PHP.Sales.Core.Models.System;
{
    public enum PaymentType
    {
        MASTERCARD,
        VISA,
        CASH
    };

    public class Transaction : BaseEntity
    {
        public string TID { get; set; }
        public string PID { get; set; }
        public string Name { get; set; }
        public decimal QTY { get; set; }
        public decimal Price { get; set; }
        public bool GST { get; set; }
        public bool Void { get; set; }
        public PaymentType PayMethod { get; set; }
    }
}
