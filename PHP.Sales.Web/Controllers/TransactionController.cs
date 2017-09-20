﻿﻿using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PHP.Sales.Web.ViewModels;

namespace PHP.Sales.Web.Controllers
{
    public class TransactionController : Controller
    {
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
                models = ctx.Transactions.ToList();
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

                    transact.Update();

                    ctx.Transactions.Add(transact);
                    ctx.SaveChanges();

                    return RedirectToAction("Read", new { id = viewModel.TransactionId });
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// Create a transaction with items that are being negated from another transaction.
        /// </summary>
        /// <param name="sales">List of Sales to be negated</param>
        /// <returns></returns>
        public ActionResult Return(List<Sale> s)
        {
            //TODO: HANDLE LIST OF SALES
            List<Sale> sales = new List<Sale>();
            using (var ctx = new SalesDbContext())
            {
            }
            /*foreach(Sale ss in sales)
            {
                ss.ID = new Guid();
                if(ss.QTY > 0)
                {
                    ss.QTY *= -1;
                }
            }*/

            Transaction transact = new Transaction
            {
                Sales = sales
            };

            return View(transact);
        }

        /// <summary>
        /// Edit Transaction Contents
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>View with Sales data from the Transaction</returns>
        public ActionResult Edit(Guid id)
        {
            /*
            List<Sale> sales = null;

            using(var ctx = new SalesDbContext())
            {
                sales = ctx.Sales.Where(t => t.TransactionID == id).ToList();
            }

            return View(sales);
            */

            using (var ctx = new SalesDbContext())
            {
                var txn = ctx.Transactions.Include("Sales").Where(t => t.ID.Equals(id)).FirstOrDefault();

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

        public ActionResult Read(Guid id)
        {
            /*Transaction t = null;

            using (var ctx = new SalesDbContext())
            {
                t = ctx.Transactions.Where(model => model.ID == id).FirstOrDefault();
                t.Sales = ctx.Sales.Where(model => model.TransactionID == id).ToList();
            }*/

            using (var ctx = new SalesDbContext())
            {
                var txn = ctx.Transactions.Include("Sales").Where(t => t.ID.Equals(id)).FirstOrDefault();

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

                            oldSale.Name = item.Name;
                            oldSale.GST = item.GST;
                            oldSale.Price = item.Price;
                            oldSale.ProductID = item.ProductID;
                            oldSale.QTY = item.QTY;

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