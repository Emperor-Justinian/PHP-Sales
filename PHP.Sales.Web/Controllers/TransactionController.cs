using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PHP.Sales.Web.Controllers
{
    public class TransactionController : Controller
    {
        // GET: Transaction
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
        public ActionResult Create(List<Sale> sales)
        {
            foreach(Sale s in sales)
            {
                if(s.QTY < 0)
                {
                    s.QTY *= -1;
                }
            }

            return View(sales);
        }

        /// <summary>
        /// Edit Transaction Contents
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>View with Sales data from the Transaction</returns>
        public ActionResult Edit(Guid id)
        {
            List<Sale> sales = null;

            using(var ctx = new SalesDbContext())
            {
                sales = ctx.Sales.Where(t => t.TransactionID == id).ToList();
            }

            return View(sales);
        }

        public ActionResult Read(Guid id)
        {
            Transaction t = null;

            using (var ctx = new SalesDbContext())
            {
                t = ctx.Transactions.Where(model => model.ID == id).FirstOrDefault();
            }

            return View(t);
        }

        //VALIDATOR
        [HttpPost]
        public ActionResult Edit(Transaction viewModel)
        {
            if(ModelState.IsValid)
            {
                //CHECK FOR INVALID RESULTS

                //AIM TO SAVE DATA
                using(var ctx = new SalesDbContext())
                {
                    var oldTransaction = ctx.Transactions.Where(t => t.ID == viewModel.ID).FirstOrDefault();

                    if(oldTransaction != null)
                    {
                        oldTransaction.Sales = viewModel.Sales;
                        ctx.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Not Found", "ID was not found");
                    }
                }
            }

            return View(viewModel);
        }
    }
}