using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.AdminAccount
{
    public class Registration
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public byte UserType { get; set; }
    }
}
