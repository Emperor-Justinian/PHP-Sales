using PHP.Sales.Core.Models.System;
using System;

namespace PHP.Sales.Core.Extensions
{
    public static class SaleExtensions
    {
        public static bool IsNew(this Sale entity)
        {
            return entity.ID == Guid.Empty;
        }

        public static void Update(this Sale entity)
        {
            if(entity.IsNew())
            {
                entity.ID = Guid.NewGuid();
            }
            entity.Name = entity.Product.Name;
        }
    }
}
