using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class license : baseEFModel
    {
        [ForeignKey("_supplier")]
        public int? supplier { get; set; }
        public virtual supplier _supplier { get; set; }
        public string issuer { get; set; }
        public string no { get; set; }
        public DateTime? issueDate { get; set; }
        public int? license_type { get; set; }
        public virtual license_type _license_type { get; set; }
        public DateTime? expiryDate { get; set; }
        public string status { get; set; }
        public string additionalInfo { get; set; }
    }
}
