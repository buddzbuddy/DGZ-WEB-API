using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class tpb_usiness_activity_date_by_inn_response : baseEFModel
    {
        public string FullAddress { get; set; }
        public string FullName { get; set; }
        public string RayonCode { get; set; }
        public string RayonName { get; set; }
        public DateTime TaxActiveDate { get; set; }
        public string TaxTypeCode { get; set; }
        public string TaxTypeName { get; set; }
        public string TIN { get; set; }
    }
}
