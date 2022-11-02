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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace BaseApiApp.Controllers
{

    [System.Web.Http.RoutePrefix("api/v1/customer")]
    public class CustomerController : ApiController
    {

        private readonly IUserService _userService;
        //public AccountController(IUserService userService)
        //{
        //    _userService = userService;
        //} 

        public CustomerController()
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
        [System.Web.Http.Route("profile")]
        [SwaggerOperation("Profile")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult GetProfile(string id)
        {
            Guid userId = Guid.Empty;
            var responseModel = new ResponseModel<UserProfileModel> { };
            try
            {
                userId = Guid.Parse(id);
                TalkToTreatService.Entities.User user = _userService.GetUserByGuid(userId);
                UserProfileModel userModel = new UserProfileModel
                {
                    MobileNo = user.MobileNo,
                    EmailId = user.Email,
                    PatientAge = user.PatientAge,
                    FullName = user.Name,
                    AboutPatient = user.Remark,
                    Remark = user.Remark,
                    Country = user.Country,
                    ProfileImage=user.Image
                };
                responseModel.Result = userModel;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel);
            }
            responseModel.StatusCode = HttpStatusCode.OK;
            return Ok(responseModel);
        }
        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/values/5
        [System.Web.Http.Route("{id}/update")]
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult UpdateProfile(string id, UserProfileModel customer)
        {
            Guid userId = Guid.Empty;
            userId = Guid.Parse(id);
            var response = new ResponseModel<BoolResponse> { };
            var user = new TalkToTreatService.Entities.User
            {
                UserId = userId,
                Email = customer.EmailId,
                Name = customer.FullName, 
                PatientAge = customer.PatientAge,
                Remark = customer.Remark,
                MobileNo = customer.MobileNo,  
                Country = customer.Country,
                Image=customer.ProfileImage
            };

            var result = _userService.UpdateUserProfile(user);
            var responseModel = new ResponseModel<BoolResponse> { Result = result };
            if (!result.IsValid)
            {
                responseModel.Message = result.Message;
            }
            return Ok(responseModel);
        }


        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT api/values/5
        [System.Web.Http.Route("{id}/updatemedicalcondition")]
        [SwaggerOperation("UpdateMedicalCondition")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult UpdateMedicalCondition(string id, string MedicalCondition)
        {
            Guid userId = Guid.Empty;
            userId = Guid.Parse(id);
            var response = new ResponseModel<BoolResponse> { };
            var user = new TalkToTreatService.Entities.User
            {
                UserId = userId, 
                Remark = MedicalCondition, 
            };

            var result = _userService.UpdateUserProfile(user);
            var responseModel = new ResponseModel<BoolResponse> { Result = result };
            if (!result.IsValid)
            {
                responseModel.Message = result.Message;
            }
            return Ok(responseModel);
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
        public IHttpActionResult AppointmentList(int PageNumber, string EmailId)
        {
            Guid userId = Guid.Empty;
            var responseModel = new ResponseModel<List<Appointment>> { };
            try
            {
                Appointment _appointment = new Appointment();
                List<Appointment> list = new List<Appointment>();
                list = _userService.GetAppointmentListByUser(PageNumber, EmailId);
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

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("appointmentdetail")]
        [SwaggerOperation("AppointmentDetail")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AppointmentDetail(string Id)
        {
            Guid RecordId = Guid.Parse(Id);
            var responseModel = new ResponseModel<Appointment> { };
            try
            {
                Appointment _appointment = new Appointment();
                _appointment = _userService.GetAppointmentDetail(RecordId);
                responseModel.Result = _appointment;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel);
            }
            responseModel.StatusCode = HttpStatusCode.OK;
            return Ok(responseModel);
        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("getuservallet")]
        [SwaggerOperation("GetUserVallet")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult GetUserVallet( string ID)
        {
            Guid userId = Guid.Parse(ID);
            var responseModel = new ResponseModel<List<UserValletModel>> { };
            try
            {
                UserValletModel _appointment = new UserValletModel();
                List<UserValletModel> list = new List<UserValletModel>();
                var vallets = _userService.GetUserValletByGuid(userId);
           

                foreach (var item in vallets)
                {
                    UserValletModel UserVallet = new UserValletModel
                    {
                        FileName = item.FileName,
                        DownloadURL = "httpss://talktotreat.com/Assets/Images/UserDocuments/"+item.FileUrl,
                        Description = item.Description,
                        Id = item.Id,
                        Type = item.Type,
                    };
                    list.Add(UserVallet);
                }
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


        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("adddocument")]
        [SwaggerOperation("AddDocument")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AddDocument(string id, UserValletModel model)
        {
            var responseModel = new ResponseModel<BoolResponse> { };
            try
            { 
                if (model.FileUrl != null && model.FileUrl.Length > 0)
                {

                }
                else
                {
                    model.FileUrl = Encoding.ASCII.GetBytes(model.DownloadURL);
                } 
                FtpWebRequest request;

                model.FileName = model.FileName.Replace(" ","");
                string absoluteFileName = Path.GetFileName(model.FileName); 
                request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", "talktotreat.com", "talktotreat.com/Assets/Images/UserDocuments", absoluteFileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential("rooveaco", "9OpAz194nn@");
                request.ConnectionGroupName = "group";

               
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(model.FileUrl, 0, model.FileUrl.Length);
                requestStream.Flush();
                requestStream.Close();  
                 

                //model.Id = Guid.NewGuid();
                //var fileName = Path.GetFileName(model.FileName); //getting only file name(ex-ganesh.jpg)  
                //var ext = Path.GetExtension(model.FileName);
                //string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                //string myfile = name + "_" + model.Id + ext; 
                //var path = Path.Combine("https://talktotreat.com/Assets/Images/UserDocuments/", myfile);
                //// File.WriteAllBytes(path, model.FileUrl.ToArray());

                //WebClient wc = new WebClient();   
                //wc.UploadDataAsync(new Uri(path),  null, model.FileUrl.ToArray(), null);


                UserVallet valled = new UserVallet
                {   Id = Guid.NewGuid(),
                    Description = model.Description,
                    FileName = model.FileName,
                    FileUrl = model.FileName
                };
                var result = _userService.AddDocument(Guid.Parse(id), valled);
                responseModel.Result = result;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel);
            }
            responseModel.StatusCode = HttpStatusCode.OK;
            return Ok(responseModel);
        } 
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("removedocument")]
        [SwaggerOperation("RemoveDocument")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult RemoveDocument(string UserId, string FileId)
        {
            var responseModel = new ResponseModel<BoolResponse> { }; 
            try
            { 
                UserVallet valled = new UserVallet
                {
                    Id = Guid.Parse(FileId) 
                };
                var result = _userService.DeleteDocument(valled);
                responseModel.Result = result;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = HttpStatusCode.InternalServerError;
                return Ok(responseModel);
            }
            responseModel.StatusCode = HttpStatusCode.OK;
            return Ok(responseModel);
        }
    }
}
