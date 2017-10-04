using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PHP.Sales.Core.Models.System;
using PHP.Sales.Core.Extensions;
using PHP.Sales.DataAccess;

namespace PHP.Sales.Web.Controllers
{
    public class ProductController : Controller
    {
        //private SalesctxContext ctx = new SalesctxContext();

        // GET: Products
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext()) { return View(ctx.Products.ToList()); }
        }

        // GET: Products/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Product product = ctx.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,QTY,Price,Discontinued")] Product product)
        {
            using (var ctx = new SalesDbContext())
            {
                if (ModelState.IsValid)
                {
                    product.ID = Guid.NewGuid();
                    ctx.Products.Add(product);
                    Log l = new Log()
                    {
                        ProductID = product.ID,
                        QTY = product.QTY
                    };
                    l.Update();
                    ctx.Logs.Add(l);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Product product = ctx.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,QTY,Price,Discontinued")] Product product)
        {
            using (var ctx = new SalesDbContext())
            {
                if (ModelState.IsValid)
                {
                    ctx.Entry(product).State = EntityState.Modified;
                    Decimal OldQTY = ctx.Products.Where(x => x.ID == product.ID).FirstOrDefault().QTY;
                    Log l = new Log()
                    {
                        ProductID = product.ID,
                        QTY = OldQTY - product.QTY
                    };
                    l.Update();
                    ctx.Logs.Add(l);
                    ctx.SaveChanges();                    
                    return RedirectToAction("Index");
                }
                return View(product);
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var ctx = new SalesDbContext())
            {
                Product product = ctx.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (var ctx = new SalesDbContext())
            {
                Product product = ctx.Products.FirstOrDefault(); //ctx.Products.Find(id)
                //ctx.Products.Remove(product);
                product.Discontinued = true;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //location of temporary change tracker storage to be bundled and sent to Logs Controller

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        ctx.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
