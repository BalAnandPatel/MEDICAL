using TalkToTreatService.Entities;
using TalkToTreatService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Models;
using System.IO;

namespace TalkToTreat.Controllers
{
    public class UserHealthController : Controller
    {
        private readonly IUserService _userService;
        public UserHealthController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>(); 
        }
         
        [HttpPost]
        public ActionResult UpdatehealthCondition(TalkToTreat.Models.User model)
        {
            string success = null; 
            try
            {
                if (Session["userdetails"] != null)
                {
                    string userdetails = null;
                    string[] details = null;
                    userdetails = Session["userdetails"].ToString();
                    details = userdetails.Split('(', '&', '#', '$', ')');
                    model.UserId = Guid.Parse(details[0]);
                    var returnbyproc = model.Save();
                    if (returnbyproc > 0)
                    {
                        success = "profile Updated successfully !!!";
                    }
                }
                TempData["HealthwalletMessage"]= success;
                TempData.Keep();
                return (RedirectToAction("Healthwallet", "User", new { @result = "Success", UserValid = success }));
            }
            catch (Exception ex)
            {
                success = "Some error accurred. Please try later !!!";
                TempData["HealthwalletMessage"] = success;
                TempData.Keep();
                return (RedirectToAction("Index", "User", new { @result = "Success", UserValid = success }));
            }
        } 
    }
}
