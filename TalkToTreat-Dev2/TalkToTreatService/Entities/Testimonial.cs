using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
	public class Testimonial
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Treatment { get; set; }
		public string City { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public bool IsActive { get; set; }

	}
}
