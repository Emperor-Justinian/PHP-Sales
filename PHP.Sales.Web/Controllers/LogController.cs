using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PHP.Sales.DataAccess;

namespace PHP.Sales.Web.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            using (var ctx = new SalesDbContext()) {
                return View(ctx.Logs.Include("Product").OrderBy(x => x.TimeStamp).ToList());
            }
        }
    }
}
