using PHP.Sales.Core.Models.System;
using PHP.Sales.DataAccess;
using PHP.Sales.Core.Extensions;

namespace PHP.Sales.Logic
{
    public static class ProductLog
    {
        /// <summary>
        /// Log the change in Quantity from reciving new product stock
        /// </summary>
        /// <param name="ctx">Database Context</param>
        /// <param name="product">Product</param>
        /// <param name="newQTY">Amount added</param>
        public static void GenerateProductLog(SalesDbContext ctx, Product product, decimal newQTY)
        {
            decimal oldQTY = (product != null) ? product.QTY : 0;

            if (newQTY != 0)
            {
                Log l = new Log()
                {
                    ProductID = product.ID,
                    QTY = newQTY
                };
                l.Update();
                ctx.Logs.Add(l);
            }
        }

        /// <summary>
        /// Log the change in Quantity from the sale of stock
        /// </summary>
        /// <param name="ctx">Database Context</param>
        /// <param name="product">Product</param>
        /// <param name="newQTY">Amount sold</param>
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
