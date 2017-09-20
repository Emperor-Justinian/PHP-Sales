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
                //First Transaction and Sale 
                var transaction = new Transaction()
                {
                    PayMethod = PaymentType.VISA,
                    SaleTime = DateTime.Now                    
                };
                
                var sale = new Sale()
                {
                    Name = "Panodol",
                    ProductID = "1000121",
                    QTY = 4.00m,
                    Price = 12.32m,
                    GST = true,
                    Void = false,
                };
                
                //Second Transaction and Sale 
                var tran2 = new Transaction()
                {
                    PayMethod = PaymentType.CASH,
                    SaleTime = DateTime.Now,

                };
                    
                var sale2 = new Sale()
                {
                    Name = "Nurofen",
                    ProductID = "1000221",
                    QTY = 2.00m,
                    Price = 6.32m,
                    GST = true,
                    Void = false,
                };
                
                var sale3 = new Sale()
                {
                    Name = "Panodol",
                    ProductID = "1000121",
                    QTY = 3.00m,
                    Price = 6.32m,
                    GST = true,
                    Void = false,
                };
                
                //Third Transaction and Sale 
                var tran3 = new Transaction()
                {
                    PayMethod = PaymentType.MASTERCARD,
                    SaleTime = DateTime.Now,
                };
                
                var sale4 = new Sale()
                {
                    Name = "Anti-Fungal",
                    ProductID = "1000135",
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,    
                };
                
                var sale5 = new Sale()
                {
                    Name = "Antibiotic",
                    ProductID = "1000621",
                    QTY = 1.00m,
                    Price = 8.99m,
                    GST = false,
                    Void = false,
                };
                
                var sale6 = new Sale()
                {
                    Name = "Antihistermine",
                    ProductID = "1000171",
                    QTY = 2.00m,
                    Price = 18.01m,
                    GST = true,
                    Void = false,
                };
                
                //Update Sale 3
                sale4.Update();
                sale5.Update();
                sale6.Update();

                tran3.Sales.Add(sale4);
                tran3.Sales.Add(sale5);
                tran3.Sales.Add(sale6);
                
                tran3.Update();

                ctx.Transactions.Add(tran3);
                
                //Update Sale 2
                sale2.Update();
                sale3.Update();
                
                tran2.Sales.Add(sale2);
                tran2.Sales.Add(sale3);
                
                tran2.Update();
                
                ctx.Transactions.Add(tran2);
                
                //Update Sale 1
                sale.Update();

                transaction.Sales.Add(sale);

                transaction.Update();

                ctx.Transactions.Add(transaction);
                ctx.SaveChanges();
            }
        }
    }
}
