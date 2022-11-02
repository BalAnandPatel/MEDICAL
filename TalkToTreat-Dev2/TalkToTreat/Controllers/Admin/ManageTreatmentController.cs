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
using AutoMapper;

namespace TalkToTreat.Controllers
{
    [AdminUserAuthenticationFilter]
    public class ManageTreatmentController : Controller
    {

        MasterService _masterService = new MasterService();
        public ManageTreatmentController()
        { 
        }
         
        public ActionResult TreatmentList()
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
            Treatment _treatment = new Treatment();
            List<Treatment> list = new List<Treatment>();
            object[] obj = _treatment.GetTreatmentList(pagenum, Searchvalue);
            list = (List<Treatment>)obj[0];
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

        public ActionResult TreatmentListMaster()
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
            Treatment _treatment = new Treatment();
            List<Treatment> list = new List<Treatment>();
            object[] obj = _treatment.GetTreatmentList(pagenum, Searchvalue);
            list = (List<Treatment>)obj[0];
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

            ViewBag.IsTreatMentMaster = true;

            return View("TreatmentList", list);
        }

        public ActionResult Create()
        {
            Treatment model = new Treatment();
            DropdownMaster _Master = new DropdownMaster();
            List<SelectListItem> hospitalList = _Master.GetDropdownMaster(1);
            List<SelectListItem> DepartmentList = _Master.GetDropdownMaster(2);
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
            List<SelectListItem> TreatMentMaster = _Master.GetDropdownMaster(4);

            ViewBag.HospitalList = hospitalList;
            ViewBag.DepartmentList = DepartmentList;
            ViewBag.LocationList = LocationList;
            ViewBag.TreatMentMaster = TreatMentMaster;

            ViewBag.IsTreatMentMaster = false;

            return View(model);
        }

        public ActionResult CreateMaster()
        {
            Treatment model = new Treatment();
            DropdownMaster _Master = new DropdownMaster();
            List<SelectListItem> hospitalList = _Master.GetDropdownMaster(1);
            List<SelectListItem> DepartmentList = _Master.GetDropdownMaster(2);
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
            List<SelectListItem> TreatMentMaster = _Master.GetDropdownMaster(4);

            ViewBag.HospitalList = hospitalList;
            ViewBag.DepartmentList = DepartmentList;
            ViewBag.LocationList = LocationList;
            ViewBag.TreatMentMaster = TreatMentMaster;

            ViewBag.IsTreatMentMaster = true;

            return View("Create", model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateTreatment(Treatment model, HttpPostedFileBase file)
        {
            string success = null;
           
            try
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName);
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + model.Id + ext;
                    var path = Path.Combine(Server.MapPath("~/Assets/Images/Treatment/"), myfile);
                    file.SaveAs(path);
                    model.Image = myfile;
                }
                var returnbyproc=model.Save();
                

                if (returnbyproc > 0)
                {
                    if (model.Id == Guid.Empty)
                    {
                        success = "Treatment created successfully !!!";
                    }
                    else
                    {
                        success = "Treatment Updated successfully !!!";
                    }
                }

                if (model.ParentId != Guid.Empty)
                {
                    return (RedirectToAction("TreatmentList", new { @result = "Success", UserValid = success }));
                }
                else
                {
                    return (RedirectToAction("TreatmentListMaster", new { @result = "Success", UserValid = success }));
                }
            }
            catch (Exception ex)
            {
                success = "Some error accurred. Please try later !!!";
                return (RedirectToAction("Create", new { @result = "Success", UserValid = success }));
            }
        }
        public ActionResult Edit(Guid id)
        {
            DropdownMaster _Master = new DropdownMaster();
            List<SelectListItem> hospitalList = _Master.GetDropdownMaster(1);
            List<SelectListItem> DepartmentList = _Master.GetDropdownMaster(2);
            List<SelectListItem> LocationList = _Master.GetDropdownMaster(3);
            List<SelectListItem> TreatMentMaster = _Master.GetDropdownMaster(4);

            ViewBag.HospitalList = hospitalList;
            ViewBag.DepartmentList = DepartmentList;
            ViewBag.LocationList = LocationList;
            ViewBag.TreatMentMaster = TreatMentMaster;

            List<Treatment> list = new List<Treatment>();
            var treatmentList = _masterService.GetTreatments();

            var treatmentDetail = treatmentList.Where(a => a.Id == id).FirstOrDefault();

            Treatment treatment = new Treatment();
            treatment.ParentId = treatmentDetail.ParentId;
            treatment.Code = treatmentDetail.Code;
            treatment.Name = treatmentDetail.Name;
            treatment.Image = treatmentDetail.Image;
            treatment.Description = treatmentDetail.Description;
            treatment.Cost = treatmentDetail.Cost;
            treatment.Id = treatmentDetail.Id;
            treatment.ShortDescription = treatmentDetail.ShortDescription;
            treatment.IsActive = treatmentDetail.IsActive;

            return View("~/Views/ManageTreatment/Create.cshtml", treatment);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Treatment treatment)
        {
            try
            {
                var doctorEntity = Mapper.Map<TalkToTreatService.Entities.Treatment>(treatment);
               // _masterService.UpsertDoctor(doctorEntity);
                return RedirectToAction("Edit", new { id = treatment.Id });
            }
            catch
            {
                return View();
            }
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
                TalkToTreat.Models.Treatment model = new Models.Treatment();
                model.Id = id;
                // TODO: Add delete logic here
                var returnbyproc = model.Delete();
                if (returnbyproc > 0)
                {
                    success = "Treatment Deleted successfully !!!";
                }
                return (RedirectToAction("TreatmentList", new { @result = "Success", UserValid = success }));
            }
            catch
            {
                return (RedirectToAction("TreatmentList", new { @result = "Failed", UserValid = success }));
            }
        }
    }
}