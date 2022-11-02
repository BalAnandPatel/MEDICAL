using System;
using System.Collections.Generic;
using System.Linq;
using Omnicx.Backend.Framework.Interface; 
using TalkToTreatService.Data;
using TalkToTreatService.Data.Dbml;
using TalkToTreatService.Entities;

namespace Service.MasterData
{
    /// <summary>
    /// Payment service
    /// </summary>
    public class HospitalService : IHospitalService
    {
        #region Fields  
        private IOrgService _orgService;
        private TalktoTreatClassesDataContext masterContext;
        public HospitalService()
        {
        }
        #endregion

        #region Ctor

        //public MasterService(  IOrgService orgService)
        //{

        //    _orgService = new OrgService();
        //}

        #endregion

        #region Methods 
        
        public List<Hospital> GetAllHospitals()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllHospitalts();

            List<Hospital> hospital = (from o in dbResponse
                                         select new Hospital
                                         {
                                             Id = o.ID,
                                             Description = o.Description, 
                                             Image = o.Image,
                                             
                                             ShortDescription =o.ShortDescription,
                                            
                                             Code=o.Code,
                                             IsActive = o.IsActive.GetValueOrDefault(),
                                             Name = o.Name,
                                             EstablishmentYesr = o.EstablishmentYesr,
                                             Type = o.Type,
                                             NumberOfbeds = o.NumberOfBeds.GetValueOrDefault(),
                                             PhoneNumber=o.PhoneNumber,                                             
                                             PostCode=o.PostCode,                                             
                                             
                                             IconImage=o.IconImage, 
                                             Spacialities = o.Spacialities,
                                             Infrastructure = o.Infrastructure,
                                             CustomNo = o.RegNo,

                                             PriorityOrder = o.PriorityOrder??0,

                                             Address =o.Address,
                                             CityId = o.CityId ?? 0,
                                             City = o.City,
                                             StateId = o.StateId??0,
                                             State = o.State,                                             
                                             CountryId = o.CountryId ?? 0,
                                             Country = o.Country,

                                             TreatmentId = o.TreatmentId ?? Guid.Empty,
                                             TreatmentDetailID = o.TreatmentDetailID ?? Guid.Empty,


                                         }).ToList();
            return hospital;
        }

        public List<Hospital> GetAllHospitalsHomePage()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllHospitaltsHomePage();

            List<Hospital> hospital = (from o in dbResponse
                                       select new Hospital
                                       {
                                           Id = o.ID,
                                           Description = o.Description,
                                           Image = o.Image,

                                           ShortDescription = o.ShortDescription,
                                           City = o.City,
                                           Code = o.Code,
                                           IsActive = o.IsActive.GetValueOrDefault(),
                                           Name = o.Name,
                                           EstablishmentYesr = o.EstablishmentYesr,
                                           Type = o.Type,
                                           NumberOfbeds = o.NumberOfBeds.GetValueOrDefault(),
                                           PhoneNumber = o.PhoneNumber,
                                           Country = o.Country,
                                           PostCode = o.PostCode,
                                           State = o.State,
                                           IconImage = o.IconImage,
                                           Spacialities = o.Spacialities,
                                           Infrastructure = o.Infrastructure,
                                           CustomNo = o.RegNo,

                                           CityId = o.CityId ?? 0,
                                           CountryId = o.CountryId ?? 0                                                                                     


                                       }).ToList();
            return hospital;
        }
        #endregion
    }
}
