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
using Microsoft.Practices.Unity;
using Unity.AspNet.Mvc;
using BaseApiApp.Framework;
using BaseApiApp.Framework.Utils;
using Service.MasterData;
using System.Collections.Generic;

namespace BaseApiApp.Controllers
{

    [System.Web.Http.RoutePrefix("api/v1/doctor/doctoraccount")]
    public class DoctorAccountController : ApiController
    {

        private readonly IUserService _userService;
        //public AccountController(IUserService userService)
        //{
        //    _userService = userService;
        //}
         
        private readonly IDoctorsService _doctorsService;
        MasterService _masterService = new MasterService();
      
        public DoctorAccountController()
        {
            var container = new UnityContainer();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IWebUtils, WebUtils>();

            _userService = DependencyResolver.Current.GetService<IUserService>();
            _doctorsService = DependencyResolver.Current.GetService<IDoctorsService>();

        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("authenticate")]
        [SwaggerOperation("Authenticate")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            var userModel = new UserModel();
            var responseModel = new ResponseModel<AuthToken> { };
            //var newhash = PaswordHash.SHA256(loginModel.Password);
            var newhash = loginModel.Password;
            try
            {
                var login = new Login()
                {
                    EmailId = loginModel.EmailId,
                    Password = newhash
                }; 
                var user = _doctorsService.Login(login.EmailId, login.Password, true); 

                if (user == null || user.Id == null || user.Id == Guid.Empty)
                {
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    responseModel.Message = "Incorrect username or Password !";
                }
                else
                {
                    responseModel.StatusCode = HttpStatusCode.OK;
                    responseModel.Message = "loggedIn successfully";
                    // HttpContext.Session["currentUser"] = user;  
                    var token = Guid.NewGuid().ToString();
                    var authToken = new AuthToken { Status = TokenStatus.Unauthorized };
                    authToken = new AuthToken()
                    {
                        Token = token,
                        Expiration = DateTime.UtcNow.AddDays(7),
                        Status = TokenStatus.Valid,
                        UserInfo = new LoggedinUser { EmailId = user.UserName, Name = user.Name, UserId = user.Id }
                    };
                    responseModel.Result = authToken;
                }
                return Ok(responseModel);
            }
            catch
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel); 
            }
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("appointmentList")]
        [SwaggerOperation("AppointmentList")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AppointmentList(int PageNumber, string DoctorId )
        {
            Guid userId = Guid.Empty;
            var responseModel = new ResponseModel<List<Appointment>> { };
            try
            {
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                list = _userService.GetAppointmentByDoctor(PageNumber,  "",Guid.Parse(DoctorId));
                responseModel.Result = list;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel);
            }
            responseModel.StatusCode = HttpStatusCode.OK;
            return Ok(responseModel);
        }

        [System.Web.Http.Route("{id}/updateappointment")]
        [SwaggerOperation("UpdateAppointment")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult UpdateAppointment(string doctorId, Appointment appointment)
        {
            Guid docId =  Guid.Parse(doctorId);
            appointment.DoctorId = docId;

            var result = _doctorsService.UpdateAppointment(appointment);
            var responseModel = new ResponseModel<BoolResponse> { Result = result };
            if (!result.IsValid)
            {
                responseModel.Message = result.Message;
            }
            return Ok(responseModel);
        }

    }
}
