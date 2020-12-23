using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class pension_info : baseEFModel
    {
        [ForeignKey("_supplier_member")]
        public int? supplier_member { get; set; }
        public virtual supplier_member _supplier_member { get; set; }
        public string rusf { get; set; }
        public string numDossier { get; set; }
        public string pinPensioner { get; set; }
        public string pinRecipient { get; set; }
        public string dateFromInitial { get; set; }
        public string sum { get; set; }
        public string kindOfPension { get; set; }
        public string categoryPension { get; set; }
    }
}
