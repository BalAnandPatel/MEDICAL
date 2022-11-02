using BaseApiApp.Framework.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TalkToTreat.Models
{
    public class Doctors
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }
        public string Hospital { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }        
        public string Code { get; set; }
        public Decimal Cost { get; set; }
        public bool IsActive { get; set; }
        public bool Gender { get; set; }
        public int YearsofExp { get; set; }
        public string Award { get; set; }
        
        public string MobileNumber { get; set; }
        public  string EmailId { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public int TotalRecord { get; set; }

        public Guid HospitalId { get; set; }

        public string Degree { get; set; }
        public string Experience { get; set; }
        public string AdditionalInfo { get; set; }

        public string SavedBy { get; set; }
       

        public object[] GetDoctorList(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllDoctorsList");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();

            List<Doctors> list = new List<Doctors>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Doctors
                {
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    Department = ResultTable.Rows[i]["Department"].ToString(),
                    Designation = ResultTable.Rows[i]["Designation"].ToString(),
                    Hospital = ResultTable.Rows[i]["Hospital"].ToString(),
                    Image = ResultTable.Rows[i]["Image"].ToString(),
                    City = ResultTable.Rows[i]["City"].ToString(), 
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"])
                });
            }

            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            //Obj[0] = TotalRecords;
            Obj[0] = list;
            return Obj;
        }
        public Doctors GetDoctorById(Guid Id)
        {
            search _search = new search("GetDoctorById");
            _search.AddParameter("@Id", Id.ToString());
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();

            Doctors list = new Doctors();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list = new Doctors
                {
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    Department = ResultTable.Rows[i]["Department"].ToString(),
                    Designation = ResultTable.Rows[i]["Designation"].ToString(),
                    
                    Hospital = ResultTable.Rows[i]["Hospital"].ToString(),

                    Image = ResultTable.Rows[i]["Image"].ToString(),
                    City = ResultTable.Rows[i]["City"].ToString(),
                    Username = ResultTable.Rows[i]["Username"].ToString(),
                    EmailId = ResultTable.Rows[i]["Username"].ToString(),
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    Award = ResultTable.Rows[i]["Award"].ToString(),
                    ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString(),
                    Description = ResultTable.Rows[i]["Description"].ToString(),
                    MobileNumber = ResultTable.Rows[i]["MobileNumber"].ToString(),
                    Cost =Convert.ToDecimal(ResultTable.Rows[i]["Cost"]),
                    
                    Degree =  ResultTable.Rows[i]["Degree"].ToString(),
                    Experience = ResultTable.Rows[i]["Experience"].ToString(),
                    AdditionalInfo = ResultTable.Rows[i]["AdditionalInfo"].ToString(),

                    Password = ResultTable.Rows[i]["Password"].ToString(),

                    IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"]),
                    Gender = Convert.ToBoolean(ResultTable.Rows[i]["Gender"]),
                    YearsofExp = Convert.ToInt32(ResultTable.Rows[i]["YearsofExp"]),
                };
            } 
            return list;
        }

        public int Save()
        {
            int returnbyproc = 0;
            try
            { 
                _Mysave _mysave = new _Mysave("procJsonUpsertDoctor_New");
                _mysave.AddParameter("@DoctorId", this.Id.ToString());
                _mysave.AddParameter("@Name", this.Name);
                _mysave.AddParameter("@Department", this.Department);
                _mysave.AddParameter("@Designation", this.Designation);
                _mysave.AddParameter("@ShortDescription", this.ShortDescription);
                _mysave.AddParameter("@Description", this.Description);

                _mysave.AddParameter("@MobileNumber", this.MobileNumber);
                _mysave.AddParameter("@SavedBy", SavedBy);
                _mysave.AddParameter("@IsActive", this.IsActive.ToString());
                _mysave.AddParameter("@Image", this.Image);

                _mysave.AddParameter("@Hospital", this.Hospital);
                _mysave.AddParameter("@YearsofExp", this.YearsofExp);
                _mysave.AddParameter("@Degree", this.Degree);

                _mysave.AddParameter("@EmailId", this.Username);
                _mysave.AddParameter("@Password", this.Password);

                _mysave.AddParameter("@AdditionalInfo", AdditionalInfo);
                _mysave.AddParameter("@gender", Gender?"1":"0");


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

        public int Update()
        {
            int returnbyproc = 0;
            try
            {
                string salt = string.Empty;
                var passowrd = PasswordHelper.Generate();
                if (!string.IsNullOrEmpty(MobileNumber))
                { 
                    Password = PasswordHelper.EncryptPassword(ref salt, MobileNumber);
                }

                var doctorJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertDoctor");
                _mysave.AddParameter("@DoctorId", this.Id.ToString());
                _mysave.AddParameter("@DoctorJson", doctorJson);
                _mysave.AddParameter("@SavedBy", "");

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

        public int Delete()
        {
            int returnbyproc = 0;

            try
            {
                _Mysave _mysave = new _Mysave("procDeleteObject");
                _mysave.AddParameter("@ObjectId", Id.ToString());
                _mysave.AddParameter("@ObjectType", 1);
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

    }
}