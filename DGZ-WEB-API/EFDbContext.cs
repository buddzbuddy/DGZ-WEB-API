using DGZ_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options)
            : base(options)
        {

        }
        public DbSet<ownership_type> ownership_types { get; set; }
        public DbSet<supplier> suppliers { get; set; }
    }
}
