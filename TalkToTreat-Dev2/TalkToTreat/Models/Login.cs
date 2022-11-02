using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TalkToTreat.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please Enter Email Address")] 
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        public int UserType { get; set; }
        public bool checkPwd { get; set; }
    }
    public class AuthToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid ApiAppId { get; set; }

        public TokenStatus Status { get; set; }
        public string IpAddress { get; set; }
        public string UserType { get; set; }
        public int StateId { get; set; }
    }
    public enum TokenStatus
    {
        Unauthorized = 404,
        Valid = 200,
        Invalid = 400

    }
    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
        // Implement IClientValidatable for client side Validation
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "checkbox", ErrorMessage = this.ErrorMessage } };
        }


    }

    public class ChangePassword
    {
        public Guid UserId { get; set; }
        public string Salt { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]                
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirm Password have to be the same")]
        public string ConfirmPassword { get; set; }
        
    }
}
