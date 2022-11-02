using Newtonsoft.Json;
using TalkToTreatService.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Omnicx.Backend.Framework.Interface;
using Microsoft.WindowsAzure.Storage.Blob;
using Omnicx.Backend.Framework.Services;
using ExcelDataReader;
using Microsoft.WindowsAzure.Storage;
using System.Globalization;
using Omnicx.Backend.Framework.Entities;
using TalkToTreatService.Data;
using BaseApiApp.Framework.Cryptography;
using BaseApiApp.Framework;
using TalkToTreatService.Data.Dbml;
using System.Data.Linq;
using System.Xml.Linq;
using System.Web.UI.WebControls;

namespace TalkToTreatService.Users
{
    /// <summary>
    /// Payment service
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields 
        private TalktoTreatClassesDataContext masterContext;
        private readonly IWebUtils _webUtils;
        #endregion

        public UserService(IWebUtils webUtils)
        {
            _webUtils = webUtils;
        }
        #region Methods
        public User Login(string userName, string password, bool checkPwd)
        {
            try
            {
                var dataContextFactory = new DataContextFactory();
                masterContext = dataContextFactory.TalktoTreatDataContext();
                var dbResponse = masterContext.procSiteAuthenticate(userName);

                User user = (from o in dbResponse
                             select new User
                             {
                                 RecordId = o.ID,
                                 Id = (long)o.iid,
                                 Username = o.UserName,
                                 Password = o.Password,
                                 Salt = o.Salt,
                                 Name = o.FirstName,
                                 Email = o.Email,
                                 NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                                 Mobile = o.PhoneNo ?? string.Empty,
                                 Gender = o.Gender ?? string.Empty

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
            catch (Exception ex)
            {
                var msg = ex.Message;
                User user = new User();
                return user;
            }
        }

        public User GetUserByUserName(string userName)
        {
            try
            {
                var dataContextFactory = new DataContextFactory();
                masterContext = dataContextFactory.TalktoTreatDataContext();
                var dbResponse = masterContext.procSiteAuthenticate(userName).ToList(); 

                User user = (from o in dbResponse
                             select new User
                             {
                                 RecordId = o.ID,
                                 Id = (long)o.iid,
                                 Username = o.UserName,
                                 Password = o.Password,
                                 Salt = o.Salt,
                                 Name = o.FirstName,
                                 Email = o.Email,
                                 NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                                 Mobile = o.PhoneNo ?? string.Empty,
                                 Gender = o.Gender ?? string.Empty

                             }).FirstOrDefault();


                return user;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                User user = new User();
                return user;
            }
        }

        public BoolResponse InsertEnquiry(User user)
        {
            try
            {
                if (user.AppointMentDate < DateTime.Now)
                {
                    user.AppointMentDate = DateTime.Now.Date;
                }
                user.IpAddress = _webUtils.GetCurrentIpAddress();
                var dataContextFactory = new DataContextFactory();
                masterContext = dataContextFactory.TalktoTreatDataContext();
                var dbResponse = masterContext.procInsertEnquiry(user.Name,
                                                                    user.Email,
                                                                    user.PatientAge,
                                                                    user.Remark,
                                                                    user.Mobile,
                                                                    user.Country,
                                                                    user.City,
                                                                    user.IpAddress,
                                                                    user.DoctorId,
                                                                    user.AppointMentDate,
                                                                    user.AppointMentType,
                                                                    user.Gender,
                                                                    user.UserId);

                BoolResponse resp =
                    (from o in dbResponse
                     select
                         new BoolResponse
                         {
                             IsValid = o.IsValid,
                             Id = (long)o.IId,
                             Message = "true",
                             MessageCode = o.IId.ToString(),
                             Acknowledge = string.IsNullOrEmpty(user.Password) ? AcknowledgeType.Error : AcknowledgeType.Success
                         }
                        ).FirstOrDefault();
                return resp;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public virtual BoolResponse InsertUser(User user)
        {
            user.IpAddress = _webUtils.GetCurrentIpAddress();
            var res = InsertSiteUser(user);
            user.RecordId = res.RecordId;
            user.Id = res.Id;
            
            if (res.IsValid == true && res.Acknowledge == AcknowledgeType.Success && user.IsCreatedByAdmin)
            {
                user.Token = Guid.NewGuid();
                if (string.IsNullOrEmpty(user.Username))
                    user.Username = user.Email;            
            }
            if (res.IsValid == true && res.RecordId != Guid.Empty)
            {
                var siteUser = GetUserByGuid(res.RecordId);
                siteUser.Id = user.Id;              
            }
            else
            {
                res.IsValid = false;
                res.Message = "User Already Exists";
            }
            return res;
        }
        public BoolResponse InsertSiteUser(User user)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            string salt = string.Empty;
            var passowrd = PasswordHelper.Generate();
            if (string.IsNullOrEmpty(user.Password))
                user.Password = passowrd;
            user.Password = PasswordHelper.EncryptPassword(ref salt, user.Password);
            var dbResponse = masterContext.procUpsertUser(Guid.Empty, user.Name,
                         "", user.Email, user.Password, salt, user.Culture, user.NewsLetterSubscribed, user.Remark, user.Gender, user.Mobile, user.Country, user.IpAddress, user.Mobile, user.PatientAge);

            BoolResponse resp =
                (from o in dbResponse
                 select
                     new BoolResponse
                     {
                         RecordId = o.RecordId.GetValueOrDefault(),
                         IsValid = o.IsValid,
                         Id = (long)o.IId,
                         MessageCode = o.IsExistingUser == false ? "false" : "true", //Check user is new or existing ticket id 213,
                         Message = "true",
                         Acknowledge = string.IsNullOrEmpty(user.Password) ? AcknowledgeType.Error : AcknowledgeType.Success
                     }
                    ).FirstOrDefault();
            user.Password = passowrd;
            return resp;
        }
        public List<UserVallet> GetUserValletByGuid(Guid userId)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procGetUserValletBy(userId);
            List<UserVallet> uservalets = (from o in dbResponse
                                           select new UserVallet
                                           {
                                               Id = o.ID,
                                               FileName = o.FileName,
                                               FileUrl = o.FileUrl,
                                               Description = o.Description,
                                               Type = o.FileType,
                                           }).ToList();
            return uservalets;
        }
        public User GetUserByGuid(Guid userId)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procGetUserById(userId);
            User user =
                (from o in dbResponse
                 select
                     new User
                     {
                         RecordId = o.ID,
                         Username = o.UserName,
                         UserId = o.ID,
                         Name = o.FirstName ,
                         Email = o.Email,
                         Mobile = o.PhoneNo,
                         Remark = o.PatientCondition,
                         Gender = o.Gender,
                         PostCode = o.Postcode,
                         PatientAge = o.PatientAge.GetValueOrDefault(),
                         Created = o.Created,
                         CreatedBy = o.CreatedBy,
                         Image = o.Image,
                         NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                         Password = o.Password,
                         PhoneNumber = o.PhoneNo
                     }).FirstOrDefault();
            return user;
        }


        public List<BaseApiApp.Entities.Account.Appointment> GetAppointmentDetailByUser(int Pagenum, string Searchvalue)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllAppointmentByUser(Searchvalue, Pagenum);
            List<BaseApiApp.Entities.Account.Appointment> appointment = (from o in dbResponse
                                                                         select new BaseApiApp.Entities.Account.Appointment
                                                                         {
                                                                             Id = o.Id,
                                                                             EmailId = o.EmailId,
                                                                             MobileNo = o.MobileNo,
                                                                             DoctorName = o.DoctorName,
                                                                             Gender = o.Gender,
                                                                             FullName = o.FullName,
                                                                             IsReplied = o.IsReplied,
                                                                             Body = o.Body,
                                                                             Subject = o.Subject,
                                                                             DoctorId = o.DoctorId.GetValueOrDefault(),
                                                                             AppointMentDate = o.AppointmentDate.GetValueOrDefault(),
                                                                             PatientAge = o.PatientAge.GetValueOrDefault(),
                                                                             ConsultationFee = o.ConsultationFee.GetValueOrDefault(),
                                                                             DoctorImage = o.Image
                                                                         }).ToList();
            return appointment;
        }
        public List<BaseApiApp.Entities.Account.Appointment> GetAppointmentListByUser(int Pagenum, string Searchvalue)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAppointmentListByUser(Searchvalue, Pagenum);
            List<BaseApiApp.Entities.Account.Appointment> appointment = (from o in dbResponse
                                                                         select new BaseApiApp.Entities.Account.Appointment
                                                                         {
                                                                             Id = o.Id,
                                                                             EmailId = o.EmailId,
                                                                             MobileNo = o.MobileNo,
                                                                             DoctorName = o.DoctorName,
                                                                             FullName = o.FullName,
                                                                             DoctorId = o.DoctorId.GetValueOrDefault(),
                                                                             AppointMentDate = o.AppointmentDate.GetValueOrDefault(),
                                                                             PatientAge = o.PatientAge.GetValueOrDefault(),
                                                                             ConsultationFee = o.ConsultationFee,
                                                                             DoctorImage = o.Image
                                                                         }).ToList();
            return appointment;
        }
        public BaseApiApp.Entities.Account.Appointment GetAppointmentDetail(Guid recordId)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllAppointmentDetailByUser(recordId);
            BaseApiApp.Entities.Account.Appointment appointment = (from o in dbResponse
                                                                   select new BaseApiApp.Entities.Account.Appointment
                                                                   {
                                                                       Id = o.Id,
                                                                       EmailId = o.Email,
                                                                       MobileNo = o.Phone,
                                                                       DoctorName = o.DoctorName,
                                                                       Gender = o.Gender,
                                                                       FullName = o.Name,
                                                                       IsReplied = o.IsReplied,
                                                                       Body = o.Body,
                                                                       Subject = o.Subject,
                                                                       DoctorId = o.DoctorId.GetValueOrDefault(),
                                                                       AppointMentDate = o.AppointmentDate.GetValueOrDefault(),
                                                                       PatientAge = o.Age.GetValueOrDefault(),
                                                                       ConsultationFee = o.ConsultationFee,
                                                                       DoctorImage = o.Image
                                                                   }).FirstOrDefault();
            return appointment;
        }
        public BoolResponse UpdateUserProfile(User user)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            string salt = string.Empty;
            var passowrd = PasswordHelper.Generate();
            if (string.IsNullOrEmpty(user.Password))
                user.Password = passowrd;
            user.Password = PasswordHelper.EncryptPassword(ref salt, user.Password);
            var dbResponse = masterContext.procUpdateUserProfile(user.UserId, user.Name, "", user.Email, user.Remark, user.Gender, user.Mobile, user.Country, user.PatientAge, user.Mobile);

            BoolResponse resp =
                (from o in dbResponse
                 select
                     new BoolResponse
                     {
                         RecordId = o.RecordId.GetValueOrDefault(),
                         IsValid = o.IsValid,
                         Id = (long)o.IId,
                         MessageCode = o.IsExistingUser == false ? "false" : "true", //Check user is new or existing ticket id 213,
                         Message = "true",
                         Acknowledge = string.IsNullOrEmpty(user.Password) ? AcknowledgeType.Error : AcknowledgeType.Success
                     }
                    ).FirstOrDefault();
            user.Password = passowrd;
            return resp;
        }
        public BoolResponse AddDocument(Guid UserId, UserVallet model)
        {
            var dataContextFactory = new DataContextFactory();
            var walletJson = JsonConvert.SerializeObject(model);
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procJsonUpsertVallet_New(UserId, walletJson, UserId.ToString(), model.Type, model.Description,
                model.FileUrl, model.FileName, model.Id);

            BoolResponse resp =
                (from o in dbResponse
                 select
                     new BoolResponse
                     {
                         IsValid = o.IsValid.GetValueOrDefault(),
                         RecordId = o.RecordId.GetValueOrDefault(),
                         Message = o.Message
                     }
                    ).FirstOrDefault();
            return resp;
        }

        public BoolResponse DeleteDocument(UserVallet model)
        {
            var dataContextFactory = new DataContextFactory();
            var walletJson = JsonConvert.SerializeObject(model);
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procDeleteObject(model.Id, 4);

            BoolResponse resp =
                (from o in dbResponse
                 select
                     new BoolResponse
                     {
                         IsValid = o.IsValid.GetValueOrDefault(),
                         RecordId = o.RecordId.GetValueOrDefault(),
                         Message = o.Message
                     }
                    ).FirstOrDefault();
            return resp;
        }

        public List<BaseApiApp.Entities.Account.Appointment> GetAppointmentByDoctor(int Pagenum, string Searchvalue, Guid DoctorId)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();

            var dbResponse = masterContext.GetAllAppointmentByDoctor_New(Searchvalue, Pagenum, DoctorId);
            List<BaseApiApp.Entities.Account.Appointment> appointment = (from o in dbResponse
                    select new BaseApiApp.Entities.Account.Appointment
                    {
                        Id = o.Id,
                        EmailId = o.EmailId,
                        MobileNo = o.MobileNo,
                        DoctorName = o.DoctorName,
                        Gender = o.Gender,
                        FullName = o.FullName,
                        IsReplied = o.IsReplied,
                        DoctorId = o.DoctorId.GetValueOrDefault(),
                        AppointMentDate = o.AppointmentDate.GetValueOrDefault(),
                        PatientAge = o.PatientAge.GetValueOrDefault(),
                        EnquiryResponseXml = CommonUtils.GetNodeValue(o.Response, "OrderLines"),
                        //enquiryResponses= JsonConvert.DeserializeObject<List<BaseApiApp.Entities.Account.EnquiryResponse>>(o.Response ?? "", JsonSettings)
                    }).ToList();
            return appointment;
        }
        protected JsonSerializerSettings JsonSettings
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                return settings;
            }
        }

        public User SocialLogin(string userName, bool IsPhoneNumber)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.procSiteAuthenticateSocialLogin(userName, IsPhoneNumber);

            User user = (from o in dbResponse
                         select new User
                         {
                             RecordId = o.ID,
                             Id = (long)o.iid,
                             Username = o.UserName,
                             Password = o.Password,
                             Salt = o.Salt,
                             Name = o.FirstName,
                             Email = o.Email,
                             NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                             Mobile = o.PhoneNo ?? string.Empty,
                             Gender = o.Gender ?? string.Empty

                         }).FirstOrDefault();
            if (user.RecordId == Guid.Empty)
            {
                return null;
            }
            return user;
        }
        public void Subscribe(string EmailId)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            masterContext.procUpsertSubscribers(EmailId, false);
        }


        public User GetUserByIdentifier(string username)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetUserByIdentifier(username);
            User user =
                (from o in dbResponse
                 select
                     new User
                     {
                         RecordId = o.ID,
                         Username = o.UserName,
                         UserId = o.ID,
                         Name = o.FirstName,
                         Email = o.Email,
                         Mobile = o.PhoneNo,
                         Remark = o.PatientCondition,
                         Gender = o.Gender,
                         PostCode = o.Postcode,
                         PatientAge = o.PatientAge.GetValueOrDefault(),
                         Created = o.Created,
                         CreatedBy = o.CreatedBy,
                         Image = o.Image,
                         NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                         Password = o.Password,
                         PhoneNumber = o.PhoneNo
                     }).FirstOrDefault();
            return user;

        }


        public bool UpadtePassword(Guid UserId ,string Salt , string Password)
        {
            try
            {              
                var dataContextFactory = new DataContextFactory();
                masterContext = dataContextFactory.TalktoTreatDataContext();
                var dbResponse = masterContext.UpdateUserPassword(UserId,
                                                                  Password,
                                                                  Salt);               
                     
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public User LoginDoctor(string userName, string password, bool checkPwd)
        {
            try
            {
                var dataContextFactory = new DataContextFactory();
                masterContext = dataContextFactory.TalktoTreatDataContext();
                var dbResponse = masterContext.procSiteAuthenticate(userName);

                User user = (from o in dbResponse
                             select new User
                             {
                                 RecordId = o.ID,
                                 Id = (long)o.iid,
                                 Username = o.UserName,
                                 Password = o.Password,
                                 Salt = o.Salt,
                                 Name = o.FirstName,
                                 Email = o.Email,
                                 NewsLetterSubscribed = o.NewsletterSubscribe.GetValueOrDefault(),
                                 Mobile = o.PhoneNo ?? string.Empty,
                                 Gender = o.Gender ?? string.Empty

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
            catch (Exception ex)
            {
                var msg = ex.Message;
                User user = new User();
                return user;
            }
        }
    }
}
