using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;
using PHP.Sales.Web.ViewModels;

namespace PHP.Sales.Web.Controllers
{
    public class ReportController : Controller
    {
        public IEnumerable<SelectListItem> GetProducts(Guid? selected)
        {
            SalesDbContext ctx = new SalesDbContext();

            var Products = ctx.Products.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Name
            });

            return new SelectList(Products, "Value", "Text", selected);
        }

        public ViewResult AddProduct(Guid? selected)
        {
            var model = new ProductListViewModel()
            {
                Products = GetProducts(selected)
            };

            if (selected != null)
            {
                model.ProductId = (Guid)selected;
            }

            return View("_ReportProductListSelector", model);
        }
        // GET: Reports
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext())
            {
                return View(ctx.Reports.ToList());
            }
        }

        // GET: Reports/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (var ctx = new SalesDbContext())
            {
                ChartViewModel chart = new ChartViewModel()
                {
                    Report = ctx.Reports.Find(id)
                };
                if (chart.Report == null)
                {
                    return HttpNotFound();
                }
                chart.Report.Product = ctx.Products.Where(x => x.ID == chart.Report.ProductID).FirstOrDefault();

                //GET DATA
                var data1 = ctx.Logs
                                .Where(x => x.ProductID == chart.Report.ProductID)
                                .Where(x => x.TimeStamp >= chart.Report.Start.Date)
                                .Where(x => x.TimeStamp <= chart.Report.End.Date)
                                .ToList();
                var data2 = ctx.Sales
                                .Where(x => x.ProductID == chart.Report.ProductID)
                                .Where(x => x.Transaction.SaleTime >= chart.Report.Start.Date)
                                .Where(x => x.Transaction.SaleTime <= chart.Report.End.Date)
                                .ToList();

                return View(chart);
            }
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    report.Update();
                    ctx.Reports.Add(report);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                if (report == null)
                {
                    return HttpNotFound();
                }
                return View(report);
            }
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new SalesDbContext())
                {
                    ctx.Entry(report).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                if (report == null)
                {
                    return HttpNotFound();
                }
                return View(report);
            }
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (var ctx = new SalesDbContext())
            {
                Report report = ctx.Reports.Find(id);
                ctx.Reports.Remove(report);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (var ctx = new SalesDbContext())
            {
                if (disposing)
                {
                    ctx.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
