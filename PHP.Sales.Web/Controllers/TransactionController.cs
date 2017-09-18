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
        public ActionResult Create(Sale s)
        {
            //TODO: HANDLE LIST OF SALES
            List<Sale> sales = new List<Sale>();
            using(var ctx = new SalesDbContext())
            {
                sales.Add(ctx.Sales.Where(t => t.ID == s.ID).FirstOrDefault());
            }
            foreach(Sale ss in sales)
            {
                if(ss.QTY > 0)
                {
                    ss.QTY *= -1;
                }
            }

            Transaction transact = new Transaction();
            transact.Sales = sales;

            return View(transact);
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
            /*Transaction t = null;

            using (var ctx = new SalesDbContext())
            {
                t = ctx.Transactions.Where(model => model.ID == id).FirstOrDefault();
                t.Sales = ctx.Sales.Where(model => model.TransactionID == id).ToList();
            }*/

            List<Sale> t = null;

            using (var ctx = new SalesDbContext())
            {
                t = ctx.Sales.Where(model => model.TransactionID == id).ToList();
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

        public ViewResult TransactionEditorRow(Sale sale)
        {
            return View(sale);
        }

        public ViewResult BlankRowEditor()
        {
            return View("TransactionEditorRow", new Sale());
        }
    }
}