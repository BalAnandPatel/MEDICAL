using BaseApiApp.Entities.Account;
using System;
using System.Collections.Generic; 
using TalkToTreatService.Entities;

namespace Service.MasterData
{
    /// <summary>
    ///Data Import service interface
    /// </summary>
    public interface IDoctorsService
    { 
        List<Doctors> GetAllDoctors();
        List<Doctors> GetDoctorsBySearch(string Search);
        List<Appointment> GetAllAppointments();
        BoolResponse UpsertDoctor(Doctors model);
        Doctors Login(string userName, string password, bool checkPwd);
        BoolResponse UpdateAppointment(Appointment appointment);
    }
}
