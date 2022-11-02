using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Filters;
using TalkToTreat.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using AutoMapper;
using System.Threading;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading.Tasks;
using TalkToTreat.Helper;
using BaseApiApp.Entities.Enum;

namespace TalkToTreat.Controllers
{
    [AdminUserAuthenticationFilter]
    public class ManageDoctorController : Controller
    {
        string from = "emailtest@roovea.com";
        string password = "Test@123";

        DoctorsService _doctorService = new DoctorsService();

        public ActionResult DoctorList()
        {
            int pagenum = 0;
            int totalRecords = 0;
            string Searchvalue = "";
            if (Request["pagenum"] != null)
            {
                pagenum = Convert.ToInt32(Request["pagenum"]);
                TempData["pagenum"] = pagenum;
            }
            if (String.IsNullOrEmpty(Request["Searchvalue"]) == false)
            {
                Searchvalue = Request["Searchvalue"].ToString();
                ViewBag.Searchvalue = Searchvalue;
            }
            Doctors _doctor = new Doctors();
            List<Doctors> list = new List<Doctors>();
            object[] obj = _doctor.GetDoctorList(pagenum, Searchvalue);
            list = (List<Doctors>)obj[0];
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

        public ActionResult Create()
        {
            Doctors model = new Doctors();
            DropdownMaster _Master = new DropdownMaster();
            List<SelectListItem> hospitalList = _Master.GetDropdownMaster(1);
            List<SelectListItem> DepartmentList = _Master.GetDropdownMaster(2);
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);

            ViewBag.HospitalList = hospitalList;
            ViewBag.DepartmentList = DepartmentList;
            ViewBag.LocationList = LocationList;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateDoctor(Doctors model, HttpPostedFileBase file)
        {
            string success = null;

            try
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName);
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name.Replace(" ","") + "_" + model.Id + ext;
                    var path = Path.Combine(Server.MapPath("~/Assets/Images/Doctors/"), myfile);
                    file.SaveAs(path);
                    model.Image = myfile;
                }

                var returnbyproc = model.Save();

                if (returnbyproc > 0)
                {
                    success = "Doctor Updated successfully !!!";
                }
                return (RedirectToAction("DoctorList", new { @result = "Success", UserValid = success }));
            }
            catch (Exception ex)
            {
                success = "Some error accurred. Please try later !!!";
                return (RedirectToAction("Create", new { @result = "Success", UserValid = success }));
            }
        }
        public ActionResult Edit(Guid id)
        {
            DropdownMaster _Master = new DropdownMaster();
            List<SelectListItem> hospitalList = _Master.GetDropdownMaster(1);
            List<SelectListItem> DepartmentList = _Master.GetDropdownMaster(2);
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
            

            ViewBag.HospitalList = hospitalList;
            //ViewBag.DepartmentList = DepartmentList;
            //ViewBag.LocationList = LocationList;
            
            Doctors doctor = new Doctors(); 
            doctor = doctor.GetDoctorById(id); 

            return View("~/Views/ManageDoctor/Create.cshtml", doctor);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Doctors dotor)
        {
            try
            {
                var doctorEntity = Mapper.Map<TalkToTreatService.Entities.Doctors>(dotor);
                _doctorService.UpsertDoctor(doctorEntity);
                return RedirectToAction("Edit", new { id = dotor.Id });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAppointment(Appointment model, HttpPostedFileBase file)
        {
            //Service.MasterData.SendEmail sendEmail = new Service.MasterData.SendEmail();
            TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();


            string cclistEmail = string.Empty;

            string to = model.EmailId;
            string subject = model.Subject;
            string body = model.Body;
            string meetUrl = string.Empty;
            string appDate = model.AppointMentDate.ToString("MM/dd/yyyy");


            if (model.Status == (int)BaseApiApp.Entities.Enum.AppointmentStatus.Scheduled)
            {
                appDate = model.AppointMentDate.ToString("MM/dd/yyyy") + " " + model.AppointMentTime;

                GoogleCalendar request = new GoogleCalendar();
                request.Summary = subject;
                request.Description = model.Body;
                //request.Location = "Onlline";
                //request.Start =   model.AppointMentDate ;

                //adjust as per the server time and keep only ist
                request.Start = Convert.ToDateTime(appDate).ToUniversalTime().AddHours(-12).AddMinutes(-30);

                request.End = request.Start.AddMinutes(30);
                request.RequestId = model.Id.ToString();

                List<EventAttendee> attendees = new List<EventAttendee>();
                EventAttendee user = new EventAttendee();
                EventAttendee doctor = new EventAttendee();

                user.Email = model.EmailId;
                doctor.Email = model.DoctorEmail;
                attendees.Add(user);
                attendees.Add(doctor);
                if (!string.IsNullOrEmpty(model.cclist))
                {
                    List<string> clist = new List<string>();
                    foreach (var item in model.cclist.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            EventAttendee cclist = new EventAttendee();
                            cclist.Email = item.Trim();

                            clist.Add(cclist.Email);
                            attendees.Add(cclist);
                        }
                    }
                    model.cclist = string.Join(";", clist);
                    cclistEmail = string.Join(",", clist);
                }
                request.attendees = attendees;


                var creJson = Server.MapPath("~/Assets/Cre/cre.json");
                var tokenJson = Server.MapPath("~/Assets/token.json");

                var rtn = await GoogleCelendarHelper.CreateGoogleCalendar(request, creJson, tokenJson);

                meetUrl = rtn.HangoutLink;
            }

            model.GoogleMeetUrl = meetUrl;
            model.AppointMentDate = Convert.ToDateTime(appDate);

            

            var returnbyproc = model.UpdateAppointment();

            if (model.Status == (int)BaseApiApp.Entities.Enum.AppointmentStatus.Scheduled)
            {                
                var ccemail = !string.IsNullOrEmpty(cclistEmail) ? 
                                cclistEmail +","+ System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString()
                              : System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();

                body = PrepareEmailBody(model);
                sendEmail.Send("", model.EmailId + "," + model.DoctorEmail,
                                   ccemail,
                                   "",
                                   "Your appointment has been scheduled",
                                   body);
            }
            return RedirectToAction("AppointmentList");
        }
        public void SendWhatsAppMessage()
        {
             
                const string accountSid = "specify your ssid here";
                const string authToken = "specify auth token here";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(from: new Twilio.Types.PhoneNumber("whatsapp:+9101204276065"), body: "Good morning user", to: new Twilio.Types.PhoneNumber("whatsapp:+917053007664"));
                Console.WriteLine(message.Sid);
        }
        protected string PrepareEmailBody(Appointment model)
        {
            string name = "";
            string subject, loginid, password;
            string body = "";
            try
            {
                string date = "";

                string to = model.EmailId;
                
                body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/AppoitmentUpdate.html"));               
                body = body.Replace("$$lblEmail$$", model.EmailId);
                body = body.Replace("$$Name$$", model.EmailId);
                body = body.Replace("$$date$$", model.AppointMentDate.ToString());
                body = body.Replace("$$lblMeetURL$$", model.GoogleMeetUrl.ToString());
                body = body.Replace("$$body$$", model.Body);

            }
            catch (Exception es)
            {

            }
            return body;
        }

        public ActionResult AppointmentList()
        {
            DateTime? DateFrom = null;
            DateTime? DateTo = null;
            string DateTo1 = null;
            int pagenum = 0;
            int totalRecords = 0;
            string Searchvalue = "";
            if (Request["pagenum"] != null)
            {
                pagenum = Convert.ToInt32(Request["pagenum"]);
                TempData["pagenum"] = pagenum;
            }
            if (String.IsNullOrEmpty(Request["Searchvalue"]) == false)
            {
                Searchvalue = Request["Searchvalue"].ToString();
                ViewBag.Searchvalue = Searchvalue;
            }
            if (String.IsNullOrEmpty(Request["DateFrom"]) == false)
            {
                DateFrom = Convert.ToDateTime(Request["DateFrom"]);
                ViewBag.DateFrom = DateFrom;
            }
            if (String.IsNullOrEmpty(Request["DateTo"]) == false)
            {
                DateTo = Convert.ToDateTime(Request["DateTo"]);
                DateTo1 = Request["DateTo"].ToString();
                DateTime date = Convert.ToDateTime(DateTo1);
                ViewBag.DateTo = DateTo1;
            }
            Appointment _appointment = new Appointment();
            List<Appointment> list = new List<Appointment>();
            object[] obj = _appointment.GetAppointmentList(pagenum, Searchvalue, DateFrom, DateTo);
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

        public ActionResult ReplytoCustomer(Guid id)
        {

            Appointment _appointment = new Appointment();
            _appointment = _appointment.GetAppointmentDetail(id);
            _appointment.AppointMentTime = _appointment.AppointMentDate.ToString("HH:mm");
            return View("~/Views/ManageDoctor/ReplytoCustomer.cshtml", _appointment);
        }
         
        public ActionResult Delete(Guid id)
        {
            string success = null;
            try
            {
                TalkToTreat.Models.Doctors model = new Models.Doctors();
                model.Id = id;
                // TODO: Add delete logic here
                var returnbyproc = model.Delete();
                if (returnbyproc > 0)
                {
                    success = "Doctor Deleted successfully !!!";
                }
                return (RedirectToAction("DoctorList", new { @result = "Success", UserValid = success }));
            }
            catch
            {
                return (RedirectToAction("DoctorList", new { @result = "Failed", UserValid = success }));
            }
        }

        private CalendarService GetCalendarService(string keyfilepath)
        {
            try
            {
                string[] Scopes = {
   CalendarService.Scope.Calendar,
   CalendarService.Scope.CalendarEvents,
   CalendarService.Scope.CalendarEventsReadonly
  };

                GoogleCredential credential;
                using (var stream = new FileStream(keyfilepath, FileMode.Open, FileAccess.Read))
                {
                    // As we are using admin SDK, we need to still impersonate user who has admin access    
                    //  https://developers.google.com/admin-sdk/directory/v1/guides/delegation    
                    credential = GoogleCredential.FromStream(stream)
                     .CreateScoped(Scopes).CreateWithUser("talktotreatAPI@gmail.com");
                }

                // Create Calendar API service.    
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar Sample",
                });
                return service;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CreateEvent(CalendarService _service)
        {
            Event body = new Event();

            EventAttendee a = new EventAttendee();
            //a.Email = "spidey@avengers.com";
            a.Email = "govind.sharma1271987@gmail.com";

            EventAttendee b = new EventAttendee();
            //b.Email = "ironman@avengers.com";
            b.Email = "govindsharma9875@gmail.com";

            List<EventAttendee> attendes = new List<EventAttendee>();
            attendes.Add(a);
            attendes.Add(b);

            body.Attendees = attendes;

            EventDateTime start = new EventDateTime();
            start.DateTime = Convert.ToDateTime("2019-08-28T09:00:00+0530");
            EventDateTime end = new EventDateTime();
            end.DateTime = Convert.ToDateTime("2019-08-28T11:00:00+0530");
            body.Start = start;
            body.End = end;

            body.Location = "Avengers Mansion";
            body.Summary = "Discussion about new Spidey suit";

            //EventsResource.InsertRequest request = new EventsResource.InsertRequest(_service, body, "nickfury@avengers.com");
            EventsResource.InsertRequest request = new EventsResource.InsertRequest(_service, body, "talktotreatAPI@gmail.com");
            Event response = request.Execute();
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
            _appointment.SaveCancelRequest(Id, CancelRegion, (int)CancelledBY.Admin);

            return Redirect("/ManageDoctor/AppointmentList");
        }

    }
}