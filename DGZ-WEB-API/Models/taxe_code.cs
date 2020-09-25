using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class taxe_code:baseEFModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DetailName { get; set; }
    }
}
