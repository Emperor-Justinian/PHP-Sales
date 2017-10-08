using PHP.Sales.Core.Extensions;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Logic;
using System;
using System.Collections.Generic;
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

                //CREATE THE PRODUCTS
                List<Product> prods = new List<Product>(new Product[] {
                    new Product()
                    {
                        Name = "Panodol",
                        Price = 12.32m,
                        QTY = 30
                    },
                    new Product()
                    {
                        Name = "Nurofen",
                        Price = 6.32m,
                        QTY = 40
                    },
                    new Product()
                    {
                        Name = "Anti-Fungal",
                        Price = 7.29m,
                        QTY = 10
                    },
                    new Product()
                    {
                        Name = "Antibiotic",
                        Price = 8.99m,
                        QTY = 5
                    },
                    new Product()
                    {
                        Name = "Antihistermine",
                        Price = 18.02m,
                        QTY = 10
                    },
                    new Product()
                    {
                        Name = "Sunscreen",
                        Price = 3372.5m,
                        QTY = 1000
                    },
                    new Product()
                    {
                        Name = "Multivitamin 90",
                        Price = 112400.00m,
                        QTY = 7500
                    },
                    new Product()
                    {
                        Name = "Vitamin D",
                        QTY = 1000.00m,
                        Price = 8990.00m,
                    },
                    new Product()
                    {
                        Name = "HS Pain Killer",
                        QTY = 10m,
                        Price = 8.99m,
                    },
                    new Product()
                    {
                        Name = "Baby Paracetemol",
                        QTY = 5.00m,
                        Price = 12.59m,
                    },
                    new Product()
                    {
                        Name = "Cold and Flu 24",
                        QTY = 4.00m,
                        Price = 7.29m,
                    },
                });

                foreach(Product p in prods)
                {
                    p.Update();
                    ProductLog.GenerateProductLog(ctx, p, p.QTY);
                }
                ctx.Products.AddRange(prods);

                //CREATE THE TRANSACTIONS
                //First Transaction and Sale 
                var tran1 = new Transaction()
                {
                    PayMethod = PaymentType.VISA,
                    SaleTime = DateTime.Now                    
                };
                
                var sale11 = new Sale()
                {
                    Product = prods[0],
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
                    
                var sale21 = new Sale()
                {
                    Product = prods[1],
                    QTY = 2.00m,
                    Price = 6.32m,
                    GST = true,
                    Void = false,
                };
                
                var sale22 = new Sale()
                {
                    Product = prods[0],
                    QTY = 3.00m,
                    Price = 9.24m,
                    GST = true,
                    Void = false,
                };
                
                //Third Transaction and Sale 
                var tran3 = new Transaction()
                {
                    PayMethod = PaymentType.MASTERCARD,
                    SaleTime = DateTime.Now,
                };
                
                var sale31 = new Sale()
                {
                    Product = prods[2],
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,    
                };
                
                var sale32 = new Sale()
                {
                    Product = prods[3],
                    QTY = 1.00m,
                    Price = 8.99m,
                    GST = false,
                    Void = false,
                };
                
                var sale33 = new Sale()
                {
                    Product = prods[4],
                    QTY = 2.00m,
                    Price = 18.02m,
                    GST = true,
                    Void = false,
                };
                
                //Fourth Transaction and Sale (Large Quantities)
                var tran4 = new Transaction()
                {
                    PayMethod = PaymentType.CASH,
                    SaleTime = DateTime.Now,
                };
                
                var sale41 = new Sale()
                {
                    Product = prods[5],
                    QTY = 250.00m,
                    Price = 3372.5m,
                    GST = true,
                    Void = false,    
                };
                
                var sale42 = new Sale()
                {
                    Product = prods[6],
                    QTY = 5000.00m,
                    Price = 112400.00m,
                    GST = true,
                    Void = false,
                };
                
                var sale43 = new Sale()
                {
                    Product = prods[7],
                    QTY = 1000.00m,
                    Price = 8990.00m,
                    GST = true,
                    Void = false,
                };
                
                //Fifth Transaction and Sale (One of Everything)
                var tran5 = new Transaction()
                {
                    PayMethod = PaymentType.CASH,
                    SaleTime = DateTime.Now,
                };
                
                var sale51 = new Sale()
                {
                    Product = prods[0],
                    QTY = 1.00m,
                    Price = 3.08m,
                    GST = true,
                    Void = false, 
                };
                
                var sale52 = new Sale()
                {
                    Product = prods[1],
                    QTY = 1.00m,
                    Price = 3.16m,
                    GST = true,
                    Void = false,
                };
                
                var sale53 = new Sale()
                {
                    Product = prods[2],
                    QTY = 1.00m,
                    Price = 9.02m,
                    GST = true,
                    Void = false,
                };
                
                var sale54 = new Sale()
                {
                    Product = prods[5],
                    QTY = 1.00m,
                    Price = 13.49m,
                    GST = true,
                    Void = false,    
                };
                
                var sale55 = new Sale()
                {
                    Product = prods[6],
                    QTY = 1.00m,
                    Price = 22.48m,
                    GST = true,
                    Void = false,
                };
                
                var sale56 = new Sale()
                {
                    Product = prods[7],
                    QTY = 10.00m,
                    Price = 8.99m,
                    GST = true,
                    Void = false,
                };
                
                var sale57 = new Sale()
                {
                    Product = prods[4],
                    QTY = 1.00m,
                    Price = 8.99m,
                    GST = false,
                    Void = false,    
                };
                
                var sale58 = new Sale()
                {
                    Product = prods[8],
                    QTY = 1.00m,
                    Price = 8.99m,
                    GST = false,
                    Void = false,
                };
                
                var sale59 = new Sale()
                {
                    Product = prods[9],
                    QTY = 1.00m,
                    Price = 12.59m,
                    GST = true,
                    Void = false,
                };
                
                var sale510 = new Sale()
                {
                    Product = prods[2],
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,   
                };
                
                var sale511 = new Sale()
                {
                    Product = prods[10],
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,   
                };
                
                //Sixth Sale and Trnasaction
                var tran6 = new Transaction()
                {
                    PayMethod = PaymentType.VISA,
                    SaleTime = DateTime.Now,
                };

                var sale61 = new Sale()
                {
                    Product = prods[10],
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,
                };
                    
                var sale62 = new Sale()
                {
                    Product = prods[8],
                    QTY = 1.00m,
                    Price = 8.99m,
                    GST = false,
                    Void = false,
                };
                
                var sale63 = new Sale()
                {
                    Product = prods[6],
                    QTY = 1.00m,
                    Price = 22.48m,
                    GST = true,
                    Void = false,
                };
                
                //Seventh Sale and Transaction (Same product sold multiple times)
                var tran7 = new Transaction()
                {
                    PayMethod = PaymentType.MASTERCARD,
                    SaleTime = DateTime.Now,
                };
                
                var sale71 = new Sale()
                {
                    Product = prods[6],
                    QTY = 10.00m,
                    Price = 224.80m,
                    GST = true,
                    Void = false,
                };
                
                var sale72 = new Sale()
                {
                    Product = prods[6],
                    QTY = 1.00m,
                    Price = 22.48m,
                    GST = true,
                    Void = false,
                };
                
                var sale73 = new Sale()
                {
                    Product = prods[6],
                    QTY = 1.00m,
                    Price = 22.48m,
                    GST = true,
                    Void = false,
                };
                
                var sale74 = new Sale()
                {
                    Product = prods[10],
                    QTY = 1.00m,
                    Price = 7.29m,
                    GST = true,
                    Void = false,   
                };
                
                var sale75 = new Sale()
                {
                    Product = prods[6],
                    QTY = 1.00m,
                    Price = 22.48m,
                    GST = true,
                    Void = false,
                };
                
                //Update Sale 7
                sale71.Update();
                sale72.Update();
                sale73.Update();
                sale74.Update();
                sale75.Update();
                
                tran7.Sales.Add(sale71);
                tran7.Sales.Add(sale72);
                tran7.Sales.Add(sale73);
                tran7.Sales.Add(sale74);
                tran7.Sales.Add(sale75);

                foreach(Sale s in tran7.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }
                
                tran7.Update();

                ctx.Transactions.Add(tran7);

                //Update Sale 6 
                sale61.Update();
                sale62.Update();
                sale63.Update();
                
                tran6.Sales.Add(sale61);
                tran6.Sales.Add(sale62);
                tran6.Sales.Add(sale63);

                foreach (Sale s in tran6.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran6.Update();

                ctx.Transactions.Add(tran6);

                //Update Sale 5
                sale51.Update();
                sale52.Update();
                sale53.Update();
                sale54.Update();
                sale55.Update();
                sale56.Update();
                sale57.Update();
                sale58.Update();
                sale59.Update();
                sale510.Update();
                sale511.Update();
                
                tran5.Sales.Add(sale51);
                tran5.Sales.Add(sale52);
                tran5.Sales.Add(sale53);
                tran5.Sales.Add(sale54);
                tran5.Sales.Add(sale55);
                tran5.Sales.Add(sale56);
                tran5.Sales.Add(sale57);
                tran5.Sales.Add(sale58);
                tran5.Sales.Add(sale59);
                tran5.Sales.Add(sale510);
                tran5.Sales.Add(sale511);

                foreach (Sale s in tran5.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran5.Update();

                ctx.Transactions.Add(tran5);
                
                //Update Sale 4 
                sale41.Update();
                sale42.Update();
                sale43.Update();
                
                tran4.Sales.Add(sale41);
                tran4.Sales.Add(sale42);
                tran4.Sales.Add(sale43);

                foreach (Sale s in tran4.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran4.Update();

                ctx.Transactions.Add(tran4);
                
                //Update Sale 3
                sale31.Update();
                sale32.Update();
                sale33.Update();

                tran3.Sales.Add(sale31);
                tran3.Sales.Add(sale32);
                tran3.Sales.Add(sale33);

                foreach (Sale s in tran3.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran3.Update();

                ctx.Transactions.Add(tran3);
                
                //Update Sale 2
                sale21.Update();
                sale22.Update();
                
                tran2.Sales.Add(sale21);
                tran2.Sales.Add(sale22);

                foreach (Sale s in tran2.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran2.Update();
                
                ctx.Transactions.Add(tran2);
                
                //Update Sale 1
                sale11.Update();

                tran1.Sales.Add(sale11);

                foreach (Sale s in tran1.Sales)
                {
                    ProductLog.GenerateSaleLog(ctx, s.Product, s.QTY);
                }

                tran1.Update();

                ctx.Transactions.Add(tran1);

                //ADD REPORT
                Report report1 = new Report()
                {
                    Name = "Test Report",
                    Product = prods[0],
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(1)
                };

                report1.Update();

                ctx.Reports.Add(report1);
                
                // Save Transaction Database
                ctx.SaveChanges();
            }
        }
    }
}
