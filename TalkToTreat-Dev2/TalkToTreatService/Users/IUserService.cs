using BaseApiApp.Entities.Account;
using System;
using System.Collections.Generic;
using TalkToTreatService.Entities;

namespace TalkToTreatService.Users
{
    /// <summary>
    ///Data Import service interface
    /// </summary>
    public interface IUserService
    {
        BoolResponse InsertUser(TalkToTreatService.Entities.User user);
        TalkToTreatService.Entities.User Login(string userName, string password, bool checkPwd);
        TalkToTreatService.Entities.User GetUserByUserName(string userName);

        TalkToTreatService.Entities.User GetUserByGuid(Guid userId);
        List<TalkToTreatService.Entities.UserVallet> GetUserValletByGuid(Guid userId); 
        List<BaseApiApp.Entities.Account.Appointment> GetAppointmentDetailByUser(int Pagenum, string Searchvalue);
        BoolResponse InsertEnquiry(TalkToTreatService.Entities.User user);
        BoolResponse UpdateUserProfile(TalkToTreatService.Entities.User user);
        BoolResponse AddDocument(Guid Userid ,UserVallet wallet);
        BoolResponse DeleteDocument(UserVallet wallet); 
        List<BaseApiApp.Entities.Account.Appointment> GetAppointmentByDoctor(int Pagenum, string Searchvalue,Guid DoctorId);

        TalkToTreatService.Entities.User SocialLogin(string userName, bool IsPhoneNumber);
        BaseApiApp.Entities.Account.Appointment  GetAppointmentDetail(Guid recordId);
        List<BaseApiApp.Entities.Account.Appointment> GetAppointmentListByUser(int Pagenum, string Searchvalue);
        void Subscribe(string EmailId);

        TalkToTreatService.Entities.User GetUserByIdentifier(string identifier);

        bool UpadtePassword(Guid UserId, string Salt, string Password);
    }
}
