using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.Account
{
    public class Login
    {
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
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
        public LoggedinUser UserInfo { get; set; }
    }
    public class LoggedinUser
    {
        public Guid UserId { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
    }
    public enum TokenStatus
    {
        Unauthorized = 404,
        Valid = 200,
        Invalid = 400

    }
}
