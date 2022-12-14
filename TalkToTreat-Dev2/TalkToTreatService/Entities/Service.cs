using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string  Name { get; set; } 
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}