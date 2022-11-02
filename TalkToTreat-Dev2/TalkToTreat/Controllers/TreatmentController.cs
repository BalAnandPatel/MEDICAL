using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using TalkToTreat.Models;
using System.Web.Mvc;
using TalkToTreatService.Users;

namespace TalkToTreat.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly IUserService _userService;
        MasterService _masterService = new MasterService();
        HospitalService _hospitalService = new HospitalService();
        public ActionResult Index()
        {
            List<TalkToTreat.Models.Treatment> treatments = new List<TalkToTreat.Models.Treatment>();
            var treatmentsList = _masterService.GetTreatments();
            foreach (var item in treatmentsList)
            {
                TalkToTreat.Models.Treatment treatment = new TalkToTreat.Models.Treatment();
                treatment.Code = item.Code;
                treatment.Cost = item.Cost;
                treatment.Name = item.Name;
                treatment.Description = item.Description;
                treatment.Image = item.Image;
                treatment.Id = item.Id;
                treatments.Add(treatment);
            }
            return View(treatments);
        }

        public ActionResult Detail(string id)
        {
            try
            {

                #region quoteData               

                //All Cities
                var AllCities = _masterService.GetLocations().OrderBy(a => a.Name);
                List<Locations> AllCitiesEntity = new List<Locations>();
                foreach (var Cityitem in AllCities)
                {
                    Locations loc = new Locations();
                    loc.CityId = Cityitem.CityId;
                    loc.Name = Cityitem.Name;
                    loc.CountryName = Cityitem.CountryName;
                    loc.CountryId = Cityitem.CountryId;
                    loc.StateId = Cityitem.StateId;
                    loc.StateName = Cityitem.StateName;
                    loc.Code = Cityitem.Code;
                    AllCitiesEntity.Add(loc);
                }
                ViewBag.AllCities = AllCitiesEntity;

                //Treatments
                ViewBag.TreatmentsForGetQuote = _masterService.GetTreatments();

                #endregion

                Guid treatmentId = Guid.Parse(id);
                var treatmentsList = _masterService.GetTreatments();
                var treatmentsDetail = treatmentsList.Where(a => a.Id == treatmentId).FirstOrDefault();
                TalkToTreat.Models.Treatment treatment = new TalkToTreat.Models.Treatment();
                treatment.Code = treatmentsDetail.Code ?? "";
                treatment.Cost = treatmentsDetail.Cost;
                treatment.Name = treatmentsDetail.Name;

                treatment.Description = treatmentsDetail.Description;

                //treatment.Description = string.Join("_", treatmentsDetail.Description.Split( System.IO.Path.GetInvalidFileNameChars()));

                //treatment.Image = treatmentsDetail.Image;
                if (!string.IsNullOrEmpty(treatmentsDetail.Image))
                {
                    treatment.Image = string.Join("", treatmentsDetail.Image.Split(System.IO.Path.GetInvalidFileNameChars()));
                }

                treatment.Id = treatmentsDetail.Id;
                treatment.ShortDescription = treatmentsDetail.ShortDescription;
                treatment.ParentId = treatmentsDetail.ParentId;

                string viewName = "ParentDetail";
                if (treatmentsDetail.ParentId != Guid.Empty)
                {
                    var parent = treatmentsList.Where(x => x.Id == treatment.ParentId).Single();
                    treatment.Parent = new Treatment()
                    {
                        Id = parent.Id,
                        ParentId = parent.ParentId,
                        Name = parent.Name,
                        Code = parent.Code,
                        Cost = parent.Cost,
                        Image = parent.Image,
                        ShortDescription = parent.ShortDescription,
                        Description = parent.Description
                    };
                    viewName = "ChildDetail";
                }
                else
                {
                    var children = treatmentsList.Where(x => x.ParentId == treatment.Id).ToList();
                    if (children.Count > 0)
                    {
                        treatment.Children = children.ConvertAll(x => new Treatment()
                        {
                            Id = x.Id,
                            ParentId = x.ParentId,
                            Name = x.Name,
                            Code = x.Code,
                            Cost = x.Cost,
                            Image = x.Image,
                            ShortDescription = x.ShortDescription,
                            Description = x.Description
                        });
                    }
                }

                ViewBag.Treatments = GetTreatmentList(treatmentsList);
                return View(viewName, treatment);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<Treatment> GetTreatmentList(List<TalkToTreatService.Entities.Treatment> treatmentsList)
        {
            List<Treatment> treatments = new List<Treatment>();
            treatmentsList = treatmentsList.Where(a => a.IsActive && !string.IsNullOrEmpty(a.Image)).ToList();
            foreach (var item in treatmentsList)
            {
                treatments.Add(new Treatment()
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    Name = item.Name,
                    Code = item.Code,
                    Cost = item.Cost,
                    Image = item.Image,
                    ShortDescription = item.ShortDescription,
                    Description = item.Description
                });
            }

            return treatments;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Enquiry")]
        public ActionResult Enquiry(Registration _registration)
        {
            string invalid = null;
            try
            {
                var user = new TalkToTreatService.Entities.User
                {
                    Email = _registration.EmailId,
                    Name = _registration.FullName,
                    PatientAge = _registration.PatientAge ?? 0,
                    Remark = _registration.Remark
                };

                var resp = _userService.InsertEnquiry(user);
                if (resp.IsValid)
                {
                    return (RedirectToAction("Confirmation", "Home"));
                }
            }
            catch (Exception ex)
            {
                invalid = "Something went wrong , Please try again";
                return (RedirectToAction("index", "Home", new { UserValid = invalid }));
            }
            return View();
        }


        private List<CountryWiseTreatmentPackages> GetCountryTreatmentPackages()
        {
            var result = new List<CountryWiseTreatmentPackages>();

            var packageList = _masterService.GetTreatmentsByFilter(null, null, null, null, null, null, null, null, null);

            var countries = packageList.Select(x => x.Country).Distinct().ToList();
            foreach (string country in countries)
            {
                var countryWisePackages = packageList.Where(x => x.Country == country).ToList();

                var cities = countryWisePackages.Select(y => y.City).Distinct().ToList();
                var cityWiseTreatmentPackages = new List<CityWiseTreatmentPackages>();

                foreach (string city in cities)
                {
                    var cityWisePackages = countryWisePackages.Where(x => x.City == city).ToList();
                    var treatmentIds = countryWisePackages.Select(x => x.TreatmentId).Distinct().ToList();


                    Dictionary<string, List<TreatmentPackage>> packages = new Dictionary<string, List<TreatmentPackage>>();
                    foreach (var treatmentId in treatmentIds)
                    {
                        var treatments = cityWisePackages.Where(x => x.TreatmentId == treatmentId).ToList();
                        packages.Add(treatmentId.ToString(), 
                            treatments.ConvertAll<TreatmentPackage>((Converter<TalkToTreatService.Entities.TreatmentPackage, TreatmentPackage>)(x => new TreatmentPackage()
                        {
                            Id = x.Id,
                            City = x.City,
                            State = x.State,
                            Country = x.Country,
                            HospitalIconImage = x.HospitalIconImage,
                            HospitalId = x.HospitalId,
                            HospitalImage = x.HospitalImage,
                            IsActive = x.IsActive,
                            HospitalName = x.HospitalName,
                            MaxCost = x.MaxCost,
                            MinCost = x.MinCost,
                            PostCode = x.PostCode,
                            TreatmentId = x.TreatmentId,
                            TreatmentImage = x.TreatmentImage,
                            TreatmentName = x.TreatmentName
                        })));
                    }
                    cityWiseTreatmentPackages.Add(new CityWiseTreatmentPackages() { City = city, TreatmentPackages = packages });
                }
                result.Add(new CountryWiseTreatmentPackages() { Country = country, Cities = cityWiseTreatmentPackages });
            }
            return result;
        }

        [ChildActionOnly]
        public ActionResult MegaMenu()
        {
            Menu menu = GetTreatmentsMenu();
            return PartialView(menu);
        }

        [ChildActionOnly]
        public ActionResult VerticalMenu(string id, string parentId)
        {
            Menu menu = GetTreatmentsMenu(id, parentId);
            return PartialView(menu);
        }

        private Menu GetTreatmentsMenu(string id = null, string parentId = null)
        {
            var menu = new Menu() { Title = "Treatments", Description = "Treatments", Items = new List<Menu>() };
            var treatments = _masterService.GetTreatments();
            if (treatments != null)
            {
                treatments = treatments.Where(x => x.IsActive).ToList();
                var parents = treatments.Where(x => x.ParentId == Guid.Empty).ToList();
                if (parents != null)
                {
                    foreach (var parent in parents)
                    {
                        List<Menu> submenus = new List<Menu>();
                        var children = treatments.Where(x => x.ParentId == parent.Id).ToList();
                        if (children != null)
                        {
                            submenus = children.ConvertAll(x => new Menu() { Id = x.Id.ToString(), Title = x.Name, Description = x.Name, IsActive = x.Id.ToString() == id });
                        }
                        menu.Items.Add(new Menu() { Id = parent.Id.ToString(), Title = parent.Name, Description = parent.Name, IsActive = parent.Id.ToString() == id, IsExpanded = parent.Id.ToString() == parentId, Items = submenus });
                    }
                }
            }

            return menu;
        }

        [ChildActionOnly]
        public ActionResult TreatmentPackages(string treatment)
        {
            ViewBag.SelectedTreatment = treatment;
            return PartialView(GetCountryTreatmentPackages());
        }
       
    }
}