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
using System.Linq;
using System;

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/Hospital")]
    public class HospitalController : ApiController
    {
        private readonly IHospitalService _hospitalService;
        MasterService _masterService = new MasterService();
        public HospitalController()
        {
            var container = new UnityContainer();
            container.RegisterType<IHospitalService, HospitalService>();
            container.RegisterType<IWebUtils, WebUtils>();

            _hospitalService = DependencyResolver.Current.GetService<IHospitalService>();

        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("allhospital")]
        [SwaggerOperation("AllHospital")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllHospital()
        {
            List<Hospital> hospitals = new List<Hospital>();
            hospitals = _hospitalService.GetAllHospitals();
            var responseModel = new ResponseModel<List<Hospital>> { Result = hospitals }; 
            return Ok(responseModel);
        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("HospitalDetail")]
        [SwaggerOperation("HospitalDetail")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult HospitalDetail(string ID)
        {
            Guid hospialId = Guid.Parse(ID);
            Hospital hospital = new Hospital();
            var hospitals = _hospitalService.GetAllHospitals();
            hospital= hospitals.Where(a => a.Id == hospialId).FirstOrDefault();
            var responseModel = new ResponseModel<Hospital> { Result = hospital };
            return Ok(responseModel);
        }
    }
}