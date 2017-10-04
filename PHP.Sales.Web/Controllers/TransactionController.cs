﻿﻿using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PHP.Sales.Web.ViewModels;
using System.Net;
using PHP.Sales.Logic;

namespace PHP.Sales.Web.Controllers
{
    public class TransactionController : Controller
    {
        public IEnumerable<SelectListItem> GetProducts()
        {
            SalesDbContext ctx = new SalesDbContext();

            var Products = ctx.Products.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Name
            });

            return new SelectList(Products, "Value", "Text");
        }

        public ViewResult AddProduct()
        {
            var model = new ProductListViewModel()
            {
                Products = GetProducts()
            };

            return View("_ProductListSelector", model);
        }

        // GET: Transaction
        /// <summary>
        /// Display the Transaction landing page
        /// </summary>
        /// <returns>List of all transactions</returns>
        public ActionResult Index()
        {
            var models = new List<Transaction>();

            using (var ctx = new SalesDbContext())
            {
                models = ctx.Transactions.OrderBy(x => x.SaleTime).ToList();
            }

            return View(models);
        }

        /// <summary>
        /// Create an empty transaction
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a transaction with items that are being negated from another transaction.
        /// </summary>
        /// <param name="sales">List of Sales to be negated</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(TransactionRowViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    var transact = new Transaction()
                    {
                        PayMethod = viewModel.Payment,
                        Sales = viewModel.SalesList
                    };

                    foreach(Sale s in viewModel.SalesList)
                    {
                        /*Decimal OldQTY = ctx.Products.Where(x => x.ID == s.ProductID).FirstOrDefault().QTY;
                        Log l = new Log()
                        {
                            ProductID = s.ProductID,
                            QTY = OldQTY - s.QTY
                        };
                        l.Update();
                        ctx.Logs.Add(l);*/

                        ProductLog.GenerateLog(ctx, s.ProductID, -s.QTY);
                        s.Update();
                    }

                    transact.Update();

                    ctx.Transactions.Add(transact);
                    ctx.SaveChanges();

                    return RedirectToAction("Read", new { id = transact.ID });
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// Create a transaction with items that are being negated from another transaction.
        /// </summary>
        /// <param name="sales">List of Sales to be negated</param>
        /// <returns></returns>
        public ActionResult Return(TransactionRowViewModel t)
        {
            using(var ctx = new SalesDbContext())
            {
                for(int i = 0; i < t.SalesList.Count; i++)
                {
                    if (t.SalesList.ElementAt(i).Void == false)
                    {
                        t.SalesList.RemoveAt(i--);
                    } else
                    {
                        Guid g = t.SalesList.ElementAt(i).ID;
                        Sale s = ctx.Sales.Where(m => m.ID == g).FirstOrDefault();
                        t.SalesList[i] = s;
                        t.SalesList.ElementAt(i).QTY *= -1;
                        t.SalesList.ElementAt(i).Void = true;
                    }
                }
            }

            return View(t);
        }

        /// <summary>
        /// Edit Transaction Contents
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>View with Sales data from the Transaction</returns>
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (var ctx = new SalesDbContext())
            {
                var txn = ctx.Transactions.Include("Sales").Where(t => t.ID == id).FirstOrDefault();

                if(txn != null)
                {
                    var viewModel = new TransactionRowViewModel()
                    {
                        TransactionId = txn.ID,
                        SalesList = txn.Sales.ToList()
                    };

                    return View(viewModel);
                }
            }

            return View(new TransactionRowViewModel()
            {
                TransactionId = Guid.Empty,
                SalesList = new List<Sale>()
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Read(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (var ctx = new SalesDbContext())
            {
                var txn = ctx.Transactions.Include("Sales").Where(t => t.ID == id).FirstOrDefault();

                if (txn != null)
                {
                    var viewModel = new TransactionRowViewModel()
                    {
                        TransactionId = txn.ID,
                        SalesList = txn.Sales.ToList()
                    };

                    return View(viewModel);
                }
            }
            
            return View(new TransactionRowViewModel()
            {
                TransactionId = Guid.Empty,
                SalesList = new List<Sale>()
            });
        }

        //VALIDATOR
        /// <summary>
        /// Validate the incoming data for an Edit form
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>Transaction Index</returns>
        [HttpPost]
        public ActionResult Edit(TransactionRowViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                using(var ctx = new SalesDbContext())
                {
                    var oldTransaction = ctx.Transactions.Include("Sales").Where(t => t.ID == viewModel.TransactionId).FirstOrDefault();

                    if(oldTransaction != null)
                    {
                        foreach(var item in viewModel.SalesList)
                        {
                            var oldSale = oldTransaction.Sales.Where(x => x.ID == item.ID).FirstOrDefault();

                            if(oldSale == null)
                            {
                                oldSale = new Sale();
                                oldTransaction.Sales.Add(oldSale); 
                                oldTransaction.Update();
                            }

                            //oldSale.Name = item.Name;
                            oldSale.GST = item.GST;
                            oldSale.Price = item.Price;
                            oldSale.ProductID = item.ProductID;
                            Decimal qtyChanged = oldSale.QTY - item.QTY;
                            oldSale.QTY = item.QTY;
                            oldSale.Product.QTY += qtyChanged;
                            /*Log l = new Log()
                            {
                                ProductID = oldSale.ProductID,
                                QTY = qtyChanged
                            };
                            l.Update();
                            ctx.Logs.Add(l);*/
                            ProductLog.GenerateLog(ctx, item.ProductID, qtyChanged);

                            oldSale.Update();
                        }

                        ctx.SaveChanges();

                        return RedirectToAction("Read", new { id = viewModel.TransactionId });
                    }
                    else
                    {
                        ModelState.AddModelError("Not Found", "ID was not found");
                    }
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// Creates a new row for the Creator Form
        /// </summary>
        /// <param name="sale"></param>
        /// <returns>A row form</returns>
        public ViewResult TransactionEditorRow(List<Sale> sale)
        {
            return View(sale);
        }

        /// <summary>
        /// Creates a new empty row for the Creator Form
        /// </summary>
        /// <returns>An empty row</returns>
        public ViewResult BlankRowEditor()
        {
            return View("TransactionEditorRow", new List<Sale>()
            {
                new Sale()
            });
        }
    }
}