using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
    public class User : BaseEntity
    {
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Userid { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string AccountStatus { get; set; }
        public int PatientAge { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// Gets or sets the user LastName
        /// </summary>
        public string Gender { get; set; } 
        /// <summary>
        /// Gets or sets the user LastName
        /// </summary>
        public string PostCode { get; set; }  
        /// <summary>
        /// Gets or sets the user LastName
        /// </summary>
        public string ReviewNickname { get; set; } 
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }  

        /// <summary>
        /// gets or sets the salt
        /// </summary>
        public string Salt { get; set; }    

        public Guid Token { get; set; }

        public string Culture { get; set; }
        
        /// <summary>
        /// is this a Guest user
        /// </summary>
        public bool IsGuest { get; set; }
        public bool IsGhostLogin { get; set; }
        public string AdminUserName { get; set; }
        public bool NewsLetterSubscribed { get; set; }
        public bool IsBlackListed { get; set; }
        public bool IsBlocked { get; set; }

        public string UserSourceType { get; set; } 
        public List<Product> WishListLines { get; set; }
        public string UserMessage { get; set; }

        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public bool ExistingCustomer { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string UniqueCode { get; set; }
        public string StoreName { get; set; }
        public string Image { get; set; }
        public DateTime NextOrderDate { get; set; }
        public string LastOrderDate { get; set; } 

        //TODO: Take all the following properties out of this class and create a separate Company class.
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public string CompanyWebsite { get; set; }
        public string TradingCurrency { get; set; } 
        public int? IsApproved { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Guid CompanyId { get; set; }  
        public string RegisteredNumber { get; set; }
        public bool IsRegistered { get; set; } 
        public bool IsCreatedByAdmin { get; set; }
        public string MobileNo { get; set; } 
        public string RegistrationSource { get; set; }
        public bool HasSubscribed { get; set; } 
        public DateTime AppointMentDate { get; set; }
        public string AppointMentType { get; set; }
        public string PhoneNumber { get; set; }
        public Guid DoctorId { get; set; } 
    }

    public class UserVallet
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
