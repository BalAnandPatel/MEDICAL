using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using BaseApiApp.Entities.Enum;

namespace TalkToTreat.Models
{
    public class Appointment
    {
        public Guid  Id { get; set; }
        public Guid DoctorId { get; set; }
        public string AppointmentNumber { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }

        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string AppointMentType { get; set; }
        public int PatientAge { get; set; }
        public DateTime AppointMentDate { get; set; }
        public string AppointMentTime { get; set; }
        public DateTime Created { get; set; }

        public int TotalRecord { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsReplied { get; set; }
        public Guid EnquiryReplyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public bool IsCancel { get; set; }
        public bool CancelledBY { get; set; }
        public string CancelRegion { get; set; }

        public string Country { get; set; }
        public string GoogleMeetUrl { get; set; }

        public int Status { get; set; }
        public string cclist { get; set; }

        public string SiteUserId { get; set; }

        public object[] GetEnquiryList(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllEnquiry");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();

            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Appointment
                {
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    AppointMentType = ResultTable.Rows[i]["AppointMentType"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(), 
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),                    
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }

        public object[] GetAppointmentList(int? pagenum, string Searchvalue, DateTime? FromDate, DateTime? ToDate)
        {
            if(FromDate == null)
            {
                FromDate= DateTime.Today.AddYears(-1);
            }
            if (ToDate == null)
            {
                ToDate = DateTime.UtcNow.AddDays(5);
            }
            search _search = new search("GetAllAppointment_20220425");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            _search.AddParameter("@FromDate", FromDate);
            _search.AddParameter("@ToDate", ToDate);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();
             
            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {  
                list.Add(new Appointment
                {
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(), 
                    IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString()),
                    Body = ResultTable.Rows[i]["Body"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    DoctorId =Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate =Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    Created = Convert.ToDateTime(ResultTable.Rows[i]["Created"].ToString()),
                    PatientAge =Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                    IsCancel = ResultTable.Rows[i]["IsCancel"] == DBNull.Value ? false : Convert.ToBoolean(ResultTable.Rows[i]["IsCancel"].ToString()),
                    CancelRegion = ResultTable.Rows[i]["CancelRegion"].ToString(),
                    Status = ResultTable.Rows[i]["Status"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["Status"].ToString())
                }); 
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }
        public object[] getdatapdf(int? pagenum, string Searchvalue, DateTime? FromDate, DateTime? ToDate)
        {
            search _search = new search("GetAllAppointment_20220425");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            _search.AddParameter("@FromDate", FromDate);
            _search.AddParameter("@ToDate", ToDate);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();


            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            { 
                list.Add(new Appointment
                {
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(),
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"])
                }); 
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }
        public Appointment GetAppointmentDetail(Guid Id)
        {
            search _search = new search("GetAppointmentDetail");
            _search.AddParameter("@Id", Id.ToString());
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0]; 
            Appointment  list = new Appointment();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString());
                list.EmailId = ResultTable.Rows[i]["EmailId"].ToString();
                list.MobileNo = ResultTable.Rows[i]["MobileNo"].ToString();
                list.DoctorName = ResultTable.Rows[i]["DoctorName"].ToString();
                list.DoctorEmail = ResultTable.Rows[i]["DoctorEmail"].ToString();

                list.Gender = ResultTable.Rows[i]["Gender"].ToString();
                list.FullName = ResultTable.Rows[i]["FullName"].ToString();
                list.IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString());
                list.Body = ResultTable.Rows[i]["Body"].ToString();
                list.Subject = ResultTable.Rows[i]["Subject"].ToString();
                list.DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString());
                list.AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString());
                list.PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString());
                list.Status = Convert.ToInt32(ResultTable.Rows[i]["Status"] == DBNull.Value ? 0 : ResultTable.Rows[i]["Status"]);
                list.GoogleMeetUrl = ResultTable.Rows[i]["GoogleMeetUrl"].ToString();
                list.cclist = ResultTable.Rows[i]["cclist"].ToString();
                list.SiteUserId = ResultTable.Rows[i]["SiteUserId"].ToString();
            }
             
            return list;
        }

        public object[] GetAppointmentDetailByUser(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllAppointmentByUser");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0]; 
            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Appointment
                {
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(),
                    IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString()),
                    Body = ResultTable.Rows[i]["Body"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                    IsCancel = ResultTable.Rows[i]["IsCancel"] == DBNull.Value ? false : Convert.ToBoolean(ResultTable.Rows[i]["IsCancel"].ToString())
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }

        public object[] GetAppointmentDetailByUserFuture(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllAppointmentByUserFuture");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Appointment
                {
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(),
                    IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString()),
                    Body = ResultTable.Rows[i]["Body"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                    IsCancel = ResultTable.Rows[i]["IsCancel"] == DBNull.Value ? false : Convert.ToBoolean(ResultTable.Rows[i]["IsCancel"].ToString())
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }

        public int UpdateAppointment()
        {
            int returnbyproc = 0; 
            try
            {
                var doctorJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procUpsertEnquiryReply_New");
                _mysave.AddParameter("@AppointmentId", this.Id.ToString());
                _mysave.AddParameter("@AppointmentJson", doctorJson);
                _mysave.AddParameter("@SavedBy", "");
                _mysave.AddParameter("@EmailId", this.EmailId);
                _mysave.AddParameter("@Title", this.Subject);
                _mysave.AddParameter("@DoctorId", this.DoctorId.ToString());
                _mysave.AddParameter("@Body", this.Body);
                _mysave.AddParameter("@Status", this.Status);
                _mysave.AddParameter("@GoogleMeetUrl", this.GoogleMeetUrl);
                _mysave.AddParameter("@AppointMentDate", this.AppointMentDate);
                _mysave.AddParameter("@cclist", this.cclist);

                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return returnbyproc;
        }
        
        public int SaveCancelRequest(string Id , string CancelRegion, int CancelledBy)
        {
            int returnbyproc = 0;
            try
            {
                var doctorJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procUpdateCancelRequest");
                _mysave.AddParameter("@AppointmentId", Id.ToString());
                _mysave.AddParameter("@IsCancel", true.ToString());
                _mysave.AddParameter("@CancelledBy", CancelledBy.ToString());
                _mysave.AddParameter("@CancelRegion", CancelRegion);              

                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return returnbyproc;
        }

        public object[] GetAppointmentDetailByDoctor(int? pagenum, string DoctorId)
        {
            search _search = new search("GetAllAppointmentByDoctor");
            _search.AddParameter("@DoctorId", DoctorId);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Appointment
                {
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(),
                    IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString()),
                    
                    Body = ResultTable.Rows[i]["Body"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    GoogleMeetUrl = ResultTable.Rows[i]["GoogleMeetUrl"].ToString(),
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                    IsCancel = ResultTable.Rows[i]["IsCancel"] == DBNull.Value ? false : Convert.ToBoolean(ResultTable.Rows[i]["IsCancel"].ToString())
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }

        public object[] GetAppointmentDetailByDoctorFuture(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllAppointmentByDoctorFuture");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            List<Appointment> list = new List<Appointment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Appointment
                {
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    EmailId = ResultTable.Rows[i]["EmailId"].ToString(),
                    MobileNo = ResultTable.Rows[i]["MobileNo"].ToString(),
                    DoctorName = ResultTable.Rows[i]["DoctorName"].ToString(),
                    Gender = ResultTable.Rows[i]["Gender"].ToString(),
                    FullName = ResultTable.Rows[i]["FullName"].ToString(),
                    IsReplied = Convert.ToBoolean(ResultTable.Rows[i]["IsReplied"].ToString()),
                    Body = ResultTable.Rows[i]["Body"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    DoctorId = Guid.Parse(ResultTable.Rows[i]["DoctorId"].ToString()),
                    AppointMentDate = Convert.ToDateTime(ResultTable.Rows[i]["AppointMentDate"].ToString()),
                    PatientAge = Convert.ToInt32(ResultTable.Rows[i]["PatientAge"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                    IsCancel = ResultTable.Rows[i]["IsCancel"] == DBNull.Value ? false : Convert.ToBoolean(ResultTable.Rows[i]["IsCancel"].ToString())
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }

    }
   
}
