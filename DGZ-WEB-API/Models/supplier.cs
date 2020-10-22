using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class supplier : baseEFModel
    {
        public string name { get; set; }

        public int? ownership_type { get; set; }

        public string inn { get; set; }
        public string legalAddress { get; set; }
        public string factAddress { get; set; }
        public string telephone { get; set; }
        public string bankName { get; set; }
        public string bankAccount { get; set; }
        public string bic { get; set; }
        public string zip { get; set; }
        public string rayonCode { get; set; }
        public bool isResident { get; set; }
    }
}
