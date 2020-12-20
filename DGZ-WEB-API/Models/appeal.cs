using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class appeal : baseEFModel
    {
        public string description { get; set; }
        [ForeignKey("_supplier")]
        public int? supplier { get; set; }
        public virtual supplier _supplier { get; set; }

        [ForeignKey("_procuring_entity")]
        public int? procuring_entity { get; set; }
        public virtual procuring_entity _procuring_entity { get; set; }
    }
}
