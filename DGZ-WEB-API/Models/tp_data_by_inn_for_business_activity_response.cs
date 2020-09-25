using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class tp_data_by_inn_for_business_activity_response : baseEFModel
    {
        public string inn { get; set; }
        public string RayonCode { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }

    }
}
