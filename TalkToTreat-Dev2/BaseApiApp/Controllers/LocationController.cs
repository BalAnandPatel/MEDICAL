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
using System.Linq;

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/location")]
    public class LocationController : ApiController
    {
         MasterService _masterService= new MasterService();
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("alllocation")]
        [SwaggerOperation("AllLocation")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllLocation()
        {
            List<Locations> locations = new List<Locations>();
            locations = _masterService.GetLocations();
            var responseModel = new ResponseModel<List<Locations>> { Result = locations }; 
            return Ok(responseModel);
        }

        [System.Web.Http.Route("allcountry")]
        [SwaggerOperation("AllCountry")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllActiveCountry()
        {
            List<Country> locations = new List<Country>();
            var Alllocations = _masterService.GetLocations();
            var responseModel = new ResponseModel<List<Country>> { Result = locations };
            return Ok(responseModel);
        }
        [System.Web.Http.Route("allactivecity")]
        [SwaggerOperation("AllActiveCity")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllActiveCity(string country)
        {
            List<City> locations = new List<City>();
            var Alllocations =   _masterService.GetLocations();
            if (!string.IsNullOrEmpty(country))
            {
                Alllocations = Alllocations.Where(a => a.CountryName.ToLower() == country.ToLower()).ToList();
            }
            var responseModel = new ResponseModel<List<City>> { Result = locations };
            return Ok(responseModel);
        }
    }
}