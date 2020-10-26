using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class tpb_usiness_activity_date_by_inn_response : baseEFModel
    {
        public string legalAddress { get; set; }
        public string name { get; set; }
        public string rayonCode { get; set; }
        public string rayonName { get; set; }
        public DateTime taxActiveDate { get; set; }
        public string taxTypeCode { get; set; }
        public string taxTypeName { get; set; }
        public string tin { get; set; }
    }
}
