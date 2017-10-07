using System;
using System.Linq;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;

namespace PHP.Sales.Logic
{
    public static class ProductLog
    {
        public static void GenerateProductLog(SalesDbContext ctx, Guid PID, decimal newQTY)
        {
            Product oldProduct = ctx.Products.Where(x => x.ID == PID).FirstOrDefault();

            decimal oldQTY = (oldProduct != null) ? oldProduct.QTY : 0;
            decimal qtyChanged = oldQTY + newQTY;

            if (qtyChanged != 0 || newQTY != 0)
            {
                Log l = new Log()
                {
                    ProductID = PID,
                    QTY = qtyChanged
                };
                l.Update();
                ctx.Logs.Add(l);
            }
        }

        public static void GenerateSaleLog(SalesDbContext ctx, Guid PID, decimal qtyChanged)
        {
            Product oldProduct = ctx.Products.Where(x => x.ID == PID).FirstOrDefault();

            if (qtyChanged != 0)
            {
                Log l = new Log()
                {
                    ProductID = PID,
                    QTY = -qtyChanged
                };
                l.Update();
                ctx.Logs.Add(l);
            }
        }
    }
}
