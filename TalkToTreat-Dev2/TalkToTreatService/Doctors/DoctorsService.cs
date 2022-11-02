using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BaseApiApp.Entities.Account;
using BaseApiApp.Framework.Cryptography;
using Newtonsoft.Json;
using TalkToTreatService.Data;
using TalkToTreatService.Data.Dbml;
using TalkToTreatService.Entities;

namespace Service.MasterData
{
    /// <summary>
    /// Payment service
    /// </summary>
    public class DoctorsService : IDoctorsService
    {
        #region Fields   
        private TalktoTreatClassesDataContext masterContext;
        string from = "emailtest@roovea.com";
        string password = "Test@123";
        public DoctorsService()
        {
        }
        #endregion

      
        #region Methods 
        public Doctors Login(string userName, string password, bool checkPwd)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procSiteAuthenticateDoctor(userName);

            Doctors user = (from o in dbResponse
                         select new Doctors
                         {
                             Id = o.ID,
                             Description = o.Description,
                             Department = o.Department,
                             Designation = o.Designation,
                             Image = o.Image,
                             ShortDescription = o.ShortDescription,
                             City = o.City,
                             Cost = o.Cost.GetValueOrDefault(),
                             Award = o.Award,
                             YearsofExp = o.YearsofExp.GetValueOrDefault(),
                             Hospital = o.Hospital,
                             MobileNumber = o.MobileNumber,
                             IsActive = o.IsActive.GetValueOrDefault(),
                             Name = o.Name,
                             Degree = o.Degree,
                             Salt = o.Salt,
                             Password = o.Password,
                             UserName=o.UserName

                         }).FirstOrDefault();

            if (checkPwd && user != null)
            {
                string salt = user.Salt;
                password = PasswordHelper.EncryptPassword(ref salt, password);
                if (user.Password != password)
                {
                    return null;
                }
            }

            return user;
        }
        public List<Doctors> GetAllDoctors()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllDoctors();

            List<Doctors> hospital = (from o in dbResponse
                                      select new Doctors
                                      {
                                          Id = o.ID,
                                          Description = o.Description,

                                          Department = o.Department,
                                          Designation = o.Designation,
                                          Image = o.Image,
                                          ShortDescription = o.ShortDescription,

                                          City = o.City,
                                          State = o.State,
                                          Country = o.Country,

                                          Cost = o.Cost.GetValueOrDefault(),
                                          Award = o.Award,

                                          YearsofExp = o.YearsofExp.GetValueOrDefault(),
                                          Hospital = o.Hospital,
                                          HospitalID = o.HospitalID ?? Guid.Empty,
                                          MobileNumber = o.MobileNumber,
                                          IsActive = o.IsActive.GetValueOrDefault(),
                                          Gender = o.Gender.GetValueOrDefault(),
                                          Name = o.Name,
                                          

                                          CityId = o.CityId ?? 0,
                                          CountryId = o.CountryId ?? 0,

                                          TreatmentId = o.TreatmentId ?? Guid.Empty,
                                          TreatmentDetailID = o.TreatmentDetailID ?? Guid.Empty,

                                          Degree = o.Degree,
                                          Experience = o.Experience,
                                          AdditionalInfo = o.AdditionalInfo,

                                      }).ToList();
            return hospital;
        }
        public List<Doctors> GetDoctorsBySearch(string Search)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetDoctorsBySearch(Search);

            List<Doctors> hospital = (from o in dbResponse
                                      select new Doctors
                                      {
                                          Id = o.ID,
                                          Description = o.Description,
                                          Department = o.Department,
                                          Designation = o.Designation,
                                          Image = o.Image,
                                          ShortDescription = o.ShortDescription,
                                          City = o.City,
                                          Cost = o.Cost.GetValueOrDefault(),
                                          Award = o.Award,
                                          YearsofExp = o.YearsofExp.GetValueOrDefault(),
                                          Hospital = o.Hospital,
                                          MobileNumber = o.MobileNumber,
                                          IsActive = o.IsActive.GetValueOrDefault(),
                                          Name = o.Name,
                                          Degree = o.Degree
                                      }).ToList();
            return hospital;
        }

        public List<Appointment> GetAllAppointments()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllAppointment();

            List<Appointment> appointment = (from o in dbResponse
                                      select new Appointment
                                      {
                                          EmailId=o.Email, 
                                          MobileNo=o.PhoneNo,
                                          DoctorName=o.DoctorName,
                                          DoctorId=o.DoctorId.GetValueOrDefault(),
                                          AppointMentDate=o.AppointMentDate.GetValueOrDefault(),
                                          PatientAge=o.Age.GetValueOrDefault(),
                                          Gender=o.Gender,
                                          AppointMentType=o.AppointMentType,
                                          FullName = o.Name
                                      }).ToList();
            return appointment;
        }

        public BoolResponse UpsertDoctor(Doctors model)
        {
            var doctorJson = JsonConvert.SerializeObject(model);
            var resp = (from o in masterContext.procJsonUpsertDoctor_New(model.Id, doctorJson,"",model.Name, model.Department, model.IsActive,
                model.UserName,model.Salt,model.Designation,model.UserName,model.ShortDescription,model.Description, model.Password,model.City, 
                "", model.MobileNumber, model.MobileNumber,model.Image, model.Hospital, model.YearsofExp, model.Degree, model.Code)
                        select new BoolResponse
                        {
                            IsValid = o.IsValid.GetValueOrDefault(),
                            Message = o.Message,
                            RecordId = (Guid)o.RecordId
                        }).FirstOrDefault();
            return resp;
        }

        public BoolResponse UpdateAppointment(Appointment appointment)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
         
            var appointmentJson = JsonConvert.SerializeObject(appointment);
            var dbResponse = masterContext.procUpsertEnquiryReply_New(appointment.Id ,appointmentJson, appointment.DoctorName,
                appointment.EmailId, appointment.Subject, appointment.DoctorId, appointment.Body);

            BoolResponse resp =
                (from o in dbResponse
                 select
                     new BoolResponse
                     {
                         RecordId = o.RecordId.GetValueOrDefault(),
                         IsValid = o.IsValid.GetValueOrDefault(),
                         MessageCode = o.MessageCode.ToString(),
                         Message = o.Message,
                     }
                    ).FirstOrDefault();

            if (resp.IsValid == true)
            {
                appointment.AppointmentNumber = resp.MessageCode;
                SendEmail sendEmail = new SendEmail();
                var body= PrepareEmailBody(appointment);
                Thread email = new Thread(delegate ()
                {
                    sendEmail.Send(from, appointment.EmailId, "", "", appointment.Subject, body);
                });
            }
            return resp;
        }
        protected string PrepareEmailBody(Appointment model)
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
                                    <td style='text-align: right; font-size: 15px; padding-right: 15px; color: #333; font-family: Arial,Helvetica,sans-serif; width: 250px;'>$$lblEmail$$
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
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> Appointment date :</b> $$date$$ </p>
<p style='font-size:11px; font-family: arial; font-weight: normal;'><b> </b> $$body$$ </p> 

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
                body = body.Replace("$$date$$", model.AppointMentDate.ToString());
                body = body.Replace("$$body$$", model.Body);

            }
            catch (Exception es)
            {

            }
            return body;
        }


        public Doctors LoginHomePage(string userName, string password, bool checkPwd)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procSiteAuthenticateDoctor(userName);

            Doctors user = (from o in dbResponse
                            select new Doctors
                            {
                                Id = o.ID,
                                Description = o.Description,
                                Department = o.Department,
                                Designation = o.Designation,
                                Image = o.Image,
                                ShortDescription = o.ShortDescription,
                                City = o.City,
                                Cost = o.Cost.GetValueOrDefault(),
                                Award = o.Award,
                                YearsofExp = o.YearsofExp.GetValueOrDefault(),
                                Hospital = o.Hospital,
                                MobileNumber = o.MobileNumber,
                                IsActive = o.IsActive.GetValueOrDefault(),
                                Name = o.Name,
                                Degree = o.Degree,
                                Salt = o.Salt,
                                Password = o.Password,
                                UserName = o.UserName

                            }).FirstOrDefault();

            if (checkPwd && user != null)
            {
                //string salt = user.Salt;
                //password = PasswordHelper.EncryptPassword(ref salt, password);
                if (user.Password != password)
                {
                    return null;
                }
            }

            return user;
        }

        #endregion
    }
}
