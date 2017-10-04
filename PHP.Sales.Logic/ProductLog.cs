using System;
using System.Linq;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;

namespace PHP.Sales.Logic
{
    public static class ProductLog
    {
        public static void GenerateLog(SalesDbContext ctx, Guid PID, decimal newQTY)
        {
            Product oldProduct = ctx.Products.Where(x => x.ID == PID).FirstOrDefault();

            decimal oldQTY = (oldProduct != null) ? oldProduct.QTY : 0;

            Log l = new Log()
            {
                ProductID = PID,
                QTY = oldQTY + newQTY
            };
            l.Update();
            ctx.Logs.Add(l);
        }
    }
}
