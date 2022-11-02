using Service.MasterData;
using AutoMapper;
using BaseApiApp.Entities.Account;
using BaseApiApp.Models;
using BaseApiApp.Models.Account;
using TalkToTreatService.Entities;
using TalkToTreatService.Users;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Collections.Generic; 

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/services")]
    public class ServicesController : ApiController
    {
         MasterService _masterService= new MasterService();
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("allservices")]
        [SwaggerOperation("AllServices")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllServices()
        {
            List<TalkToTreatService.Entities.Service> locations = new List<TalkToTreatService.Entities.Service>();
            locations = _masterService.GetServices();
            var responseModel = new ResponseModel<List<TalkToTreatService.Entities.Service>> { Result = locations }; 
            return Ok(responseModel);
        }
    }
}