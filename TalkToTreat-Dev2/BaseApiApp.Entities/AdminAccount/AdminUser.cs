using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.AdminAccount
{
    public class AdminUser
    {
        public Guid AdminUserId{ get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}