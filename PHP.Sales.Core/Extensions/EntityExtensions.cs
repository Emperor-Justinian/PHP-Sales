using PHP.Sales.Core.Models;
using System;

namespace PHP.Sales.Core.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsNew(this BaseEntity entity)
        {
            return entity.ID == Guid.Empty;
        }

        public static void Update(this BaseEntity entity)
        {
            if(entity.IsNew())
            {
                entity.ID = Guid.NewGuid();
            }
        }
    }
}
