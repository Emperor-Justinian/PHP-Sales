using PHP.Sales.Core.Extensions;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using System;
using System.IO;

namespace PHP.Sales.GenerateDatabase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath("..\\..\\..\\PHP.Sales.Web\\App_Data"));
            using (var ctx = new SalesDbContext())
            {
                ctx.Database.Delete();
                ctx.Database.CreateIfNotExists();

                //CREATE YOUR OBJECTS
                var transaction = new Transaction()
                {
                    PayMethod = PaymentType.VISA,
                    SaleTime = DateTime.Now                    
                };

                var sale = new Sale()
                {
                    Name = "Panodole",
                    ProductID = "1000121",
                    QTY = 4.00m,
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
