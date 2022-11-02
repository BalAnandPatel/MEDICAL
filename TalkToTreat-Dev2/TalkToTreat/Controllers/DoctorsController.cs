using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using TalkToTreat.Models;
using System.Web.Mvc;
using TalkToTreatService.Users;
using AutoMapper;
using System.Threading;
using BaseApiApp.Entities.Enum;

namespace TalkToTreat.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IUserService _userService;
        public DoctorsController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();

        }
        DoctorsService _doctorService = new DoctorsService();
        MasterService _masterService = new MasterService();
        HospitalService _hospitalService = new HospitalService();
        public ActionResult Index(string country, string city, string treat, string hosp)
        {
            ViewBag.SelectedHospital = hosp;
            ViewBag.SelectedCountry = country;
            ViewBag.SelectedCity = city;
            ViewBag.SelectedTreatment = treat;

            List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
            var HospialList = _hospitalService.GetAllHospitals().OrderBy(a => a.Name);
            foreach (var item in HospialList)
            {
                TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
                hospital.Code = item.Code;
                hospital.Name = item.Name;
                hospital.Id = item.Id;
                hospitals.Add(hospital);
            }
            ViewBag.Hospitals = hospitals;

            List<TalkToTreat.Models.Treatment> treatments = new List<TalkToTreat.Models.Treatment>();
            var treatmentsList = _masterService.GetTreatments().OrderBy(a => a.Name);
            foreach (var item in treatmentsList)
            {
                TalkToTreat.Models.Treatment treatment = new TalkToTreat.Models.Treatment();
                treatment.Code = item.Code;
                treatment.Cost = item.Cost;
                treatment.Name = item.Name;
                treatment.Id = item.Id;
                treatments.Add(treatment);
            }
            ViewBag.TreatmentsList = treatments;


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
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();
            if (doctorsList != null && doctorsList.Count() > 0)
            {
                if (!string.IsNullOrEmpty(country))
                {
                    doctorsList = doctorsList.Where(a => a.Country.ToLower() == country.ToLower()).ToList();
                }

                if (!string.IsNullOrEmpty(city))
                {
                    doctorsList = doctorsList.Where(a => a.City.ToLower() == city.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(treat))
                {
                    doctorsList = doctorsList.Where(a => a.Department.ToLower() == treat.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(hosp))
                {
                    doctorsList = doctorsList.Where(a => a.Hospital.ToLower() == hosp.ToLower()).ToList();
                }
            }

            foreach (var item in doctorsList)
            {
                Doctors doc = new Doctors();
                doc.Code = item.Code;
                doc.Name = item.Name;
                doc.Department = item.Department;
                doc.Designation = item.Designation;
                doc.Description = item.Description;
                doc.Image = item.Image;
                doc.Hospital = item.Hospital;
                doc.HospitalId = item.HospitalID;
                doc.City = item.City;
                doc.Id = item.Id;
                doc.Cost = item.Cost;
                doc.Award = item.Award;
                doc.YearsofExp = item.YearsofExp;
                doc.Gender = item.Gender;
                doctors.Add(doc);
            }
            return View(doctors);
        }
        public JsonResult GetDoctorsbySearch(string search)
        {
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetDoctorsBySearch(search);
            foreach (var item in doctorsList)
            {
                Doctors doc = new Doctors();
                doc.Code = item.Code;
                doc.Name = item.Name;
                doc.Department = item.Department;
                doc.Designation = item.Designation;
                doc.Description = item.Description;
                doc.Image = item.Image;
                doc.Hospital = item.Hospital;
                doc.City = item.City;
                doc.Id = item.Id;
                doc.Cost = item.Cost;
                doc.Award = item.Award;
                doc.YearsofExp = item.YearsofExp;
                doctors.Add(doc);
            }

            return Json(doctors, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string id)
        {
            Guid doctorId = Guid.Parse(id);
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();

            var doctorDetail = doctorsList.Where(a => a.Id == doctorId).FirstOrDefault();
            Doctors doctor = new Doctors();
            doctor.Code = doctorDetail.Code;
            doctor.Cost = doctorDetail.Cost;
            doctor.Name = doctorDetail.Name;
            doctor.Department = doctorDetail.Department;
            doctor.Designation = doctorDetail.Designation;
            doctor.Description = doctorDetail.Description;
            doctor.Image = doctorDetail.Image;
            doctor.Cost = doctorDetail.Cost;
            doctor.Award = doctorDetail.Award;
            doctor.Hospital = doctorDetail.Hospital;
            doctor.HospitalId = doctorDetail.HospitalID;
            doctor.YearsofExp = doctorDetail.YearsofExp;
            doctor.Id = doctorDetail.Id;

            doctor.Gender = doctorDetail.Gender;

            doctor.Degree = doctorDetail.Degree;
            doctor.Experience = doctorDetail.Experience;
            doctor.AdditionalInfo = doctorDetail.AdditionalInfo;

            return View(doctor);
        }
        public ActionResult ConsultOnline(string id)
        {
            var AllCities = _masterService.GetLocations();
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
            Guid doctorId = Guid.Empty;
            try
            {
                doctorId = Guid.Parse(id);
            }
            catch
            {

                RedirectToAction("Registration", "Home");
            }
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();

            var doctorDetail = doctorsList.Where(a => a.Id == doctorId).FirstOrDefault();
            Doctors doctor = new Doctors();
            doctor.Code = doctorDetail.Code;
            doctor.Cost = doctorDetail.Cost;
            doctor.Name = doctorDetail.Name;
            doctor.Department = doctorDetail.Department;
            doctor.Designation = doctorDetail.Designation;
            doctor.Description = doctorDetail.Description;
            doctor.Image = doctorDetail.Image;
            doctor.Id = doctorDetail.Id;


            doctor.Award = doctorDetail.Award;
            doctor.Hospital = doctorDetail.Hospital;
            doctor.HospitalId = doctorDetail.HospitalID;
            doctor.YearsofExp = doctorDetail.YearsofExp;
            doctor.Gender = doctorDetail.Gender;

            return View(doctor);
        }

        [HttpPost]
        [ActionName("BookAppointment")]
        public ActionResult BookAppointment(Appointment _registration)
        {
            string invalid = null;
            try
            {
                if (string.IsNullOrEmpty(Request["DoctorId"]))
                {
                    return (RedirectToAction("index", "Home", new { UserValid = "Something went wrong , Please try again" }));
                }

                string docId = Request["DoctorId"].ToString();
                docId = docId.Replace(",", "");
                _registration.DoctorId = Guid.Parse(docId);

                if (string.IsNullOrEmpty(_registration.Password))
                {
                    Random generator = new Random();
                    string r = generator.Next(0, 1000000).ToString("D6");
                    _registration.Password = r;
                }
                var user = new TalkToTreatService.Entities.User
                {
                    Email = _registration.EmailId,
                    Name = _registration.FullName,
                    Password = _registration.Password,
                    Mobile = _registration.MobileNo,
                    PatientAge = _registration.PatientAge,
                    Gender = _registration.Gender,
                    AppointMentType = _registration.AppointMentType,
                    AppointMentDate = _registration.AppointMentDate.Date,
                    DoctorId = _registration.DoctorId,
                };

                var userDetails = (Session["userdetails"] ?? string.Empty).ToString();
                if (!string.IsNullOrWhiteSpace(userDetails))
                {
                    // user registered and logged in
                    string userId = userDetails.Split("(&#$)").First();
                    user.UserId = new Guid(userId);
                }
                else
                {
                    string UserName = null;

                    // user registered and not logged in 
                    TalkToTreatService.Entities.User identifiedUser = _userService.GetUserByIdentifier(_registration.EmailId);
                    if (identifiedUser != null)
                    {
                        user.UserId = identifiedUser.RecordId;
                        UserName = _registration.EmailId;
                    }
                    else
                    {
                        //user not registered in the system
                        var respUser = _userService.InsertUser(user);
                        if (respUser.IsValid)
                        {
                            user.UserId = respUser.RecordId;

                            TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                            var body = SendMailconfirmation(_registration);

                            sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", body);
                        }
                    }
                }

                //add to Enquiry (Appointment)
                var resp = _userService.InsertEnquiry(user);
                if (resp.IsValid)
                {
                    _registration.AppointmentNumber = resp.MessageCode;
                    TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                    
                    var body = SendMailconfirmation(_registration);
                    //email to user and copy to admin
                    sendEmail.Send("", 
                                    _registration.EmailId, 
                                    System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString(), 
                                    "", 
                                    "Your appointment has been booked", 
                                    body);
                                      

                    return (RedirectToAction("ApointmentConfirmation", "Home"));
                }
            }
            catch (Exception ex)
            {
                invalid = ex.Message + GetLineNumber(ex);
                return (RedirectToAction("index", "Home", new { UserValid = invalid }));
            }
            return View();
        }


        //[HttpPost]
        //[ActionName("BookAppointment1")]
        //public ActionResult BookAppointment1(Appointment _registration)
        //{

        //    if (string.IsNullOrEmpty(Request["DoctorId"]) == false)
        //    {
        //        string docId = Request["DoctorId"].ToString();
        //        docId = docId.Replace(",", "");
        //        _registration.DoctorId = Guid.Parse(docId);
        //    }
        //    string invalid = null;
        //    try
        //    {
        //        if (string.IsNullOrEmpty(_registration.Password))
        //        {
        //            Random generator = new Random();
        //            string r = generator.Next(0, 1000000).ToString("D6");
        //            _registration.Password = r;
        //        }
        //        var user = new TalkToTreatService.Entities.User
        //        {
        //            Email = _registration.EmailId,
        //            Name = _registration.FullName,
        //            Password = _registration.Password,
        //            Mobile = _registration.MobileNo,
        //            PatientAge = _registration.PatientAge,
        //            Gender = _registration.Gender,
        //            AppointMentType = _registration.AppointMentType,
        //            AppointMentDate = _registration.AppointMentDate,
        //            DoctorId = _registration.DoctorId
        //        };

        //        var resp = _userService.InsertEnquiry(user);
        //        if (resp.IsValid)
        //        {
        //            _registration.AppointmentNumber = resp.MessageCode;
        //            TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
        //            var body = SendMailconfirmation(_registration);

        //            sendEmail.Send("", _registration.EmailId, "", "", "Appointment Request Confirmation", body);
        //            //Thread email = new Thread(delegate ()
        //            //{
        //            //    sendEmail.Send("", _registration.EmailId, "", "", "Appointment Request Confirmation", body);
        //            //});

        //            var UserInsertresp = _userService.InsertUser(user);
        //            if (UserInsertresp.IsValid)
        //            {
        //                var Regbody = SendRegistrationMailconfirmation(_registration);
        //                sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", Regbody);

        //                //Thread email = new Thread(delegate ()
        //                //{
        //                //    sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", Regbody);
        //                //});
        //            }

        //            return (RedirectToAction("ApointmentConfirmation", "Home"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        invalid = "Something went wrong , Please try again";
        //        return (RedirectToAction("index", "Home", new { UserValid = invalid }));
        //    }
        //    return View();
        //}
        protected string SendMailconfirmation(Appointment model)
        {            
            string body = "";
            try
            {
                string to = model.EmailId;               
                body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/AppoitmentBook.html"));
                body = body.Replace("$$lblAppointmentNumber$$", model.AppointmentNumber);
                body = body.Replace("$$Name$$", model.FullName);
                body = body.Replace("$$lblAppointmentDate$$", model.AppointMentDate.ToShortDateString());

            }
            catch (Exception es)
            {

            }
            return body;
        }
        protected string SendRegistrationMailconfirmation(Appointment model)
        {
            string name = "";
            string subject, loginid, password;
            string body = "";
            try
            {
                string date = "";

                string to = model.EmailId;
                #region
                body = @" <div style='margin: auto; background: #fff url(http://qlook.bz/images/topborder.png) top left repeat-x; border: 2px solid #ddd; width: 700px; padding-bottom: 7px; -webkit-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); -moz-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65);'>
           <table width='700' style='background: #14c5aa; margin: 0px auto; padding: 45px 0px;'>
        <tr>
            <td>

                <table cellpadding='0' cellspacing='0' style='background: none repeat scroll 0 0 #fff; border: 7px solid #dcdcdc; color: #333; font-family: Arial,Helvetica,sans-serif; margin: 0 48px; width: 585px;'>


                    <tr>
                        <td>

                            <table width='585'>
                                <tr>
                                    <td style='width: 250px; height: 70px; background: #223a66;'><a   href='https://talktotreat.com' >
                                        <img src='https://talktotreat.com/Assets/images/logo.png' height='45px' border='0' style='height: 45px; margin-left: 6px; float: left;' />
                                    </a><h2><span style='color: white;height:65px; margin-left:5px'>Talk to Treat</span></h2></td>
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 585px;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div>
                                <div class='clear'></div>

                                <table style='border-top: 1px solid rgb(204, 204, 204); height: auto; padding-bottom: 0px; padding-left: 12px; padding-top: 6px; width: 585px;'>
                                    <tr>
                                        <td>
                                            <h1 style='font-size: 15px; font-family: arial; font-weight: normal; margin: 15px 0px 15px; color: #3f3f42; text-decoration:none;'> Dear,<span style='color:#3b7bd1;'> $$Name$$ </span> </h1>
                                            <p style='font-size: 13px;   margin: 0px; color: #484848;'>Thanks for signing up on TalkToTreat.Com </p>

                                             <br/>
<p style='font-size:11px; font-family: arial; font-weight: normal; margin: 7px 4px 5px 0px; color: #3f3f42;'> <b> Your Appointment details are given below. <b></p> 
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Username :</b> $$lblEmail$$ </p>
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Password : </b> $$body$$ </p> 

                                            </table>
                                            <div style='clear: both'></div>
                                            <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 15px 0px 7px 15px; color: #3f3f42;'>
                                                If you have any issues verfying your email, we will be happy to help you.
                                            </p>
                                            <strong style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 0px 0 0 15px; color: #131313;'>You can contact  us on support@hatalktotreat.com</strong>
                                            <div style='clear: both'></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table width='554'>
                                                <tr>
                                                    <td width='70%' style='padding-top:20px;'>

                                                        <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 6px 0px 7px 15px; float: left; color: #3f3f42;'>
We look forward to seeing you on our website.  <br/><br/>
                                                            Sincerely,
                <br />
                                                            Talk To Treat Team<br/><br/>
talktotreat.com

                                                        </p>
                                                    </td>                                                   
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                </table>
                                <div class='clear'></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </div>";
                #endregion
                //mail body will be here
                body = body.Replace("$$lblEmail$$", model.EmailId);
                body = body.Replace("$$Name$$", model.FullName);
                body = body.Replace("$$body$$", model.Password);

            }
            catch (Exception es)
            {

            }
            return body;
        }


        public int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }


        [HttpPost]
        public ActionResult Filter(int ?country, int ?city, Guid? treat, Guid? hosp,bool? bestHospital)
        {           
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();
            if (doctorsList != null && doctorsList.Count() > 0)
            {
                if ((country ?? 0) != 0)
                {
                    doctorsList = doctorsList.Where(a => a.CountryId == country).ToList();
                }
                if ((city ?? 0) != 0)
                {
                    doctorsList = doctorsList.Where(a => a.CityId == city).ToList();
                }
                if ((treat ?? Guid.Empty) != Guid.Empty)
                {
                    doctorsList = doctorsList.Where(a => a.TreatmentId == treat).ToList();
                }
                if ((hosp ?? Guid.Empty) != Guid.Empty)
                {
                    doctorsList = doctorsList.Where(a => a.HospitalID == hosp).ToList();
                }
                if(bestHospital??false)
                {
                    //add logic for best doctor
                }
            }

            foreach (var item in doctorsList)
            {
                Doctors doc = new Doctors();
                doc.Code = item.Code;
                doc.Name = item.Name;
                doc.Department = item.Department;
                doc.Designation = item.Designation;
                doc.Description = item.Description;
                doc.Image = item.Image;
                doc.Hospital = item.Hospital;
                doc.HospitalId = item.HospitalID;
                doc.City = item.City;
                doc.Id = item.Id;
                doc.Cost = item.Cost;
                doc.Award = item.Award;
                doc.YearsofExp = item.YearsofExp;
                doc.Gender = item.Gender; 
                doctors.Add(doc);
            }

            return PartialView("~/Views/Doctors/_DoctorListPartial.cshtml", doctors);
           
        }


        public ActionResult AppointmentList()
        {
            if (Session["userdoctor"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["userdoctor"].ToString();
                details = userdetails.Split('(', '&', '#', '$', ')');

                string username = details[0];

                int pagenum = 0;
                int totalRecords = 0;
                string DoctorId = "";
                if (Request["pagenum"] != null)
                {
                    pagenum = Convert.ToInt32(Request["pagenum"]);
                    TempData["pagenum"] = pagenum;
                }
                DoctorId = username;
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                
                object[] obj = _appointment.GetAppointmentDetailByDoctor(pagenum, DoctorId);

                list = (List<Appointment>)obj[0];
                if (list != null && list.Any())
                {
                    totalRecords = Convert.ToInt32(list[0].TotalRecord);
                }
                if (pagenum >= 0 && pagenum <= 9)
                {
                    ViewBag.PreviousData = 0;
                    ViewBag.NextData = 10;
                }
                else
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["Previous"]);
                    ViewBag.NextData = Convert.ToInt32(Request["Next"]);
                }
                if (Request["PreviousData"] != null)
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["PreviousData"]) - 10;
                    ViewBag.NextData = Convert.ToInt32(Request["PreviousData"]);
                }
                if (Request["NextData"] != null)
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["NextData"]);
                    ViewBag.NextData = Convert.ToInt32(Request["NextData"]) + 10;
                }
                ViewBag.totalRecords = totalRecords;
                ViewBag.MyPageNumber = pagenum;
                return View(list);
            }
            else { return Redirect("/Home/Index"); }
        }

        public ActionResult AppointmentListFuture()
        {
            if (Session["userdoctor"] != null)
            {
                string doctorDetails = null;
                string[] details = null;
                doctorDetails = Session["userdoctor"].ToString();
                details = doctorDetails.Split('(', '&', '#', '$', ')');

                string doctorId = details[0];

                int pagenum = 0;
                int totalRecords = 0;
                string Searchvalue = "";
                if (Request["pagenum"] != null)
                {
                    pagenum = Convert.ToInt32(Request["pagenum"]);
                    TempData["pagenum"] = pagenum;
                }
                Searchvalue = doctorId;
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                object[] obj = _appointment.GetAppointmentDetailByDoctorFuture(pagenum, Searchvalue);
                list = (List<Appointment>)obj[0];
                if (list != null && list.Any())
                {
                    totalRecords = Convert.ToInt32(list[0].TotalRecord);
                }
                if (pagenum >= 0 && pagenum <= 9)
                {
                    ViewBag.PreviousData = 0;
                    ViewBag.NextData = 10;
                }
                else
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["Previous"]);
                    ViewBag.NextData = Convert.ToInt32(Request["Next"]);
                }
                if (Request["PreviousData"] != null)
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["PreviousData"]) - 10;
                    ViewBag.NextData = Convert.ToInt32(Request["PreviousData"]);
                }
                if (Request["NextData"] != null)
                {
                    ViewBag.PreviousData = Convert.ToInt32(Request["NextData"]);
                    ViewBag.NextData = Convert.ToInt32(Request["NextData"]) + 10;
                }
                ViewBag.totalRecords = totalRecords;
                ViewBag.MyPageNumber = pagenum;
                return View(list);
            }
            else { return Redirect("/Home/Index"); }
        }

        public ActionResult AppointmentDetail(string id)
        {
            Appointment _appointment = new Appointment();
            _appointment = _appointment.GetAppointmentDetail(Guid.Parse(id));
            return View(_appointment);
        }

        public ActionResult SaveCancelRequest(string Id, string CancelRegion)
        {
            Appointment _appointment = new Appointment();

            var appDetails = _appointment.GetAppointmentDetail(Guid.Parse(Id));

            string body = "";
            //mail to user and admin          
            body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/AppoitmentCancelRequest.html"));
            body = body.Replace("$$lblAppointmentNumber$$", appDetails.AppointmentNumber);
            body = body.Replace("$$Name$$", appDetails.FullName);
            body = body.Replace("$$lblAppointmentDate$$", appDetails.AppointMentDate.ToLongTimeString());

            try
            {
                //email to user and copy to admin
                TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                sendEmail.Send("", appDetails.EmailId, System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString(), "", "Your appointment requested to cancel", body);
            }
            catch (Exception)
            {

            }

            //save in doctor case
            _appointment.SaveCancelRequest(Id, CancelRegion, (int)CancelledBY.Doctor);

            return Redirect("/Doctors/AppointmentList");
        }

        public ActionResult viewResponse(string id)
        {
            ViewResponseModel _viewResponseModel = new ViewResponseModel();
            Appointment _appointment = new Appointment();

            List<TalkToTreatService.Entities.UserVallet> _lstUserVallet = new List<TalkToTreatService.Entities.UserVallet>();
            List<TalkToTreat.Models.UserVallet> _uservalets = new List<Models.UserVallet>();

            _appointment = _appointment.GetAppointmentDetail(Guid.Parse(id));
            _lstUserVallet = _userService.GetUserValletByGuid(Guid.Parse(_appointment.SiteUserId)).ToList();
                                 
            foreach (var item in _lstUserVallet)
            {
                TalkToTreat.Models.UserVallet UserVallet = new Models.UserVallet
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Description = item.Description,
                    Id = item.Id,
                    Type = item.Type,
                };
                _uservalets.Add(UserVallet);
            }

            _viewResponseModel.UserValletList = _uservalets;
            _viewResponseModel.Appointment = _appointment;

            //add user remark only

            var userDetails = _userService.GetUserByGuid(Guid.Parse(_appointment.SiteUserId));
            _viewResponseModel.Remark = userDetails.Remark;
            _viewResponseModel.name = userDetails.Name;
            _viewResponseModel.age = userDetails.PatientAge.ToString();
            return View(_viewResponseModel);
        }
    }
    
}
