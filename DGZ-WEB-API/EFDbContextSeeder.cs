using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API
{
    public static class EFDbContextSeeder
    {
        public static void Seed(EFDbContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
