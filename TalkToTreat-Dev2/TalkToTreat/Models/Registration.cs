using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreat.Models
{
    public class Registration
    {
        [Required(ErrorMessage = "Please Enter FullName")]
        [Display(Name = "FullName")] 
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "UserName")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile")]
        [StringLength(15, ErrorMessage = "Please Enter Correct MobileNo", MinimumLength = 10)]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please Describe patient Condition")]
        public string Remark { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter valid Age")]
        [Display(Name = "PatientAge")]
        [RegularExpression(@"^\d+$")] 
        public int? PatientAge { get; set; }
        public byte UserType { get; set; }
    }

}
