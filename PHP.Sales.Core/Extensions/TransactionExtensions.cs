using PHP.Sales.Core.Models.System;
using System;

namespace PHP.Sales.Core.Extensions
{
    public static class TransactionExtensions
    {
        public static bool IsNew(this Transaction entity)
        {
            return entity.ID == Guid.Empty;
        }

        public static void Update(this Transaction entity)
        {
            if(entity.IsNew())
            {
                entity.ID = Guid.NewGuid();
                entity.SaleTime = DateTime.Now;
            }
        }
    }
}
