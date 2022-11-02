using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Hospital
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

       
        public string Code { get; set; }
        public Decimal Cost { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfbeds { get; set; }
        public string EstablishmentYesr { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public string PostCode { get; set; }
        public string IconImage { get; set; }
        public string Infrastructure { get; set; }
        public string Spacialities { get; set; }
        public string CustomNo { get; set; }

        
        public int PriorityOrder { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }

        public Guid TreatmentId { get; set; }
        public Guid TreatmentDetailID { get; set; }
    }
}