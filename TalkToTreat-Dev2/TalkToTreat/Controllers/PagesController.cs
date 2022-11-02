using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Models;
using TalkToTreatService.Users;

namespace TalkToTreat.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult Cardioc()
        {
            return View();
        }
        public ActionResult CompanyLicence()
        {
            return View();
        }
        public ActionResult CompanySupport()
        {
            return View();
        }
        public ActionResult FAQuestions()
        {
            return View();
        }
        public ActionResult Medicine()
        { 
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult Radiology()
        {
            return View();
        }
        public ActionResult Surgery()
        {
            return View();
        }
        public ActionResult TermsAndConditions()
        {
            return View();
        }
        public ActionResult WomenHealth()
        {
            return View();
        }
    }
}