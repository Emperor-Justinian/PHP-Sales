using PHP.Sales.Core.Models.System;
using System;

namespace PHP.Sales.Core.Extensions
{
    public static class LogExtensions
    {
        public static bool IsNew(this Log entity)
        {
            return entity.ID == Guid.Empty;
        }

        public static void Update(this Log entity)
        {
            if(entity.IsNew())
            {
                entity.ID = Guid.NewGuid();
                entity.TimeStamp = DateTime.Now;
            }
        }
    }
}
