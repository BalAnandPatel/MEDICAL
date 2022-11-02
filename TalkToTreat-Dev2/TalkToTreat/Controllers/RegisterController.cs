using System; 
using System.Web.Mvc; 
using TalkToTreatService.Users;
using BaseApiApp.Entities.Account;
using System.Threading; 
namespace TalkToTreat.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;
        public RegistrationController()
        { 
            _userService = DependencyResolver.Current.GetService<IUserService>();

        }
        [ValidateAntiForgeryToken] 
        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create(Registration _registration)
        { 
            string invalid = null;  
            try
            {
                var user = new TalkToTreatService.Entities.User { 
                    Email = _registration.EmailId,
                    Name=_registration.FullName,
                    Mobile = _registration.MobileNo,
                    MobileNo = _registration.MobileNo,
                    Phone = _registration.MobileNo,
                    PhoneNumber = _registration.MobileNo,
                    Password =_registration.Password,
                    PatientAge=_registration.PatientAge,
                    Remark=_registration.Remark
                };

                var resp = _userService.InsertUser(user);
                if (resp.IsValid)
                {
                    TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
                    var body =SendMailconfirmation(_registration); 
                    Thread email = new Thread(delegate ()
                    {
                        sendEmail.Send("", _registration.EmailId, "", "", "Registration Confirmation", body);
                    });
                    return (RedirectToAction("Confirmation", "Home"));
                }
                else { return (RedirectToAction("Register", "Home", new { UserValid = invalid })); }
            }
            catch(Exception ex)
            { 
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("Register", "Home", new { UserValid = invalid })); 
            }
            return View();
        }




         
        public JsonResult Subscribe(string emailId)
        { 
             _userService.Subscribe(emailId);
            TalkToTreat.Models.SendEmail sendEmail = new TalkToTreat.Models.SendEmail();
            var body = SendSubscribeMailconfirmation(emailId);
            Thread email = new Thread(delegate ()
            {
                sendEmail.Send("", emailId, "", "", "Subscribtion Confirmation", body);
            });
            return Json( null, JsonRequestBehavior.AllowGet);
        }
        protected string SendSubscribeMailconfirmation(string EmailId)
        {
            string name = "";
            string subject, loginid, password;
            string body = "";
            try
            {
                string date = "";

                string to = EmailId;
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
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 250px;'>
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
                                            <p style='font-size: 13px;   margin: 0px; color: #484848;'>Thanks for Subscribing to TalkToTreat.Com </p>

                                             <br/>

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
                body = body.Replace("$$lblEmail$$", EmailId); 

            }
            catch (Exception es)
            {

            }
            return body;
        }

        protected string SendMailconfirmation(Registration model)
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
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 250px;'>
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
                body = body.Replace("$$Name$$", model.EmailId); 
                body = body.Replace("$$body$$", model.Password);

            }
            catch (Exception es)
            {

            }
            return body;
        }





    }
}