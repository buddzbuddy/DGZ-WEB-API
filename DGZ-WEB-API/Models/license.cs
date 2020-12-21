using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class license : baseEFModel
    {
        public string issuer { get; set; }
        public string no { get; set; }
        public DateTime? issueDate { get; set; }
        public string industryFor { get; set; }
        public DateTime? expiryDate { get; set; }
        public string status { get; set; }
        public string additionalInfo { get; set; }
    }
}
