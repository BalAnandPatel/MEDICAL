using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
    public class TreatmentPackage
    {
		public Guid Id { get; set; }
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
