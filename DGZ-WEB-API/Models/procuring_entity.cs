using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class procuring_entity : baseEFModel
    {
        public string inn { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string contactData { get; set; }
    }
}
