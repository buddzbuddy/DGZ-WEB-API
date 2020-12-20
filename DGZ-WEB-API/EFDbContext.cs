using DGZ_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
        public DbSet<taxe_code> taxe_codes { get; set; }
        public DbSet<tp_data_by_inn_for_business_activity_response> tp_data_by_inn_for_business_activity_responses { get; set; }
        public DbSet<tpb_usiness_activity_date_by_inn_response> tpb_usiness_activity_date_by_inn_responses { get; set; }
        public DbSet<counterpart_type> counterpart_types { get; set; }
        public DbSet<counterpart> counterparts { get; set; }
        public DbSet<country> countries { get; set; }
        public DbSet<currency> currencies { get; set; }
        public DbSet<procuring_entity> procuring_entities { get; set; }
        public DbSet<registrator> registrators { get; set; }
        public DbSet<supplier_member> supplier_members { get; set; }
        public DbSet<msec_detail> msec_details { get; set; }
        public DbSet<member_type> member_types { get; set; }
        public DbSet<appeal> appeals { get; set; }
    }
}
