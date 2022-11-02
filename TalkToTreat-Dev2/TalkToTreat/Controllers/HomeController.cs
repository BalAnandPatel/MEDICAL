using BaseApiApp.Framework.Cryptography;
using Service.MasterData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Models;
using TalkToTreatService.Users;

namespace TalkToTreat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        MasterService _masterService = new MasterService();
        Treatment _treatment = new Treatment();
        DoctorsService _doctorService = new DoctorsService();
        HospitalService _hospitalService = new HospitalService();
        Quote _quote = new Quote();

        public HomeController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();
        }
        public ActionResult Index()
        {           
            List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
            var HospialList = _hospitalService.GetAllHospitals();
            foreach (var item in HospialList)
            {
                TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
                hospital.Code = item.Code;
                hospital.Name = item.Name;
                hospital.Id = item.Id;
                hospital.City = item.City;
                hospital.Image = item.Image;
                hospital.IconImage = item.IconImage;
                hospitals.Add(hospital);
            }
            ViewBag.Hospitals = hospitals;
            var DistinctCities = hospitals.Select(x => x.City).Distinct().ToList();
            ViewBag.DistinctCities = DistinctCities;

            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();
            foreach (var item in doctorsList)
            {
                Doctors doc = new Doctors();
                doc.Code = item.Code;
                doc.Cost = item.Cost;
                doc.Name = item.Name;
                doc.Image = item.Image;
                doc.Description = item.Description;
                doc.Id = item.Id;
                doc.Designation = item.Designation;
                doc.Department = item.Department;
                doc.City = item.City;
                doc.Gender = item.Gender;
                doctors.Add(doc);
            }
            ViewBag.Doctors = doctors.ToList();
            //ViewBag.Doctors = doctors.Take(7).ToList();

            ViewBag.Treatments = _treatment.GetTreatmentListMaterHomePage(String.Empty);           

            List<Testimonial> testimonials = new List<Testimonial>();
            var testimonialList = _masterService.GetTestimonial();
            foreach (var item in testimonialList)
            {
                Testimonial testimonial = new Testimonial();
                testimonial.ID = item.ID;
                testimonial.Name = item.Name;
                testimonial.Treatment = item.Treatment;
                testimonial.City = item.City;
                testimonial.Description = item.Description;
                testimonial.IsActive = item.IsActive;
                testimonial.Image = item.Image;               
                testimonials.Add(testimonial);
            }
            ViewBag.Testimonials = testimonials;

            ViewBag.CountryWiseCities = GetCountryWiseCities();
            ViewBag.Services = GetServices();

            //All Cities
            var AllCities = _masterService.GetLocations().OrderBy(a => a.Name);
            List<Locations> AllCitiesEntity = new List<Locations>();
            foreach (var Cityitem in AllCities)
            {
                Locations loc = new Locations();
                loc.CityId = Cityitem.CityId;
                loc.Name = Cityitem.Name;
                loc.CountryName = Cityitem.CountryName;
                loc.CountryId = Cityitem.CountryId;
                loc.StateId = Cityitem.StateId;
                loc.StateName = Cityitem.StateName;
                loc.Code = Cityitem.Code;
                AllCitiesEntity.Add(loc);
            }
            ViewBag.AllCities = AllCitiesEntity;

            //Treatments
            ViewBag.TreatmentsForGetQuote = _masterService.GetTreatments();

            return View();
        }
         
        public ActionResult Register()
        {
            if (Request["UserValid"] != null)
            {
                ViewBag.Invalid = Request["UserValid"].ToString();
            }

            var AllCities = _masterService.GetLocations().OrderBy(a => a.Name);
            List<Locations> AllCitiesEntity = new List<Locations>();
            foreach (var Cityitem in AllCities)
            {
                Locations loc = new Locations();
                loc.CityId = Cityitem.CityId;
                loc.Name = Cityitem.Name;
                loc.CountryName = Cityitem.CountryName;
                loc.CountryId = Cityitem.CountryId;
                loc.StateId = Cityitem.StateId;
                loc.StateName = Cityitem.StateName;
                loc.Code = Cityitem.Code;
                AllCitiesEntity.Add(loc);
            }

            ViewBag.AllCities = AllCitiesEntity;

            return View(new Registration());
        }
       
        public ActionResult Login()
        {
            if (Request["UserValid"] != null)
            {
                ViewBag.Invalid = Request["UserValid"].ToString();
            }
            return View(new Login());
        }
        public ActionResult Service()
        {
            List<TalkToTreat.Models.Services> services = new List<TalkToTreat.Models.Services>();
            var servicesList = _masterService.GetServices();
            foreach (var item in servicesList)
            {
                TalkToTreat.Models.Services service = new TalkToTreat.Models.Services();
                service.Id = item.Id;
                service.Name = item.Name;
                service.ShortDescription = item.ShortDescription;
                service.Image = item.Image;
                service.Description = item.Description;
                service.IsActive = item.IsActive;
                services.Add(service);
            }
            return View(services);
        }

        public ActionResult Testimonial()
        {
            List<TalkToTreat.Models.Testimonial> testimonials = new List<TalkToTreat.Models.Testimonial>();
            var testimonialList = _masterService.GetTestimonial();
            foreach (var item in testimonialList)
            {
                TalkToTreat.Models.Testimonial testimonial = new TalkToTreat.Models.Testimonial();
                testimonial.ID = item.ID;
                testimonial.Name = item.Name;
                testimonial.Description = item.Description;
                testimonial.Image = item.Image;
                testimonial.Description = item.Description;
                testimonial.IsActive = item.IsActive;
                testimonial.City = item.City;
                testimonial.Treatment = item.Treatment;
                testimonials.Add(testimonial);
            }
            return View(testimonials);
        }

        [ChildActionOnly]
        public ActionResult OurServices()
        {
            return PartialView(GetServices());
        }
        public ActionResult About()
        {
            List<Testimonial> testimonials = new List<Testimonial>();
            var testimonialList = _masterService.GetTestimonial();
            foreach (var item in testimonialList)
            {
                Testimonial testimonial = new Testimonial();
                testimonial.ID = item.ID;
                testimonial.Name = item.Name;
                testimonial.Treatment = item.Treatment;
                testimonial.City = item.City;
                testimonial.Description = item.Description;
                testimonial.IsActive = item.IsActive;
                testimonial.Image = item.Image;
                testimonials.Add(testimonial);
            }
            ViewBag.Testimonials = testimonials;


            return View();
        }
        public ActionResult Confirmation()
        {
            return View();
        }
        public ActionResult ApointmentConfirmation()
        {
            return View();
        }
        public JsonResult GetItems(string text)
        {
            search _search = new search("GetItems");
            _search.AddParameter("@Searchvalue", text);
            List<SelectListItem> itemsList = _search.Getdropdown();
            return Json(itemsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login(Login _login)
        {
            string invalid = null;
            try
            {
                var resp = _userService.Login(_login.EmailId, _login.Password, true);
                if (resp != null)
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
            return (RedirectToAction("Index", "Home", new { UserValid = invalid }));
        }

        [HttpPost]
        public ActionResult Register(Registration _registration)
        {
            string invalid = null;
            try
            {
                var user = new TalkToTreatService.Entities.User
                {
                    Email = _registration.EmailId,
                    Name = _registration.FullName,
                    Mobile = _registration.MobileNo,
                    MobileNo = _registration.MobileNo,
                    Phone = _registration.MobileNo,
                    PhoneNumber = _registration.MobileNo,
                    Password = _registration.Password,
                    PatientAge = _registration.PatientAge??0,
                    Remark = _registration.Remark
                };

                var resp = _userService.InsertUser(user);
                if (resp.IsValid)
                {
                    TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();

                    string body = "";
                    body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/RegisterConfirm.html"));
                    body = body.Replace("$$lblEmail$$", _registration.EmailId);
                    body = body.Replace("$$Name$$", _registration.EmailId);
                    body = body.Replace("$$body$$", _registration.Password);

                    sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", body);

                    return (RedirectToAction("Confirmation", "Home"));
                }
                else
                {
                    invalid = resp.Message;
                    return (RedirectToAction("Register", "Home", new { UserValid = invalid }));
                }
            }
            catch (Exception ex)
            {
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("Register", "Home", new { UserValid = invalid }));
            }
            return View();
        }

        private List<CountryWiseCities> GetCountryWiseCities()
        {
            var result = new List<CountryWiseCities>();

            var hospitals = _hospitalService.GetAllHospitalsHomePage();
            var countries = hospitals.Select(x => x.Country).Distinct().ToList();
            foreach (string country in countries)
            {
                var cities = hospitals.Where(x => x.Country == country).Select(y => y.City).Distinct().ToList();
                var cityWiseHospitals = new List<CityWiseHospitals>();

                foreach (string city in cities)
                {
                    var cityHospitals = hospitals.Where(x => x.City == city && x.Country == country).ToList();
                    cityWiseHospitals.Add(new CityWiseHospitals()
                    {
                        City = city,
                        Hospitals = cityHospitals.ConvertAll(x => new Hospital()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            City = x.City,
                            Image = x.Image,
                            IconImage = x.IconImage
                        })
                    });
                }
                result.Add(new CountryWiseCities() { Country = country, Cities = cityWiseHospitals });
            }
            return result;
        }

        private List<Services> GetServices()
        {
            var result = new List<Services>();

            var services = _masterService.GetServices().Where(x => x.IsActive).ToList();
            result = services.ConvertAll(x => new Services()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Image = x.Image,
                Code = x.Code,
                IsActive = x.IsActive
            });
            return result;
        }

        //Change reset password
        public ActionResult ResetPassword()
        {
            if (Request["UserValid"] != null)
            {
                ViewBag.Invalid = Request["UserValid"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Login _login)
        {
            var user = _userService.GetUserByUserName(_login.EmailId);

            if (user != null && user.Email != null)
            {
                string Url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString() + "Home/ChangePassword/" + user.RecordId;
                string body = "";
                body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/ResetPassword.html"));
                body = body.Replace("$$Name$$", user.Name);
                body = body.Replace("$$URL$$", Url);

                TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();                           

                sendEmail.Send("", user.Email, "", "", "Reset Password", body);
                             

                ViewBag.ResetPasswordMessage = "Reset password mail is sent. Please check your inbox.";
            }
            else
            {
                ViewBag.ResetPasswordMessage = "User not found";
            }

            _login.EmailId = "";
            return View(_login);
        }

        public ActionResult ChangePassword(Guid Id)
        {
            var user = _userService.GetUserByGuid(Id);
            if (user != null)
            {
                ChangePassword changePass = new ChangePassword();
                changePass.UserId =Id;
                return View(changePass);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePass)
        {
            var user = _userService.GetUserByGuid(changePass.UserId);

            string salt = string.Empty;
            user.Password = PasswordHelper.EncryptPassword(ref salt, changePass.Password);


            if (user != null)
            {
                var returnbyproc = _userService.UpadtePassword(changePass.UserId,  salt, user.Password);
                if (returnbyproc)
                {
                    ViewBag.ResetPasswordMessage = "Password changed successfully";
                }
                else
                {
                    ViewBag.ResetPasswordMessage = "Please try again latter";
                }               
            }
            else
            {
                ViewBag.ResetPasswordMessage = "Invalid User";
            }
            changePass.Password = "";
            changePass.ConfirmPassword = "";
            return View(changePass);

        }

        //Get Quote
        public ActionResult GetQuote()
        {
            if (Request["UserValid"] != null)
            {
                ViewBag.Invalid = Request["UserValid"].ToString();
            }

            //All Cities
            var AllCities = _masterService.GetLocations().OrderBy(a => a.Name);
            List<Locations> AllCitiesEntity = new List<Locations>();
            foreach (var Cityitem in AllCities)
            {
                Locations loc = new Locations();
                loc.CityId = Cityitem.CityId;
                loc.Name = Cityitem.Name;
                loc.CountryName = Cityitem.CountryName;
                loc.CountryId = Cityitem.CountryId;
                loc.StateId = Cityitem.StateId;
                loc.StateName = Cityitem.StateName;
                loc.Code = Cityitem.Code;
                AllCitiesEntity.Add(loc);
            }
            ViewBag.AllCities = AllCitiesEntity;

            //Treatments
            ViewBag.TreatmentsForGetQuote = _masterService.GetTreatments();

            return View(new Quote());
        }

        [HttpPost]
        public ActionResult GetQuote(Quote model, List<HttpPostedFileBase> files)
        {
            string invalid = null;
            try
            {
                model.ID = Guid.NewGuid();
                var filesList = "";
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                        var ext = Path.GetExtension(file.FileName);
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = name.Replace(" ","").Replace(",","") + "_" + Guid.NewGuid() + ext;
                        var path = Path.Combine(Server.MapPath("~/Assets/Images/GetQuote/"), myfile);
                        file.SaveAs(path);

                        if (!string.IsNullOrEmpty(filesList))
                        {
                            filesList = filesList + "," + myfile;
                        }
                        else
                        {
                            filesList = myfile;
                        }
                        //model.FileUrl = myfile;
                    }
                }

                model.FileUrl = filesList;

                var resp = model.Save();

                if (resp>0)
                {
                    TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                    
                    string body = "";
                    body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/QuoteConfirm.html"));
                    body = body.Replace("$$Name$$", model.Name);
                    body = body.Replace("$$Subject$$", model.Subject);
                    body = body.Replace("$$Query$$", model.Query);

                    sendEmail.Send("", model.Email, "", "", "Quote Confirmation", body);
                    return (RedirectToAction("ConfirmationQuote", "Home"));
                }
                else
                {
                    invalid = "Something went wrong , Please try again";
                    return (RedirectToAction("GetQuote", "Home", new { UserValid = invalid }));
                }
            }
            catch (Exception ex)
            {
                throw;

                //invalid = "Something went wrong , Please try again";
                //return (RedirectToAction("GetQuote", "Home", new { UserValid = invalid }));
            }
            return View();
        }
        public ActionResult ConfirmationQuote()
        {
            return View();
        }


        //Login as doctor
        public ActionResult LoginDoctor()
        {
            if (Request["UserValid"] != null)
            {
                ViewBag.Invalid = Request["UserValid"].ToString();
            }
            return View(new Login());
        }

        [HttpPost]
        [ActionName("LoginDoctor")]
        public ActionResult LoginDoctor(Login _login)
        {
            string invalid = null;
            try
            {              
                var resp = _doctorService.LoginHomePage(_login.EmailId, _login.Password, true);
                if (resp != null)
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

                    userIdGuid = resp.Id;
                    username = resp.UserName;
                    Name = resp.Name;
                    EmployeeImageURl = resp.Image;
                    isblocked = false;

                    ViewBag.userid = userid;
                    ViewBag.username = username;
                    TempData["username"] = username;
                    Session["userdoctor"] = userIdGuid + "(&#$)" + username + "(&#$)" + Name + "(&#$)" + EmployeeImageURl;
                    return (RedirectToAction("AppointmentList", "Doctors"));
                }
                else
                {
                    invalid = "Incorrect userId or Password";
                    return (RedirectToAction("LoginDoctor", "Home", new { UserValid = invalid }));
                }
            }
            catch (Exception ex)
            {
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("Index", "Home", new { UserValid = invalid }));
            }
            invalid = "passed";
            return (RedirectToAction("Index", "Home", new { UserValid = invalid }));
        }


    }
}