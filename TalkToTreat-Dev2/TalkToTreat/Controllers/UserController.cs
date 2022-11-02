using TalkToTreatService.Entities;
using TalkToTreatService.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Models;
using System.IO;
using BaseApiApp.Entities.Enum;

namespace TalkToTreat.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();
        }
        // GET: User
        public ActionResult Index()
        {

            


            Guid userId = Guid.Empty;
            if (Session["userdetails"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["userdetails"].ToString();
                details = userdetails.Split('(', '&', '#', '$', ')');
                userId = Guid.Parse(details[0]);
                TalkToTreatService.Entities.User user = _userService.GetUserByGuid(userId);
                TalkToTreat.Models.User userModel = new Models.User
                {
                    MobileNo = user.PhoneNumber,
                    Phone = user.Phone,
                    Email = user.Email,
                    PatientAge = user.PatientAge,
                    Name = user.Name,
                    Remark = user.Remark
                };
                return View(userModel);
            }
            else { return Redirect("/Home/Index"); }

        }
        public ActionResult AppointmentList()
        {
            //var i = 100 / 100;
            //var j = 0;
            //var k = i / j;


            if (Session["userdetails"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["userdetails"].ToString();
                details = userdetails.Split('(', '&', '#', '$', ')');

                string username = details[5];

                int pagenum = 0;
                int totalRecords = 0;
                string Searchvalue = "";
                if (Request["pagenum"] != null)
                {
                    pagenum = Convert.ToInt32(Request["pagenum"]);
                    TempData["pagenum"] = pagenum;
                }
                Searchvalue = username;
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                object[] obj = _appointment.GetAppointmentDetailByUser(pagenum, Searchvalue);
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
            if (Session["userdetails"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["userdetails"].ToString();
                details = userdetails.Split('(', '&', '#', '$', ')');

                string username = details[5];

                int pagenum = 0;
                int totalRecords = 0;
                string Searchvalue = "";
                if (Request["pagenum"] != null)
                {
                    pagenum = Convert.ToInt32(Request["pagenum"]);
                    TempData["pagenum"] = pagenum;
                }
                Searchvalue = username;
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                object[] obj = _appointment.GetAppointmentDetailByUserFuture(pagenum, Searchvalue);
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

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Healthwallet()
        {
            Guid userId = Guid.Empty;
            if (Session["userdetails"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["userdetails"].ToString();
                details = userdetails.Split('(', '&', '#', '$', ')');
                userId = Guid.Parse(details[0]);
                List<TalkToTreatService.Entities.UserVallet> uservaletsList = _userService.GetUserValletByGuid(userId);

                TalkToTreatService.Entities.User user = _userService.GetUserByGuid(userId);


                List<TalkToTreat.Models.UserVallet> uservalets = new List<Models.UserVallet>();
                ViewBag.UserHealthCondition = user.Remark;
                foreach (var item in uservaletsList)
                {
                    TalkToTreat.Models.UserVallet UserVallet = new Models.UserVallet
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Description = item.Description,
                        Id = item.Id,
                        Type = item.Type,
                    };
                    uservalets.Add(UserVallet);
                }
                ViewBag.UserValletList = uservalets;
                return View(new TalkToTreat.Models.UserVallet());
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult AddDocument(TalkToTreat.Models.UserVallet model, HttpPostedFileBase file)
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
                    if (file != null)
                    {
                        model.Id = Guid.NewGuid();
                        var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                        var ext = Path.GetExtension(file.FileName);
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = name + "_" + model.Id + ext;
                        var path = Path.Combine(Server.MapPath("~/Assets/Images/UserDocuments/"), myfile);
                        file.SaveAs(path);
                        model.FileUrl = myfile;
                    }
                    var returnbyproc = model.Save();
                    if (returnbyproc > 0)
                    {
                        success = "File Added successfully !!!";
                    }
                }
                TempData["HealthwalletMessage"] = success;
                TempData.Keep();
                return (RedirectToAction("Healthwallet", new { @result = "Success", UserValid = success }));
            }
            catch (Exception ex)
            {
                success = ex.Message;
                TempData["HealthwalletMessage"] = success;
                TempData.Keep();
                return (RedirectToAction("Healthwallet", new { @result = "Success", UserValid = success }));
            }
        }
        public ActionResult Delete(Guid id)
        {
            string success = null;
            try
            {
                TalkToTreat.Models.UserVallet model = new Models.UserVallet();
                model.Id = id;
                // TODO: Add delete logic here
                var returnbyproc = model.Delete();
                if (returnbyproc > 0)
                {
                    success = "File Deleted successfully !!!";
                }
                return (RedirectToAction("Healthwallet", new { @result = "Success", UserValid = success }));
            }
            catch
            {
                return (RedirectToAction("Healthwallet", new { @result = "Failed", UserValid = success }));
            }
        }

        [HttpPost]
        public ActionResult UpdateProfile(TalkToTreat.Models.User model)
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
                return (RedirectToAction("Index", new { @result = "Success", UserValid = success }));
            }
            catch (Exception ex)
            {
                success = "Some error accurred. Please try later !!!";
                return (RedirectToAction("Index", new { @result = "Success", UserValid = success }));
            }
        }

        [HttpPost]
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

            //try
            //{
                //email to user and copy to admin
                TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                sendEmail.Send("", appDetails.EmailId, System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString(), "", "Your appointment requested to cancel", body);
            //}
            //catch (Exception)
            //{

            //}

            //save in user case
            _appointment.SaveCancelRequest(Id, CancelRegion, (int)CancelledBY.User);


            return Redirect("/User/AppointmentList");
        }

        public ActionResult viewResponse(string id)
        {
           
            Appointment _appointment = new Appointment();
            _appointment = _appointment.GetAppointmentDetail(Guid.Parse(id));          
            return View(_appointment);
        }

    }
}
