using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration; 
using System.Data.SqlClient; 
using System.Web.Security;
using System.Security.Principal;
using System.Text;
using System.Security.Cryptography;
using TalkToTreatService.Users;
using TalkToTreat.Models;

namespace TalkToTreat.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class LoginController : Controller
    {

        private readonly IUserService _userService;
        public LoginController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create(Login _login)
        {
            return (RedirectToAction("Login", "Home", new {  }));
            string invalid = null;
            try
            { 

                var resp = _userService.Login(_login.EmailId, _login.Password,true);
                if (resp!=null)
                {
                    string PassMatch = null; 
                    long Id;
                    Guid userIdGuid;
                    int userid = 0;
                    string username = null; 
                    string Email = null;
                    string Name = null;
                    bool isblocked = true;
                    string EmployeeImageURl = null;
                    int? WrongAttempt = null;
                    string Blocked = null;

                    userIdGuid = resp.RecordId; 
                    username = resp.Username;
                    Name = resp.Name; 
                    EmployeeImageURl = resp.Image;
                    isblocked = resp.IsBlocked;
                     
                    ViewBag.userid = userid;
                    ViewBag.username = username; 
                    TempData["username"] = username;
                    Session["userdetails"] = userIdGuid + "(&#$)" + username + "(&#$)" + Name + "(&#$)" + EmployeeImageURl;
                     return (RedirectToAction("Index", "Home"));
                }
                else
                {
                    invalid = "Incorrect userId or Password";
                    return (RedirectToAction("Login", "Home", new { UserValid = invalid }));
                }
            }
            catch (Exception ex)
            {
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("Index", "Home", new { UserValid = invalid }));
            }
            invalid = "passed";
            return (RedirectToAction( "Index" ,"Home", new { UserValid = invalid }));
        }
        public ActionResult Logout()
        {
            Session["userdoctor"] = null;
            Session["userdetails"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}