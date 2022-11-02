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
    public class MasterService : IMasterService
    {
        #region Fields  
        private IOrgService _orgService;
        private TalktoTreatClassesDataContext masterContext;
        public MasterService()
        {
        }
        #endregion

        #region Methods 
        public List<Treatment> GetTreatments()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllTreatments();

            List<Treatment> treatment = (from o in dbResponse
                                         select new Treatment
                                         {
                                             Id = o.ID,
                                             ParentId = o.ParentId ?? Guid.Empty,
                                             ShortDescription = o.ShortDescription,
                                             Description = o.Description,
                                             Code = o.Code,
                                             Image = o.Image,
                                             Cost = o.Cost.GetValueOrDefault(),
                                             IsActive = o.IsActive.GetValueOrDefault(),
                                             Name = o.Name
                                         }).ToList();
            return treatment;
        }
        public List<Locations> GetLocations()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetAllLocations();

            List<Locations> locations = (from o in dbResponse
                                         select new Locations
                                         {
                                             CityId = o.CityId,
                                             CountryId = o.CountryId??0,
                                             StateId = o.Stateid??0,
                                             StateName = o.Statename,
                                             CountryName = o.CountryName,
                                             Status = o.status,
                                             Name = o.Cityname,
                                             Code = o.Code
                                         }).ToList();
            return locations;
        }
        public List<TalkToTreatService.Entities.Service> GetServices()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetServices();

            List<TalkToTreatService.Entities.Service> locations = (from o in dbResponse
                                                                   select new TalkToTreatService.Entities.Service
                                                                   {
                                                                       Id = o.Id,
                                                                       Name = o.Name,
                                                                       ShortDescription = o.ShortDescription,
                                                                       Image = o.Image,
                                                                       Description = o.Description,
                                                                       IsActive = o.IsActive
                                                                   }).ToList();
            return locations;
        }

        public List<TreatmentPackage> GetTreatmentsByFilter(string country, string state, string city, string postCode, string hospitalId, string treatmentId, decimal? minCost, decimal? maxCost, string searchText)
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetTreatments(country, state, city, postCode, hospitalId, treatmentId, minCost, maxCost, searchText);



            List<TreatmentPackage> treatmentPackages = (from o in dbResponse
                                                        select new TreatmentPackage
                                                        {
                                                            Id = o.Id,
                                                            City = o.City,
                                                            State = o.State,
                                                            Country = o.Country,
                                                            HospitalIconImage = o.HospitalIconImage,
                                                            HospitalId = o.HospitalId,
                                                            HospitalImage = o.HospitalImage,
                                                            IsActive = o.IsActive,
                                                            HospitalName = o.HospitalName,
                                                            MaxCost = Convert.ToDecimal(o.MaxCost),
                                                            MinCost = Convert.ToDecimal(o.MinCost),
                                                            PostCode = o.PostCode,
                                                            TreatmentId = o.TreatmentId,
                                                            TreatmentImage = o.TreatmentImage,
                                                            TreatmentName = o.TreatmentName
                                                        }).ToList();
            return treatmentPackages;
        }

        public List<Testimonial> GetTestimonial()
        {
            var dataContextFactory = new DataContextFactory();
            masterContext = dataContextFactory.TalktoTreatDataContext();
            var dbResponse = masterContext.GetTestimonial();

            List<Testimonial> testimonial = (from o in dbResponse
                                             select new Testimonial
                                             {
                                                 ID = o.ID,
                                                 Name = o.Name,
                                                 Treatment = o.Treatment,
                                                 City = o.City,
                                                 Description = o.Description,
                                                 Image = o.Image,
                                                 IsActive = o.IsActive.GetValueOrDefault()
                                             }).ToList();
            return testimonial;
        }

        #endregion

       

        #region



        #endregion
    }
}
