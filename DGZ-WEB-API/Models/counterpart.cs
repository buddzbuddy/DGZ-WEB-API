using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class counterpart : baseEFModel
    {
        public string name { get; set; }
        public int? counterpart_type { get; set; }
        public string contactData { get; set; }
        public string address { get; set; }
        public string bankAccountNo { get; set; }
        public string comments { get; set; }
    }
}
