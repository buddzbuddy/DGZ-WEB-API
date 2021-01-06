using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class audit_method_type : baseEFModel
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
