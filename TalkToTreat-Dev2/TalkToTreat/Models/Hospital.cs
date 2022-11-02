using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TalkToTreat.Models
{
    public class Hospital
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        
        public string State { get; set; }
        public string Country { get; set; }
        
        public string Code { get; set; }
        public Decimal Cost { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfbeds { get; set; }
        public string EstablishmentYesr { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public string PostCode { get; set; }
        public string IconImage { get; set; } 
        public string Infrastructure { get; set; }
        public string Spacialities { get; set; }
        public string CustomNo { get; set; } 
        public int TotalRecord { get; set; }

        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        public object[] GetHospitalList(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllHospitalList");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];
            //DataTable RecordsCountTable = ds.Tables[1];
            //String TotalRecords = RecordsCountTable.Rows[0][0].ToString();

            List<Hospital> list = new List<Hospital>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Hospital
                {
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    Description = ResultTable.Rows[i]["Description"].ToString(),
                    ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString(),
                    Country = ResultTable.Rows[i]["Country"].ToString(),
                    Image = ResultTable.Rows[i]["Image"].ToString(), 
                    Code = ResultTable.Rows[i]["Code"].ToString(),
                    City = ResultTable.Rows[i]["City"].ToString(),
                    PhoneNumber = ResultTable.Rows[i]["PhoneNumber"].ToString(),
                    IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"].ToString()),
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



        public int Save()
        {
            int returnbyproc = 0;

            try
            {
                var hospitalJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertHospital_20220424");
                _mysave.AddParameter("@HospitalId", this.Id.ToString());
                _mysave.AddParameter("@HospitalJson", hospitalJson);
                _mysave.AddParameter("@SavedBy", ""); 
                _mysave.AddParameter("@Name", this.Name);
                _mysave.AddParameter("@IsActive", this.IsActive.ToString());
                _mysave.AddParameter("@ShortDescription", this.ShortDescription);
                _mysave.AddParameter("@Description", this.Description);
                _mysave.AddParameter("@City", this.City);
                _mysave.AddParameter("@State", this.State);
                _mysave.AddParameter("@Country", this.Country);
                _mysave.AddParameter("@Code", this.Code);
                _mysave.AddParameter("@PhoneNumber", this.PhoneNumber);
                _mysave.AddParameter("@MobileNumber", this.PhoneNumber);
                _mysave.AddParameter("@Image", this.Image);
                _mysave.AddParameter("@CustomNo", this.Code);
                _mysave.AddParameter("@noofbeds", this.NumberOfbeds);
                _mysave.AddParameter("@estYear", this.EstablishmentYesr);
                _mysave.AddParameter("@HositalType", this.Type);
                 _mysave.AddParameter("@Spacialities", this.Spacialities);
                _mysave.AddParameter("@Icon", this.IconImage);
                _mysave.AddParameter("@Infrastructure", this.Infrastructure);
                _mysave.AddParameter("@Regno", this.CustomNo);
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
                var hospitalJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertHospital");
                _mysave.AddParameter("@HospitalId", this.Id.ToString());
                _mysave.AddParameter("@HospitalJson", hospitalJson);
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
                _Mysave _mysave = new _Mysave("ProcDeleteObject");
                _mysave.AddParameter("@ObjectId", Id.ToString());
                _mysave.AddParameter("@ObjectType", 3);
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