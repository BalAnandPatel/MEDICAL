using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Enquiry
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; } 
        public string Email { get; set; }
        public string County  { get; set; } 
        public bool IsActive { get; set; }
    }
}