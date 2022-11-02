using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Filters;
using TalkToTreat.Models;

namespace TalkToTreat.Controllers
{
    [AdminUserAuthenticationFilter]
    public class AdminHomeController : Controller
    {
        private readonly IDoctorsService _doctorservice;
        public AdminHomeController()
        {
            _doctorservice = DependencyResolver.Current.GetService<IDoctorsService>();

        }
        public ActionResult Index()
        {
            Guid employee_id;
            int? pagenum = null;
            if (Session["adminuserdetails"] != null)
            {
                string userdetails = null;
                string[] details = null;
                userdetails = Session["adminuserdetails"].ToString();
                details = userdetails.Split('(', '@', '#', '$', ')');
                employee_id = Guid.Parse(details[0]);
            }
            Home _Home = new Home();
             object[] obj = _Home.getdata(pagenum);
            Appointment _appointment = new Appointment();
            object[] Listobj = _appointment.GetAppointmentList(1, "", null, null);
            _Home = (Home)obj[0];
            _Home.Appointments = (List<Appointment>)Listobj[0];
            return View(_Home);
        }
    }
}