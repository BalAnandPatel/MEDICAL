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

namespace TalkToTreat.Controllers
{
    [AdminUserAuthenticationFilter]
    public class ManageHospitalController : Controller
    {
       
        HospitalService _hospitalService = new HospitalService();
        public ManageHospitalController()
        { 
        } 
        public ActionResult HospitalList()
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
            Hospital _hospital = new Hospital();
            List<Hospital> list = new List<Hospital>();
            object[] obj = _hospital.GetHospitalList(pagenum, Searchvalue);
            list = (List<Hospital>)obj[0];
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
            Hospital model = new Hospital();
            DropdownMaster _Master = new DropdownMaster(); 
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
             
            ViewBag.LocationList = LocationList;

            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateHospital(Hospital model, HttpPostedFileBase file)
        {
            string success = null;
           
            try
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName);
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + model.Id + ext;
                    var path = Path.Combine(Server.MapPath("~/Assets/Images/hospitals/"), myfile);
                    file.SaveAs(path);
                    model.Image = myfile;
                }
                var returnbyproc=model.Save();  
                if (returnbyproc > 0)
                {
                    if (model.Id == Guid.Empty)
                    {
                        success = "Hospital created successfully !!!";
                    }
                    else
                    {
                        success = "Hospital Updated successfully !!!";
                    }
                }
                return (RedirectToAction("HospitalList", new { @result = "Success", UserValid = success }));
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
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
             
            ViewBag.LocationList = LocationList;
            List<Hospital> list = new List<Hospital>();
            var hospitalList = _hospitalService.GetAllHospitals();

            var doctorDetail = hospitalList.Where(a => a.Id == id).FirstOrDefault();
            Hospital hospital = new Hospital();
            hospital.Code = doctorDetail.Code;
            hospital.Cost = doctorDetail.Cost;
            hospital.Description = doctorDetail.Description;
            hospital.City = doctorDetail.City;
            hospital.Country = doctorDetail.Country;
            hospital.ShortDescription = doctorDetail.ShortDescription;
            hospital.Name = doctorDetail.Name;
            hospital.PhoneNumber = doctorDetail.PhoneNumber; 
            hospital.Description = doctorDetail.Description;
            hospital.Image = doctorDetail.Image;
            hospital.IsActive = doctorDetail.IsActive;


            hospital.NumberOfbeds = doctorDetail.NumberOfbeds;
            hospital.Type = doctorDetail.Type;
            hospital.EstablishmentYesr = doctorDetail.EstablishmentYesr;
            hospital.Spacialities = doctorDetail.Spacialities;
            hospital.IconImage = doctorDetail.IconImage;
            hospital.Infrastructure = doctorDetail.Infrastructure;
            hospital.CustomNo = doctorDetail.CustomNo;
            hospital.Id = doctorDetail.Id; 
            return View("~/Views/ManageHospital/Create.cshtml", hospital);
        }

        public ActionResult Delete(Guid id)
        {
            string success = null;
            try
            {
                TalkToTreat.Models.Hospital model = new Models.Hospital();
                model.Id = id;
                // TODO: Add delete logic here
                var returnbyproc = model.Delete();
                if (returnbyproc > 0)
                {
                    success = "Hospital Deleted successfully !!!";
                }
                return (RedirectToAction("HospitalList", new { @result = "Success", UserValid = success }));
            }
            catch
            {
                return (RedirectToAction("HospitalList", new { @result = "Failed", UserValid = success }));
            }
        }
    }
}