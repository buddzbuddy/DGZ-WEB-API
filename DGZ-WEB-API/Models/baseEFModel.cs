using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public abstract class baseEFModel
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
