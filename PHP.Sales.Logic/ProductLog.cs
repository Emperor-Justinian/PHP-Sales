using System;
using System.Linq;
using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;

namespace PHP.Sales.Logic
{
    public static class ProductLog
    {
        public static void GenerateProductLog(SalesDbContext ctx, Product product, decimal newQTY)
        {
            decimal oldQTY = (product != null) ? product.QTY : 0;
            decimal qtyChanged = oldQTY + newQTY;

            if (qtyChanged != 0 || newQTY != 0)
            {
                Log l = new Log()
                {
                    ProductID = product.ID,
                    QTY = qtyChanged
                };
                l.Update();
                ctx.Logs.Add(l);
            }
        }

        public static void GenerateSaleLog(SalesDbContext ctx, Product product, decimal qtyChanged)
        {
            if (qtyChanged != 0)
            {
                Log l = new Log()
                {
                    ProductID = product.ID,
                    QTY = -qtyChanged
                };
                l.Update();
                ctx.Logs.Add(l);
            }
        }
    }
}
