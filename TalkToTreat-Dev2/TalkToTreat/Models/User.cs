using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreat.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Country { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string AccountStatus { get; set; }
        public int PatientAge { get; set; }
        public string Remark { get; set; }
        public int WrongAttempt { get; set; }

        public bool IsBlocked { get; set; }
        public string Textvalue { get; set; }
        public int Save()
        {
            int returnbyproc = 0;

            try
            {
                var ProfileJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procUpdateUserProfile_new");
                _mysave.AddParameter("@UserId", this.UserId.ToString());
                _mysave.AddParameter("@FirstName", this.Name);
                _mysave.AddParameter("@LastName", "");
                _mysave.AddParameter("@Email", this.Email);
                _mysave.AddParameter("@PatientCondition", this.Remark);
                _mysave.AddParameter("@Gender", "");
                _mysave.AddParameter("@PhoneNo", this.MobileNo);
                _mysave.AddParameter("@Country", this.Country);
                _mysave.AddParameter("@Age", this.PatientAge);
                _mysave.AddParameter("@MobileNo", this.MobileNo);

                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return returnbyproc;
        }
    } 
}
