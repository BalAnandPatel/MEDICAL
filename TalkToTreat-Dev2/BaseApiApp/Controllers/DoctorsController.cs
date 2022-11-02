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
using System;

namespace BaseApiApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/Doctors")]
    public class DoctorsController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IDoctorsService _doctorsService;
        MasterService _masterService = new MasterService();
        public DoctorsController()
        {
            var container = new UnityContainer();
            container.RegisterType<IDoctorsService, DoctorsService>();
            container.RegisterType<IWebUtils, WebUtils>(); 
            container.RegisterType<IUserService, UserService>(); 

            _userService = DependencyResolver.Current.GetService<IUserService>();

            _doctorsService = DependencyResolver.Current.GetService<IDoctorsService>();

        }
        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("alldoctors")]
        [SwaggerOperation("AllDoctors")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult AllDoctors()
        {
            List<Doctors> docs = new List<Doctors>();
            docs = _doctorsService.GetAllDoctors();
            var responseModel = new ResponseModel<List<Doctors>> { Result = docs }; 
            return Ok(responseModel);
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("bookappointment")]
        [SwaggerOperation("BookAppointment")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult BookAppointment(Appointmentmodel userInformation)
        {
            var response = new ResponseModel<BoolResponse> { };
            var user = new TalkToTreatService.Entities.User
            {
                Email = userInformation.EmailId,
                Name = userInformation.FullName,
                PatientAge = userInformation.PatientAge,
                Gender = userInformation.Gender,
                AppointMentType = userInformation.AppointMentType,
                AppointMentDate = userInformation.AppointMentDate,
                DoctorId = userInformation.DoctorId,
                Mobile=userInformation.MobileNo,
                MobileNo = userInformation.MobileNo,
                Country =userInformation.Country 
            };

            var result = _userService.InsertEnquiry(user);
            if (result.IsValid)
            {
                userInformation.AppointmentNumber = result.MessageCode;
                Models.SendEmail sendEmail = new Models.SendEmail();
                var body = SendMailconfirmation(userInformation);

                sendEmail.Send("", userInformation.EmailId, "", "", "Appointment Request Confirmation", body);
                //Thread email = new Thread(delegate ()
                //{
                //    sendEmail.Send("", _registration.EmailId, "", "", "Appointment Request Confirmation", body);
                //});

                var UserInsertresp = _userService.InsertUser(user);
                if (UserInsertresp.IsValid)
                {
                    var Regbody = SendRegistrationMailconfirmation(userInformation);
                    sendEmail.Send("", userInformation.EmailId, "", "", "Registration Confirmation", Regbody);

                    //Thread email = new Thread(delegate ()
                    //{
                    //    sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", Regbody);
                    //});
                } 
            }
            if (!result.IsValid) return BadRequest(result.MessageCode + ":" + result.Message);
            var responseModel = new ResponseModel<BoolResponse> { Result = result };
            responseModel.Message = "Appointent Requested Successfully !";
            return Ok(responseModel);
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.Route("GetDoctorsbySearch")]
        [SwaggerOperation("GetDoctorsbySearch")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Forbidden)]
        public IHttpActionResult GetDoctorsbySearch(string search)
        {
            List<Doctors> docs = new List<Doctors>();
            docs = _doctorsService.GetDoctorsBySearch(search);
            var responseModel = new ResponseModel<List<Doctors>> { Result = docs };
            return Ok(responseModel);
        }

        protected string SendMailconfirmation(Appointmentmodel model)
        {
            string name = "";
            string subject, loginid, password;
            string body = "";
            try
            {
                string date = "";

                string to = model.EmailId;
                #region
                body = @" <div style='margin: auto; background: #fff url(http://qlook.bz/images/topborder.png) top left repeat-x; border: 2px solid #ddd; width: 700px; padding-bottom: 7px; -webkit-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); -moz-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65);'>
           <table width='700' style='background: #14c5aa; margin: 0px auto; padding: 45px 0px;'>
        <tr>
            <td>

                <table cellpadding='0' cellspacing='0' style='background: none repeat scroll 0 0 #fff; border: 7px solid #dcdcdc; color: #333; font-family: Arial,Helvetica,sans-serif; margin: 0 48px; width: 585px;'>


                    <tr>
                        <td>

                            <table width='585'>
                                <tr>
                                    <td style='width: 250px; height: 70px; background: #223a66;'><a   href='https://talktotreat.com' >
                                        <img src='https://talktotreat.com/Assets/images/logo.png' height='45px' border='0' style='height: 45px; margin-left: 6px; float: left;' />
                                    </a><h2><span style='color: white;height:65px; margin-left:5px'>Talk to Treat</span></h2></td>
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 585px;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div>
                                <div class='clear'></div>

                                <table style='border-top: 1px solid rgb(204, 204, 204); height: auto; padding-bottom: 0px; padding-left: 12px; padding-top: 6px; width: 585px;'>
                                    <tr>
                                        <td>
                                            <h1 style='font-size: 15px; font-family: arial; font-weight: normal; margin: 15px 0px 15px; color: #3f3f42; text-decoration:none;'> Dear,<span style='color:#3b7bd1;'> $$Name$$ </span> </h1>
                                            <p style='font-size: 13px;   margin: 0px; color: #484848;'>Thanks for signing up on TalkToTreat.Com </p>

                                             <br/>
<p style='font-size:11px; font-family: arial; font-weight: normal; margin: 7px 4px 5px 0px; color: #3f3f42;'> <b> Your Appointment details are given below. <b></p> 
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Appointment Number :</b> $$lblAppointmentNumber$$ </p>
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Appointment Date : </b> $$lblAppointmentDate$$ </p> 

                                            </table>
                                            <div style='clear: both'></div>
                                            <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 15px 0px 7px 15px; color: #3f3f42;'>
                                                If you have any issues verfying your email, we will be happy to help you.
                                            </p>
                                            <strong style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 0px 0 0 15px; color: #131313;'>You can contact  us on support@hatalktotreat.com</strong>
                                            <div style='clear: both'></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table width='554'>
                                                <tr>
                                                    <td width='70%' style='padding-top:20px;'>

                                                        <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 6px 0px 7px 15px; float: left; color: #3f3f42;'>
We look forward to seeing you on our website.  <br/><br/>
                                                            Sincerely,
                <br />
                                                            Talk To Treat Team<br/><br/>
talktotreat.com

                                                        </p>
                                                    </td>                                                   
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                </table>
                                <div class='clear'></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </div>";
                #endregion
                //mail body will be here
                body = body.Replace("$$lblAppointmentNumber$$", model.AppointmentNumber);
                body = body.Replace("$$Name$$", model.FullName);
                body = body.Replace("$$lblAppointmentDate$$", model.AppointMentDate.ToLongTimeString());

            }
            catch (Exception es)
            {

            }
            return body;
        }
        protected string SendRegistrationMailconfirmation(Appointmentmodel model)
        {
            string name = "";
            string subject, loginid, password;
            string body = "";
            try
            {
                string date = "";

                string to = model.EmailId;
                #region
                body = @" <div style='margin: auto; background: #fff url(http://qlook.bz/images/topborder.png) top left repeat-x; border: 2px solid #ddd; width: 700px; padding-bottom: 7px; -webkit-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); -moz-box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65); box-shadow: 0 2px 9px rgba(0, 0, 0, 0.65);'>
           <table width='700' style='background: #14c5aa; margin: 0px auto; padding: 45px 0px;'>
        <tr>
            <td>

                <table cellpadding='0' cellspacing='0' style='background: none repeat scroll 0 0 #fff; border: 7px solid #dcdcdc; color: #333; font-family: Arial,Helvetica,sans-serif; margin: 0 48px; width: 585px;'>


                    <tr>
                        <td>

                            <table width='585'>
                                <tr>
                                    <td style='width: 250px; height: 70px; background: #223a66;'><a   href='https://talktotreat.com' >
                                        <img src='https://talktotreat.com/Assets/images/logo.png' height='45px' border='0' style='height: 45px; margin-left: 6px; float: left;' />
                                    </a><h2><span style='color: white;height:65px; margin-left:5px'>Talk to Treat</span></h2></td>
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 585px;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div>
                                <div class='clear'></div>

                                <table style='border-top: 1px solid rgb(204, 204, 204); height: auto; padding-bottom: 0px; padding-left: 12px; padding-top: 6px; width: 585px;'>
                                    <tr>
                                        <td>
                                            <h1 style='font-size: 15px; font-family: arial; font-weight: normal; margin: 15px 0px 15px; color: #3f3f42; text-decoration:none;'> Dear,<span style='color:#3b7bd1;'> $$Name$$ </span> </h1>
                                            <p style='font-size: 13px;   margin: 0px; color: #484848;'>Thanks for signing up on TalkToTreat.Com </p>

                                             <br/>
<p style='font-size:11px; font-family: arial; font-weight: normal; margin: 7px 4px 5px 0px; color: #3f3f42;'> <b> Your Appointment details are given below. <b></p> 
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Username :</b> $$lblEmail$$ </p>
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Password : </b> $$body$$ </p> 

                                            </table>
                                            <div style='clear: both'></div>
                                            <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 15px 0px 7px 15px; color: #3f3f42;'>
                                                If you have any issues verfying your email, we will be happy to help you.
                                            </p>
                                            <strong style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 0px 0 0 15px; color: #131313;'>You can contact  us on support@hatalktotreat.com</strong>
                                            <div style='clear: both'></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table width='554'>
                                                <tr>
                                                    <td width='70%' style='padding-top:20px;'>

                                                        <p style='font-size: 12px; font-family: Arial; font-weight: normal; margin: 6px 0px 7px 15px; float: left; color: #3f3f42;'>
We look forward to seeing you on our website.  <br/><br/>
                                                            Sincerely,
                <br />
                                                            Talk To Treat Team<br/><br/>
talktotreat.com

                                                        </p>
                                                    </td>                                                   
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                </table>
                                <div class='clear'></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </div>";
                #endregion
                //mail body will be here
                body = body.Replace("$$lblEmail$$", model.EmailId);
                body = body.Replace("$$Name$$", model.FullName);
                body = body.Replace("$$body$$", model.Password);

            }
            catch (Exception es)
            {

            }
            return body;
        }

    }
}