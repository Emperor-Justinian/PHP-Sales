using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;

namespace PHP.Sales.Web.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext()) { return View(ctx.Products.ToList()); }
        }

        // GET: Log/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Log log = ctx.Logs.Find(id);
        //    if (log == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(log);
        //}

        // GET: Log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TimeStamp,ProductID,Message")] Log log)
        {
            using (var ctx = new SalesDbContext())
            {
                if (ModelState.IsValid)
                {
                    log.ID = Guid.NewGuid();
                    ctx.Logs.Add(log);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(log);
        }

        // GET: Log/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Log log = db.Logs.Find(id);
        //    if (log == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(log);
        //}

        // POST: Log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,TimeStamp,ProductID,Message")] Log log)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(log).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(log);
        //}

        // GET: Log/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Log log = db.Logs.Find(id);
        //    if (log == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(log);
        //}

        // POST: Log/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Log log = db.Logs.Find(id);
        //    db.Logs.Remove(log);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
