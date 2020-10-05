using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class registrator : baseEFModel
    {
        public string Name { get; set; }
        public int? counterpart { get; set; }
        public string ContactData { get; set; }
    }
}
