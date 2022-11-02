using Service.MasterData;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Filters;
using TalkToTreat.Models; 
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace TalkToTreat.Controllers
{
    [AdminUserAuthenticationFilter]
    public class ManageEnquiryController : Controller
    {
        private readonly IDoctorsService _doctorservice;
        Treatment _treatment = new Treatment();
        DoctorsService _doctorService = new DoctorsService();
        HospitalService _hospitalService = new HospitalService();
        MasterService _masterService = new MasterService();

        public ManageEnquiryController()
        {
            _doctorservice = DependencyResolver.Current.GetService<IDoctorsService>();

        }  
        public ActionResult EnquiryList()
        {
            int pagenum = 0;
            int totalRecords = 0;
            string Searchvalue = "";
            if (Request["pagenum"] != null)
            {
                pagenum = Convert.ToInt32(Request["pagenum"]);
                TempData["pagenum"] = pagenum;
            }
            if (String.IsNullOrEmpty(Request["Searchvalue"]) == false)
            {
                Searchvalue = Request["Searchvalue"].ToString();
                ViewBag.Searchvalue = Searchvalue;
            }
            Quote _quote = new Quote();
            List<Quote> list = new List<Quote>();
            object[] obj = _quote.GetQuoteList(pagenum, Searchvalue);

            list = (List<Quote>)obj[0];
            if (list != null && list.Any())
            {
                totalRecords = Convert.ToInt32(list[0].TotalRecord);
            }
            if (pagenum >= 0 && pagenum <= 9)
            {
                ViewBag.PreviousData = 0;
                ViewBag.NextData = 10;
            }
            else
            {
                ViewBag.PreviousData = Convert.ToInt32(Request["Previous"]);
                ViewBag.NextData = Convert.ToInt32(Request["Next"]);
            }
            if (Request["PreviousData"] != null)
            {
                ViewBag.PreviousData = Convert.ToInt32(Request["PreviousData"]) - 10;
                ViewBag.NextData = Convert.ToInt32(Request["PreviousData"]);
            }
            if (Request["NextData"] != null)
            {
                ViewBag.PreviousData = Convert.ToInt32(Request["NextData"]);
                ViewBag.NextData = Convert.ToInt32(Request["NextData"]) + 10;
            }
            ViewBag.totalRecords = totalRecords;
            ViewBag.MyPageNumber = pagenum;
            return View(list);
        }
        public ActionResult GeneratePDF()
        {
            int pagenum = 0;
            string DepartmentName = null;

            if (TempData["MyPageNumber"] != null)
            {
                pagenum = Convert.ToInt32(TempData["MyPageNumber"]);
            }

            if (TempData["Name"] != null)
            {
                DepartmentName = TempData["Name"].ToString();
            }
            Appointment _IssuedReport = new Appointment();
            List<Appointment> list = new List<Appointment>();
            object[] obj = _IssuedReport.getdatapdf(pagenum, DepartmentName, null, null);
            list = (List<Appointment>)obj[0];
            TempData["resultdata"] = list;
            DownLoadPDF();
            var serverFilename = "~/pdf/result.pdf";
            Response.Redirect(serverFilename);
            return RedirectToAction("Index");
        }

        public void DownLoadPDF()
        {
            //server folder path which is stored your PDF documents       
            List<Appointment> list = new List<Appointment>();
            if (TempData.ContainsKey("resultdata"))
            {
                list = TempData["resultdata"] as List<Appointment>;
                TempData.Keep("resultdata");
            }
            string path = Server.MapPath("~/pdf");
            string filename = path + "/result.pdf";
            //Create new PDF document

            Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
            try
            {
                Type t = typeof(Appointment);
                var fieldcount = t.GetProperties().Count();

                PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 550f;
                //fix the absolute width of the table 
                table.LockedWidth = true;
                //relative col widths in proportions - 1/3 and 2/3 
                float[] widths = new float[] { 3f, 1f, 3f, 3f, 2f };
                table.SetWidths(widths);
                table.HorizontalAlignment = 1;
                //leave a gap before and after the table 
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                if (list != null && list.Any())
                {
                    PdfPCell cell = new PdfPCell(new Paragraph("Name", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, BaseColor.BLUE)));
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("EmailId", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, BaseColor.BLUE)));
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("MobileNo", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, BaseColor.BLUE)));
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("DoctorName", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, BaseColor.BLUE)));
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("AppointMentDate", new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD, BaseColor.BLUE)));
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    //int i = 1;
                    foreach (var item in list)
                    {
                        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL);
                        table.AddCell(new PdfPCell(new Phrase(item.FullName.ToString(), fontH1)));
                        table.AddCell(new PdfPCell(new Phrase(item.EmailId.ToString(), fontH1)));
                        table.AddCell(new PdfPCell(new Phrase(item.MobileNo.ToString(), fontH1)));
                        table.AddCell(new PdfPCell(new Phrase(item.DoctorName.ToString(), fontH1)));
                        table.AddCell(new PdfPCell(new Phrase(item.AppointMentDate.ToString(), fontH1)));

                        //table.AddCell(StringExtention.StripHTML(item.ProductName));
                        //table.AddCell(StringExtention.StripHTML(item.Item_Quantity));
                        //table.AddCell(StringExtention.StripHTML(item.IssuedEmpName));
                        //table.AddCell(StringExtention.StripHTML(item.Collected_By));
                        //table.AddCell(StringExtention.StripHTML(item.IssuingDate));

                    }
                }
                Paragraph para = new Paragraph("Appointment Report", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLUE));
                para.Alignment = Element.ALIGN_CENTER;
                document.Open();
                document.Add(para);
                document.Add(table);
            }
            catch (Exception ex)
            {
                document.Close();
            }
            finally
            {
                document.Close();
            }
        }

        public ActionResult Delete(Guid id)
        {
            string success = null;
            try
            {
                TalkToTreat.Models.Quote model = new Models.Quote();
                model.ID = id;
                // TODO: Add delete logic here
                var returnbyproc = model.Delete();
                if (returnbyproc > 0)
                {
                    success = "Enquiry Deleted successfully !!!";
                }
                return (RedirectToAction("EnquiryList", new { @result = "Success", UserValid = success }));
            }
            catch
            {
                return (RedirectToAction("EnquiryList", new { @result = "Failed", UserValid = success }));
            }
        }

        public ActionResult Edit(Guid id)
        {
            string success = null;
            try
            {

                List<TalkToTreat.Models.Hospital> hospitals = new List<TalkToTreat.Models.Hospital>();
                var HospialList = _hospitalService.GetAllHospitals();
                foreach (var item in HospialList)
                {
                    TalkToTreat.Models.Hospital hospital = new TalkToTreat.Models.Hospital();
                    hospital.Code = item.Code;
                    hospital.Name = item.Name;
                    hospital.Id = item.Id;
                    hospital.City = item.City;
                    hospital.Image = item.Image;
                    hospital.IconImage = item.IconImage;
                    hospitals.Add(hospital);
                }
                ViewBag.Hospitals = hospitals;
                var DistinctCities = hospitals.Select(x => x.City).Distinct().ToList();
                ViewBag.DistinctCities = DistinctCities;

                List<Doctors> doctors = new List<Doctors>();
                var doctorsList = _doctorService.GetAllDoctors();
                foreach (var item in doctorsList)
                {
                    Doctors doc = new Doctors();
                    doc.Code = item.Code;
                    doc.Cost = item.Cost;
                    doc.Name = item.Name;
                    doc.Image = item.Image;
                    doc.Description = item.Description;
                    doc.Id = item.Id;
                    doc.Designation = item.Designation;
                    doc.Department = item.Department;
                    doc.City = item.City;
                    doctors.Add(doc);
                }
                ViewBag.Doctors = doctors.ToList();
                //ViewBag.Doctors = doctors.Take(7).ToList();

                ViewBag.Treatments = _treatment.GetTreatmentListMaterHomePage(String.Empty);

                List<Testimonial> testimonials = new List<Testimonial>();
                var testimonialList = _masterService.GetTestimonial();
                foreach (var item in testimonialList)
                {
                    Testimonial testimonial = new Testimonial();
                    testimonial.ID = item.ID;
                    testimonial.Name = item.Name;
                    testimonial.Treatment = item.Treatment;
                    testimonial.City = item.City;
                    testimonial.Description = item.Description;
                    testimonial.IsActive = item.IsActive;
                    testimonial.Image = item.Image;
                    testimonials.Add(testimonial);
                }
                ViewBag.Testimonials = testimonials;

                ViewBag.CountryWiseCities = GetCountryWiseCities();
                ViewBag.Services = GetServices();

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

                TalkToTreat.Models.Quote model = new Models.Quote();
                var returnbyproc = model.GetQuoteDetail(id);
                return View("~/Views/ManageEnquiry/Create.cshtml", returnbyproc);
            }
            catch
            {
                return (RedirectToAction("EnquiryList", new { @result = "Failed", UserValid = success }));
            }
        }

        private List<CountryWiseCities> GetCountryWiseCities()
        {
            var result = new List<CountryWiseCities>();

            var hospitals = _hospitalService.GetAllHospitalsHomePage();
            var countries = hospitals.Select(x => x.Country).Distinct().ToList();
            foreach (string country in countries)
            {
                var cities = hospitals.Where(x => x.Country == country).Select(y => y.City).Distinct().ToList();
                var cityWiseHospitals = new List<CityWiseHospitals>();

                foreach (string city in cities)
                {
                    var cityHospitals = hospitals.Where(x => x.City == city && x.Country == country).ToList();
                    cityWiseHospitals.Add(new CityWiseHospitals()
                    {
                        City = city,
                        Hospitals = cityHospitals.ConvertAll(x => new Hospital()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            City = x.City,
                            Image = x.Image,
                            IconImage = x.IconImage
                        })
                    });
                }
                result.Add(new CountryWiseCities() { Country = country, Cities = cityWiseHospitals });
            }
            return result;
        }

        private List<Services> GetServices()
        {
            var result = new List<Services>();

            var services = _masterService.GetServices().Where(x => x.IsActive).ToList();
            result = services.ConvertAll(x => new Services()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Image = x.Image,
                Code = x.Code,
                IsActive = x.IsActive
            });
            return result;
        }
    }
}