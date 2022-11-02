using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreat.Models
{
    public class CountryWiseCities
    {
        public string Country { get; set; }
        public List<CityWiseHospitals> Cities { get; set; }
    }

    public class CityWiseHospitals
    {
        public string City { get; set; }
        public List<Hospital> Hospitals { get; set; }
    }

    public class CountryWiseTreatmentPackages
    {
        public string Country { get; set; }
        public List<CityWiseTreatmentPackages> Cities { get; set; }
    }

    public class CityWiseTreatmentPackages
    {
        public string City { get; set; }
        public Dictionary<string, List<TreatmentPackage>> TreatmentPackages { get; set; }
    }

    public class TreatmentPackage
    {
        public System.Guid Id { get; set; }
        public Guid HospitalId { get; set; }
        public Guid TreatmentId { get; set; }
        public decimal MinCost { get; set; }
        public decimal MaxCost { get; set; }
        public bool IsActive { get; set; }
        public string HospitalName { get; set; }
        public string HospitalImage { get; set; }
        public string HospitalIconImage { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string TreatmentName { get; set; }
        public string TreatmentImage { get; set; }
    }
}