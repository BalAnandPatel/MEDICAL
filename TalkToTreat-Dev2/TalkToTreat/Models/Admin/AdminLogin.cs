using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalkToTreat.Models
{
    public class AdminLogin
    {
        [Required]
        [MaxLength(12)]
        [MinLength(1)]
        public string User_name { get; set; }
        [Required]
        public string Password { get; set; }
        public string Security_code { get; set; }
        public string EmailID { get; set; }

        public int? Log_id { get; set; }
        public string logmeassage { get; set; }

        public int? User_Id { get; set; }
        public int? State_Id { get; set; }
        public int? District_Id { get; set; }

        public string hdrandom { get; set; }

        

    }
    public static class LoginX
    {
        public static string _rndomcode { get; set;}
}
}