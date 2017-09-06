using PHP.Sales.Core.Extensions;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;

namespace PHP.Sales.GenerateDatabase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = new SalesDbContext())
            {
                ctx.Database.Delete();
                ctx.Database.CreateIfNotExists();

                //CREATE YOUR OBJECTS
                var transaction = new Transaction()
                {
                    PayMethod = PaymentType.VISA
                };

                var sale = new Sale()
                {
                    Name = "Panodole",
                    QTY = 4,
                    Price = 12.32m,
                    GST = true,
                    Void = false,
                };

                sale.Update();

                transaction.Sales.Add(sale);

                transaction.Update();

                ctx.Transactions.Add(transaction);
                ctx.SaveChanges();
            }
        }
    }
}
