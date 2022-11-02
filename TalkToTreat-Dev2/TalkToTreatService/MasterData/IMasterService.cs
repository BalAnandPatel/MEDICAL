using System;
using System.Collections.Generic; 
using TalkToTreatService.Entities;

namespace Service.MasterData
{
    /// <summary>
    ///Data Import service interface
    /// </summary>
    public interface IMasterService
    {
        //void ProcessImport();
        List<Locations> GetLocations();
        List<Treatment> GetTreatments();
        List<TreatmentPackage> GetTreatmentsByFilter(string country, string state, string city,string postCode, string hospitalid, string treatmentid, decimal? minCost, decimal? maxCost, string searchText);
        List<TalkToTreatService.Entities.Service> GetServices();
    }
}
