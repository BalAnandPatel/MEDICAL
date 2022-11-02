using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Doctors
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        
        public string Code { get; set; }
        public string MobileNumber { get; set; }
        
        public Decimal Cost { get; set; }
        public bool IsActive { get; set; }
        public bool Gender { get; set; }
        
        public int YearsofExp { get; set; }
        public string Hospital { get; set; }
        public Guid HospitalID { get; set; }
        public string Award { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
        public string UserName { get; set; }

        public int CityId { get; set; }
        public int CountryId { get; set; }
        public Guid TreatmentId { get; set; }
        public Guid TreatmentDetailID { get; set; }

        public string Degree { get; set; }
        public string Experience { get; set; }
        public string AdditionalInfo { get; set; }

    }
}