using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseApiApp.Models.Master
{
    public class Locations
    {
        public string CityId { get; set; }
        public int Name { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; } 
        public bool Status { get; set; }
    }
}