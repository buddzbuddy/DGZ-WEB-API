using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGZ_WEB_API.Models
{
    public class supplier_member : baseEFModel
    {
        public int supplier { get; set; }
        public string pin { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string nationality { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string passportSeries { get; set; }
        public string passportNumber { get; set; }
        public string passportAuthority { get; set; }
        public DateTime issuedDate { get; set; }
        public DateTime expiredDate { get; set; }
        public int voidStatus { get; set; }
        public int familyStatus { get; set; }
        public string gender { get; set; }
        public string addressRegion { get; set; }
        public string addressLocality { get; set; }
        public string addressStreet { get; set; }
        public string addressHouse { get; set; }
        public int? regionId { get; set; }
        public int? districtId { get; set; }
        public int? areaId { get; set; }
        public int? subareaId { get; set; }
        public int? streetId { get; set; }
        public int? houseId { get; set; }
        public int? memberTypeId { get; set; }

    }
}
