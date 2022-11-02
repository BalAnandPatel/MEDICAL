using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseApiApp.Models.Account
{
    public class RegistrationModel
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
        public int PatientAge { get; set; }
        public string Country { get; set; }
        public byte UserType { get; set; }
    }

    public class LoginModel
    {
        public string EmailId { get; set; }
        public string Password { get; set; } 
    }
    public class SocialLoginModel
    {
        public string UserName { get; set; }
        public bool IsPhoneNumber { get; set; }
    }
    public class UserModel
    {
        public int state { get; set; }
        public int district { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Deptaddr { get; set; }
        public string Conaddr { get; set; }
        public string Contel1 { get; set; }
        public string Contel2 { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string DMName { get; set; }
        public string DMAddress { get; set; }
        public string DMtel1 { get; set; }
        public string DMtel2 { get; set; }
        public string DMEmail { get; set; }

        public string EntName { get; set; }
        public string EntDepartment { get; set; }
        public string EntDesignation { get; set; }
        public decimal farmId { get; set; }
        public decimal GroupNo { get; set; }
        public string AccountStatus { get; set; }
        public int RailZId { get; set; }
        public int RailDId { get; set; }
        public int WrongAttempt { get; set; }

        public bool IsBlocked { get; set; }
        public string Textvalue { get; set; }

    }

    public class EnquiryModel
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; } 
        public string Remark { get; set; }
        public int PatientAge { get; set; }
        public byte UserType { get; set; }
    }

    public class Appointmentmodel
    {  
        public Guid DoctorId { get; set; }
        public string AppointmentNumber { get; set; }
        public string FullName { get; set; } 
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string AppointMentType { get; set; }
        public int PatientAge { get; set; }
        public DateTime AppointMentDate { get; set; } 
        public string Country { get; set; }
        public string Password { get; set; }
    }

    public class UserProfileModel
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; } 
        public string Remark { get; set; }
        public int PatientAge { get; set; }
        public string AboutPatient { get; set; }
        public string Country { get; set; }
        public string ProfileImage { get; set; }
    }

    public class UserValletModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string DownloadURL { get; set; }
        public byte[] FileUrl { get; set; }
    }
}