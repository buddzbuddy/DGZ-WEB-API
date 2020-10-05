using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class procuring_entity : baseEFModel
    {
        public string INN { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactData { get; set; }
    }
}
