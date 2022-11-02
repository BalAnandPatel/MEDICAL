using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreat.Models
{
    public class ViewResponseModel
    {
        public Appointment Appointment { get; set; }
        public List<UserVallet> UserValletList { get; set; }

        public string Remark { get; set; }
        public string name { get; set; }
        public string age { get; set; }
    }
}