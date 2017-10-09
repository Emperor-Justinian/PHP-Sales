using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Logic;

namespace PHP.Sales.Web.Controllers
{
    public class ProductController : Controller
    {
        // GET: Products
        /// <summary>
        /// List of Products
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext()) { return View(ctx.Products.ToList()); }
        }

        // GET: Products/Details/5
        /// <summary>
        /// Product details
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product view</returns>
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
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
        /// <summary>
        /// Add a new Product via a form
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Add a new Product to the database
        /// </summary>
        /// <param name="product">Product model</param>
        /// <returns></returns>
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
                    ProductLog.GenerateProductLog(ctx, product, product.QTY);

                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(product);
            }
        }

        // GET: Products/Edit/5
        /// <summary>
        /// Edit the product details
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
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
        /// <summary>
        /// Update the product details
        /// </summary>
        /// <param name="product">Product model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,QTY,Price,Discontinued")] Product product)
        {
            using (var ctx = new SalesDbContext())
            {
                if (ModelState.IsValid)
                {
                    var oldProduct = ctx.Products.Where(x => x.ID == product.ID).FirstOrDefault();

                    oldProduct.Name = product.Name;
                    oldProduct.QTY = product.QTY;
                    oldProduct.LowWarn = product.LowWarn;
                    oldProduct.Price = product.Price;
                    oldProduct.Discontinued = product.Discontinued;

                    ProductLog.GenerateProductLog(ctx, product, -product.QTY);
                    ctx.SaveChanges();                    
                    return RedirectToAction("Index");
                }
                return View(product);
            }
        }

        // GET: Products/Delete/5
        /// <summary>
        /// Remove a product from active use.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
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
        /// <summary>
        /// Discontinue a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
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
