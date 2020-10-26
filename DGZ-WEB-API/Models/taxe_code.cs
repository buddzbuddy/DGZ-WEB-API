using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class taxe_code:baseEFModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public string detailName { get; set; }
    }
}
