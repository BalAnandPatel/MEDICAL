using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Locations
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public int Status { get; set; }

        public string Code { get; set; }

    }

    public class Country
    { 
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public int Status { get; set; }
    }
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; } 
        public int Status { get; set; }
    }
}