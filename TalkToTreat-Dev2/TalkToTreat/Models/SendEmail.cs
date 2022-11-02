using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TalkToTreat.Models
{
    public class SendEmail
    { 
        public void Sendmail(string to, string from, string password, string subject, string body, HttpPostedFileBase postedFile)
        {
            using (MailMessage mm = new MailMessage(from, to))
            {
                mm.Subject = subject;
                mm.Body = body;
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    mm.Attachments.Add(new Attachment(postedFile.InputStream, fileName));
                }
                mm.IsBodyHtml = false; 

                using (SmtpClient smtp = new SmtpClient("webmail.roovea.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(from, password);

                    smtp.Host = "webmail.roovea.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = true;
                    smtp.Port = 587; 
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        MailMessage Msg = new MailMessage();
                        Msg.From = new MailAddress(from);
                        Msg.To.Add(to);
                        //Msg.Subject = txtSubject.Text;
                        Msg.Body = body;
                        Msg.IsBodyHtml = true;

                        SmtpClient smtp1 = new SmtpClient();
                        smtp1.Host = "smtp.gamil.com";
                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                        NetworkCred.UserName = "talktotreat@gmail.com";
                        NetworkCred.Password = "talktotreat@123";
                        smtp1.UseDefaultCredentials = true;

                        smtp1.Credentials = NetworkCred;
                        smtp1.Port = 587;
                        smtp1.EnableSsl = true;
                        smtp.Send(Msg); 
                    }
                }
            }
        }
        
        //govind send mail        
        public string Send(string from, string to, string cc, string bcc, string subject, string body)
        {
            //testing
            //to = "test-0r6re1603@srv1.mail-tester.com";
            //cc = "";
            //bcc = "";

            string username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString();
            string pass = System.Configuration.ConfigurationManager.AppSettings["pass"].ToString();
            string host = System.Configuration.ConfigurationManager.AppSettings["host"].ToString();  
            from = System.Configuration.ConfigurationManager.AppSettings["from"].ToString();
            string displayName = System.Configuration.ConfigurationManager.AppSettings["displayName"].ToString();

            string strret = "";
            
            MailMessage mail = new MailMessage();
            if (to != "")
            {
                string[] toMailid = to.Split(new char[] { ',' });
                for (int i = 0; i < toMailid.Length; i++)
                {
                    mail.To.Add(toMailid[i]);
                }
            }
            if (cc != "")
            {
                string[] ccMailid = cc.Split(new char[] { ',' });
                for (int i = 0; i < ccMailid.Length; i++)
                {
                    mail.CC.Add(ccMailid[i]);
                }
            }
            if (bcc != "")
            {
                string[] bccMailid = bcc.Split(new char[] { ',' });
                for (int i = 0; i < bccMailid.Length; i++)
                {
                    mail.Bcc.Add(bccMailid[i]);
                }
            }

            mail.From = new MailAddress(from,displayName);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(host, 25);
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(username, pass);



            try
            {
                smtp.Send(mail);
                strret = "s";
                return strret;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public void SendSMS(string to, string message)
        {
            string key = "sb46eqvfuub62wd";
            string secret = "rjxzukqbhouud9l";
             
            string sURL;


            sURL = "https://www.thetexting.com/rest/sms/json/message/send?api_key=" + key + "&api_secret=" + secret + "&to=" + to + "&text=" + message;

            try
            {

                using (WebClient client = new WebClient())
                {

                    string s = client.DownloadString(sURL);

                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(s);
                    int n = responseObject.Status;
                    if (n == 3)
                    {
                        //MessageBox.Show("Something went wrong with your credentials", "My Demo App"); 
                    }
                    else
                    {
                       // MessageBox.Show("Message Send Successfully", "My Demo App");
                    }

                }


            }
            catch (Exception ex)
            { 
                ex.ToString();
            }
        }
        public class Response
        {
            public string message_id { get; set; }
            public int message_count { get; set; }
            public double price { get; set; }
        }

        public class RootObject
        {
            public Response Response { get; set; }
            public string ErrorMessage { get; set; }
            public int Status { get; set; }
        }
    } 
}