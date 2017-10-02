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
    }
}
