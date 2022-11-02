using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient; 
using System.Web.Security;
using System.Security.Principal;
using System.Text;
using System.Security.Cryptography;
using TalkToTreat.Models;

namespace TalkToTreat.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class AdminLoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {
            //if (Session["adminuserdetails"] == null)
            //{
            //    return Redirect("/Adminlogin/Index");
            //    //return (RedirectToAction("Index", "Home"));
            //}
            Login _login = new Login();
            if (Request["UserValid"] != null)
            {  
                
                ViewBag.Invalid = Request["UserValid"].ToString();
              
                if (TempData["UserLogin"] != null)
                {
                    ViewBag.abc = "yes";
                    string userid = TempData["userid"].ToString();
                    string username = TempData["username"].ToString();

                    ViewBag.userid = userid;
                    ViewBag.username = username;
                    FormsAuthentication.SignOut();
                    HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                    System.Web.HttpContext.Current.Session.RemoveAll();
                    Session["adminuserdetails"] = null;
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Abandon();
                }
                
            }
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            System.Web.HttpContext.Current.Session.RemoveAll();
            Session["adminuserdetails"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            string hdn = null;
            hdn= GenerateCoupon(16);
            ViewBag.hdrandom = hdn;

            AdminLogin login = new AdminLogin();

            return View(login);
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create(AdminLogin _login)
        {
            string PassMatch = null;
            string invalid = null;
            Guid Id;
            int userid = 0;
            string username = null;
            string status = null;
            string textvalue = null;
            string Account_status = null;
            bool isblocked = true;
            string EmployeeImageURl = null;
            int? WrongAttempt = null;
            string Blocked = null;

            try
            {
                //if (this.IsCaptchaValid("Validate your captcha"))
                //{
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TalktoTreatConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@username", _login.User_name);
                DataSet dsmenupass = new DataSet();
                con.Open();
                da.Fill(dsmenupass);
                con.Close();

                DataTable dtmenupass = dsmenupass.Tables[0];
                if (dtmenupass.Rows.Count > 0)
                {
                    Id = Guid.Parse(dtmenupass.Rows[0]["employee_id"].ToString());
                    userid = Convert.ToInt32(dtmenupass.Rows[0]["employee_id1"]);
                    username = dtmenupass.Rows[0]["userid"].ToString();
                    PassMatch = dtmenupass.Rows[0]["password"].ToString();
                    textvalue = dtmenupass.Rows[0]["Textvalue"].ToString();
                    status = dtmenupass.Rows[0]["status"].ToString();
                    Account_status = dtmenupass.Rows[0]["accountstatus"].ToString();
                    EmployeeImageURl = dtmenupass.Rows[0]["employee_image"].ToString();                    
                    isblocked = Convert.ToBoolean(dtmenupass.Rows[0]["IsBlocked"]);

                    //string sValue = LoginX._rndomcode;
                    //string newhash = EncryptPasswordSha(PassMatch + sValue);

                    if (textvalue == _login.hdrandom && isblocked == false)
                    {
                        ViewBag.userid = userid;
                        ViewBag.username = username;
                        TempData["userid"] = userid;
                        TempData["username"] = username;
                        Session["adminuserdetails"] = Id + "(@#$)" + username + "(@#$)" + Account_status + "(@#$)" + EmployeeImageURl;

                        if (status != "Active")
                        {
                            return Redirect("/Login/Index");
                        }
                        else
                        {
                            if (Account_status == "admin")
                            {
                                return Redirect("/AdminHome/Index");
                            } 
                        }
                    }
                    else if (textvalue != _login.hdrandom && isblocked == false)
                    {
                        int returnbyproc = 0;
                        _Mysave _mysave = new _Mysave("WrongAttemp_password");
                        _mysave.AddParameter("@User_name", _login.User_name);
                        returnbyproc = _mysave.ExecuteSave();
                        invalid = "Your username or password is incorrect. Please enter correct username or password and try again.";
                        return (RedirectToAction("Index", "AdminLogin", new { UserValid = invalid }));
                    }
                    else if (WrongAttempt == 6 && Blocked == "Blocked")
                    {
                        invalid = "Your account is bocked. Please contact ims admin.";
                        return (RedirectToAction("Index", "AdminLogin", new { UserValid = invalid }));
                    }
                    else
                    {
                        invalid = "Your account is bocked due to 5 times wrong username or password. Please contact ims admin.";
                        return (RedirectToAction("Index", "AdminLogin", new { UserValid = invalid }));
                    }

                }
                else
                {
                    invalid = "Your username or password is incorrect. Please enter correct username or password.";
                    return (RedirectToAction("Index", "AdminLogin", new { UserValid = invalid }));
                }                
            }
            catch(Exception ex)
            {
                //string mes = ex.Message;
                //_login.logmeassage = mes;
                //dropdown.Savesession();
                //invalid = "Session Error "+ _login.logmeassage +" ";
                //return (RedirectToAction("Index", "Login", new { UserValid = invalid }));
                invalid = "Your username or password is incorrect. Please enter correct username or password.";
                return (RedirectToAction("Index", "AdminLogin", new { UserValid = invalid }));

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["adminuserdetails"] = null;
            Session.Abandon();
            return RedirectToAction("Index");
        }

        #region(PASSWORD ENCRYPT/DECRYPT)
        public static string EncryptPasswordSha(string password)
        {
            SHA256CryptoServiceProvider shaHasher = new SHA256CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = shaHasher.ComputeHash(encoder.GetBytes(password));
            System.Text.StringBuilder strHex = new System.Text.StringBuilder();
            foreach (byte b in hashedDataBytes)
            {
                strHex.Append(b.ToString("x2").ToLower());
            }
            string str = strHex.ToString();
            return str;
        }
        #endregion

        #region(GENERATECOUPON)
        private string GenerateCoupon(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            LoginX._rndomcode = result.ToString();
            return result.ToString();
        }
        #endregion
    }
}