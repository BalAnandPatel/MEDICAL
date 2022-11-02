using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using TalkToTreat.Models;
using System.Web.Mvc; 
using TalkToTreatService.Users;
using AutoMapper;

namespace TalkToTreat.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly IUserService _userService;
        public HospitalsController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();

        }
        DoctorsService _doctorService = new DoctorsService();
        MasterService _masterService = new MasterService();
        HospitalService _hospitalService = new HospitalService();
        public ActionResult Index(string country, string city)
        {

            ViewBag.SelectedCountry = country;
            ViewBag.SelectedCity = city;

            List<TalkToTreat.Models.Treatment> treatments = new List<TalkToTreat.Models.Treatment>();
            var treatmentsList = _masterService.GetTreatments();
            foreach (var item in treatmentsList)
            {
                TalkToTreat.Models.Treatment treatment = new TalkToTreat.Models.Treatment();
                treatment.Code = item.Code;
                treatment.Cost = item.Cost;
                treatment.Name = item.Name; 
                treatment.Id = item.Id;
                treatment.Name = item.Name;
                treatment.Id = item.Id;
                treatments.Add(treatment);
            }
            ViewBag.TreatmentsList = treatments;


            var AllCities = _masterService.GetLocations().OrderBy(a=>a.Name);
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
                AllCitiesEntity.Add(loc);
            }
            ViewBag.AllCities = AllCitiesEntity;


            //List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
            //var HospialList = _hospitalService.GetAllHospitals().OrderBy(a => a.Name);
            //foreach (var item in HospialList)
            //{
            //    TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
            //    hospital.Code = item.Code;
            //    hospital.Name = item.Name;
            //    hospital.Id = item.Id;
            //    hospitals.Add(hospital);
            //}
            //ViewBag.Hospitals = hospitals;


            List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
            var HospialList = _hospitalService.GetAllHospitals();
            if (HospialList != null && HospialList.Count() > 0)
            {
                if (!string.IsNullOrEmpty(city))
                {
                    HospialList = HospialList.Where(a => a.City.ToLower() == city.ToLower()).ToList();
                } 
            }
            foreach (var item in HospialList)
            {
                TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
                hospital.Code = item.Code;
                hospital.Name = item.Name;
                hospital.Id = item.Id;
                hospital.Type = item.Type;
                hospital.ShortDescription = item.ShortDescription;
                hospital.Description = item.Description;
                hospital.EstablishmentYesr = item.EstablishmentYesr;
                hospital.Image = item.Image;
                hospital.IsActive = item.IsActive;
                hospital.NumberOfbeds = item.NumberOfbeds;

                hospital.PostCode = item.PostCode;

                hospital.Address = item.Address;
                hospital.City = item.City;
                hospital.State = item.State;
                hospital.Country = item.Country;

                hospitals.Add(hospital);
            }
            return View(hospitals); 
        }
        public ActionResult Detail(string id)
        {
            Guid hospitalId = Guid.Parse(id);

            List<TalkToTreat.Models.Treatment> treatments = new List<TalkToTreat.Models.Treatment>();
            var treatmentsList = _masterService.GetTreatments();
            foreach (var item in treatmentsList)
            {
                TalkToTreat.Models.Treatment treatment = new TalkToTreat.Models.Treatment();
                treatment.Code = item.Code;
                treatment.Cost = item.Cost;
                treatment.Name = item.Name;
                treatment.Id = item.Id;
                treatment.Name = item.Name;
                treatment.Id = item.Id;
                treatments.Add(treatment);
            }
            ViewBag.TreatmentsList = treatments;

           

            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();
            doctorsList = doctorsList.Where(a => a.HospitalID == hospitalId).ToList();
            foreach (var item in doctorsList)
            {
                Doctors doc = new Doctors();
                doc.Code = item.Code;
                doc.Name = item.Name;
                doc.Department = item.Department; 
                doc.Image = item.Image;
                doc.Hospital = item.Hospital;
                doc.City = item.City;
                doc.Id = item.Id;
                doc.Cost = item.Cost; 
                doctors.Add(doc);
            }
            ViewBag.Doctors = doctors;

             
            var HospialList = _hospitalService.GetAllHospitals();

            var hospitalDetail = HospialList.Where(a => a.Id == hospitalId).FirstOrDefault();
            TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
            hospital.Code = hospitalDetail.Code;
            hospital.Name = hospitalDetail.Name;
            hospital.Id = hospitalDetail.Id;
            hospital.Type = hospitalDetail.Type;
            hospital.ShortDescription = hospitalDetail.ShortDescription;
            hospital.Description = hospitalDetail.Description;
            hospital.EstablishmentYesr = hospitalDetail.EstablishmentYesr;
            hospital.Image = hospitalDetail.Image;
            hospital.IsActive = hospitalDetail.IsActive;
            hospital.NumberOfbeds = hospitalDetail.NumberOfbeds;

            hospital.PostCode = hospitalDetail.PostCode;

            hospital.Address = hospitalDetail.Address;
            hospital.City = hospitalDetail.City;
            hospital.State = hospitalDetail.State;
            hospital.Country = hospitalDetail.Country;



            return View(hospital);
        }
        public ActionResult ConsultOnline(string id)
        {
            Guid treatmentId = Guid.Empty;
            try
            {
                 treatmentId = Guid.Parse(id);
            } catch {

                RedirectToAction("Registration", "Home");
            }
            List<Doctors> doctors = new List<Doctors>();
            var doctorsList = _doctorService.GetAllDoctors();

            var doctorDetail = doctorsList.Where(a => a.Id == treatmentId).FirstOrDefault();
            Doctors doctor = new Doctors();
            doctor.Code = doctorDetail.Code;
            doctor.Cost = doctorDetail.Cost;
            doctor.Name = doctorDetail.Name;
            doctor.Department = doctorDetail.Department;
            doctor.Designation = doctorDetail.Designation;
            doctor.Description = doctorDetail.Description;
            doctor.Image = doctorDetail.Image;
            doctor.Id = doctorDetail.Id;
            return View(doctor);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("BookAppointment")]
        public ActionResult BookAppointment(Appointment _registration)
        {
            if (String.IsNullOrEmpty(Request["DoctorId"]) == false)
            {

                _registration.DoctorId =Guid.Parse(Request["DoctorId"].ToString());
            }
            string invalid = null;
            try
            {
                var user = new TalkToTreatService.Entities.User
                {
                    Email = _registration.EmailId,
                    Name = _registration.FullName,
                    Password = _registration.Password,
                    Mobile = _registration.MobileNo,
                    PatientAge = _registration.PatientAge,
                    Gender = _registration.Gender,
                    AppointMentType = _registration.AppointMentType,
                    AppointMentDate = _registration.AppointMentDate,
                    DoctorId=_registration.DoctorId
                };

                var resp = _userService.InsertEnquiry(user);
                if (resp.IsValid)
                {
                    return (RedirectToAction("Confirmation", "Home"));
                }
            }
            catch (Exception ex)
            {
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("index", "Home", new { UserValid = invalid }));
            }
            return View();
        }

        [HttpPost]
        public ActionResult Filter(int? country, int? city, Guid? treat, Guid? hosp, bool? bestHospital)
        {           
            List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
            var HospialList = _hospitalService.GetAllHospitals();
            if (HospialList != null && HospialList.Count() > 0)
            {
                if ((country ?? 0) != 0)
                {
                    HospialList = HospialList.Where(a => a.CountryId == country).ToList();
                }
                if ((city ?? 0) != 0)
                {
                    HospialList = HospialList.Where(a => a.CityId == city).ToList();
                }
                if ((treat ?? Guid.Empty) != Guid.Empty)
                {
                    HospialList = HospialList.Where(a => a.TreatmentId == treat).ToList();
                }
                if ((hosp ?? Guid.Empty) != Guid.Empty)
                {
                    HospialList = HospialList.Where(a => a.Id == hosp).ToList();
                }
                if (bestHospital ?? false)
                {
                    //add logic for best doctor
                }
            }
            foreach (var item in HospialList)
            {
                TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
                hospital.Code = item.Code;
                hospital.Name = item.Name;
                hospital.Id = item.Id;
                hospital.Type = item.Type;
                hospital.ShortDescription = item.ShortDescription;
                hospital.Description = item.Description;
                hospital.EstablishmentYesr = item.EstablishmentYesr;
                hospital.Image = item.Image;
                hospital.IsActive = item.IsActive;
                hospital.NumberOfbeds = item.NumberOfbeds;
                hospital.PostCode = item.PostCode;

                hospital.Address = item.Address;
                hospital.City = item.City;
                hospital.State = item.State;
                hospital.Country = item.Country;

                hospitals.Add(hospital);
            }
            return PartialView("~/Views/Hospitals/_HospitalListPartial.cshtml", hospitals);
        }
    }
}
