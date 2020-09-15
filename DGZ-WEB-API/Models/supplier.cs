﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class supplier
    {
        public int id { get; set; }
        public string name { get; set; }

        public int? ownership_type { get; set; }

        public string inn { get; set; }
        public string legalAddress { get; set; }
        public string factAddress { get; set; }
        public string telephone { get; set; }
        public string bankName { get; set; }
        public string bankAccount { get; set; }
        public string bic { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
