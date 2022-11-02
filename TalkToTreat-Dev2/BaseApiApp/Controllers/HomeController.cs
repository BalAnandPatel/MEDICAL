using Service.MasterData;
using BaseApiApp.Models;
using TalkToTreatService.Entities;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using BaseApiApp.Framework;
using BaseApiApp.Framework.Utils;
using TalkToTreatService.Users;
using BaseApiApp.Entities.Account;
using BaseApiApp.Models.Account;
using System.Linq;

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/Home")]
    public class HomeController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IDoctorsService _doctorsService;
        private readonly IHospitalService _hospitalService;
        MasterService _masterService = new MasterService();
        public HomeController()
        {
            var container = new UnityContainer();
            container.RegisterType<IDoctorsService, DoctorsService>();
            container.RegisterType<IWebUtils, WebUtils>(); 
            container.RegisterType<IUserService, UserService>(); 

            _userService = DependencyResolver.Current.GetService<IUserService>();
            _hospitalService = DependencyResolver.Current.GetService<IHospitalService>(); 
            _doctorsService = DependencyResolver.Current.GetService<IDoctorsService>();

        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("homepagedata")]
        [SwaggerOperation("HomePageData")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult HomePageData(string country, string city)
        {
            Home _home = new Home();
            var hospital = _hospitalService.GetAllHospitals();
            _home.Hospital = new List<HospitalModel>();
            _home.Treatment = new List<TreatmentModel>();
            foreach (var hosp in hospital?.Where(a => a.City.ToLower() == city.ToLower()).ToList())
            {
                var HospitalModel = new HospitalModel();
                HospitalModel.Id = hosp.Id;
                HospitalModel.Name = hosp.Name;
                HospitalModel.City = hosp.City;
                HospitalModel.Image = hosp.Image;
                HospitalModel.Description = hosp.Description;
                HospitalModel.ShortDescription = hosp.ShortDescription;
                _home.Hospital.Add(HospitalModel);
            }
            _home.Hospital = _home.Hospital.Take(5).ToList();
            var Doctors = _doctorsService.GetAllDoctors(); 
            foreach (var treat in _masterService.GetTreatments().Where(a => a.Image != null && a.Image != ""))
            {
                var TreatmentModel = new TreatmentModel();
                TreatmentModel.Id = treat.Id;
                TreatmentModel.Name = treat.Name;
                TreatmentModel.Cost = treat.Cost;
                TreatmentModel.Image = treat.Image;
                TreatmentModel.Description = treat.Description;
                TreatmentModel.Code = treat.Code;
                _home.Treatment.Add(TreatmentModel);
            }
            _home.Treatment = _home.Treatment.Take(5).ToList();
            var selectedDoctors =
            from Hospital in _home.Hospital
            join Doctor in Doctors on Hospital.Name equals Doctor.Hospital
            where Hospital.Name == Doctor.Hospital
            select new DoctorModel { Id = Doctor.Id, Department = Doctor.Department, Designation = Doctor.Designation, Image = Doctor.Image, Name = Doctor.Name };
            _home.Doctors = selectedDoctors.Take(5).ToList();
            _home.services = new List<ServiceModel>();
           var services = _masterService.GetServices();
            foreach (var service in services)
            {
                var serviceModel = new ServiceModel();
                serviceModel.Id = service.Id;
                serviceModel.Name = service.Name;
                serviceModel.Code = service.Code;
                serviceModel.IsActive = service.IsActive;
                serviceModel.Description = service.Description;
                serviceModel.ShortDescription = service.ShortDescription;
                _home.services.Add(serviceModel);
            }
            //_home.services = _home.services.Take(5).ToList();

            var responseModel = new ResponseModel<Home> { Result = _home }; 
            return Ok(responseModel);
        } 
    }
}