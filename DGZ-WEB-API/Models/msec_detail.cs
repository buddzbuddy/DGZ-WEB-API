using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class msec_detail : baseEFModel
    {
        public string organizationName { get; set; }
        public DateTime? examinationDate { get; set; }
        public string examinationType { get; set; }
        public string disabilityGroup { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string reExamination { get; set; }
        public int? supplier_member { get; set; }
    }
}
