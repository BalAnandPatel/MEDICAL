using System;
using System.Collections.Generic; 
using TalkToTreatService.Entities;

namespace Service.MasterData
{
    /// <summary>
    ///Data Import service interface
    /// </summary>
    public interface IHospitalService
    { 
        List<Hospital> GetAllHospitals(); 
    }
}
