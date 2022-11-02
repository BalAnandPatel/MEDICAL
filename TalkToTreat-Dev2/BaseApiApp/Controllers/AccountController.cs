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

namespace BaseApiApp.Controllers
{

    [System.Web.Http.RoutePrefix("api/v1/user/account")]
    public class AccountController : ApiController
    {

        private readonly IUserService _userService;
        //public AccountController(IUserService userService)
        //{
        //    _userService = userService;
        //}


        public AccountController()
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
                var user = _userService.Login(login.EmailId, login.Password, true); 

                if (user == null || user.RecordId == null)
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
                        UserInfo = new LoggedinUser { EmailId = user.Email, UserId = user.RecordId, Name=user.Name }
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
        [System.Web.Http.Route("socialogin")]
        [SwaggerOperation("SocialLogin")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult SocialLogin(SocialLoginModel loginModel)
        {
            var userModel = new UserModel();
            var responseModel = new ResponseModel<AuthToken> { };
            //var newhash = PaswordHash.SHA256(loginModel.Password); 
            try
            {
                string UserName = ""; 
                if (!loginModel.IsPhoneNumber)
                {
                    UserName = loginModel.UserName;  
                }
                else
                {
                    UserName = loginModel.UserName; 
                } 
                 
                var user = _userService.SocialLogin(UserName, loginModel.IsPhoneNumber);

                if (user == null || user.RecordId == null)
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
                        UserInfo = new LoggedinUser { EmailId = user.Email, UserId = user.RecordId, Name = user.Name }
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
        /// Create a new user 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST api/values
        [System.Web.Http.Route("create")]
        [SwaggerOperation("register")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Registration(RegistrationModel customer)
        { 
           
            var response = new ResponseModel<BoolResponse> { };
            var user = new TalkToTreatService.Entities.User
            {
                Email = customer.EmailId,
                Name = customer.FullName,
                Password = customer.Password,
                Mobile = customer.MobileNo,
                MobileNo = customer.MobileNo,
                Phone = customer.MobileNo,
                PhoneNumber = customer.MobileNo,
                PatientAge = customer.PatientAge,
                Remark = customer.Remark,
                Country=customer.Country
            };

            var result = _userService.InsertUser(user);
             var responseModel = new ResponseModel<BoolResponse> { Result = result };
            if (!result.IsValid)
            {
                responseModel.Message = result.Message; 
            }
            return Ok(responseModel);
        }
    }
}
