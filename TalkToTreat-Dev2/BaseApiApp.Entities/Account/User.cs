using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.Account
{
    public class User
    {    
        public string Country { get; set; } 
        public string Mobile { get; set; } 
        public string Email { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string  Address { get; set; } 
        public string Phone { get; set; }   
        public string AccountStatus { get; set; }
        public int PatientAge { get; set; }
        public string Remark { get; set; }
        public int WrongAttempt { get; set; }

        public bool IsBlocked { get; set; }
        public string Textvalue { get; set; }
      
    }
}
