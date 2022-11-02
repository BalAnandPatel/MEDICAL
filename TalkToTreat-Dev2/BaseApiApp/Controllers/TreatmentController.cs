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
using Microsoft.Practices.Unity;
using Unity.AspNet.Mvc;
using BaseApiApp.Framework;
using BaseApiApp.Framework.Utils;

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/treatment")]
    public class TreatmentController : ApiController
    {
        private readonly IUserService _userService;
        MasterService _masterService = new MasterService();
        //public AccountController(IUserService userService)
        //{
        //    _userService = userService;
        //}


        public TreatmentController()
        {
            var container = new UnityContainer();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IWebUtils, WebUtils>();

            _userService = DependencyResolver.Current.GetService<IUserService>();

        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("alltreatment")]
        [SwaggerOperation("AllTreatment")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllTreatment()
        {
            List<Treatment> treatments = new List<Treatment>();
            treatments = _masterService.GetTreatments();
            var responseModel = new ResponseModel<List<Treatment>> { Result = treatments }; 
            return Ok(responseModel);
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("sendenquiry")]
        [SwaggerOperation("SendEnquiry")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult SendEnquiry(EnquiryModel userInformation)
        {
            var response = new ResponseModel<BoolResponse> { };
            var user = new TalkToTreatService.Entities.User
            {
                Email = userInformation.EmailId,
                Name = userInformation.FullName, 
                PatientAge = userInformation.PatientAge,
                Remark = userInformation.Remark,
                MobileNo = userInformation.MobileNo,
                Mobile = userInformation.MobileNo,
                PhoneNumber = userInformation.MobileNo,
                AppointMentDate =DateTime.Now
            };

            var result = _userService.InsertEnquiry(user);
            if (!result.IsValid) return BadRequest(result.MessageCode + ":" + result.Message);
            var responseModel = new ResponseModel<BoolResponse> { Result = result };
            responseModel.Message = "Enquiry Sent Successfully !";
            return Ok(responseModel);
        }
    }
}