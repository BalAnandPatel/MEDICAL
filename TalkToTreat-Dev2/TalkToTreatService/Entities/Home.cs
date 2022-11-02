using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Home
    { 
        public List<DoctorModel> Doctors { get; set; }
        public List<TreatmentModel> Treatment { get; set; }

        public List<HospitalModel> Hospital { get; set; }
        public List<ServiceModel> services { get; set; } 

    }
    public class ServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
    public class DoctorModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }
    }
    public class HospitalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
    }
    public class TreatmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Decimal Cost { get; set; }
    }
}