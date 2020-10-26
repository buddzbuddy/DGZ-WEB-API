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
        public string ContactData { get; set; }
        public string Address { get; set; }
        public string BankAccountNo { get; set; }
        public string Comments { get; set; }
    }
}
