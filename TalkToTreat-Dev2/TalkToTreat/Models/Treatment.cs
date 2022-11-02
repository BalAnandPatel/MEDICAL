using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace TalkToTreat.Models
{
    public class Treatment
    {
        public Int32 Rn { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string ShortDescription { get; set; }
        public Guid SubId { get; set; }
        public string SubName { get; set; }
        public decimal MinCost { get; set; }


        public Guid ParentId { get; set; }       
        public decimal Cost { get; set; }
        
        public int TotalRecord { get; set; }
        public Treatment Parent { get; set; }
        public List<Treatment> Children { get; set; }

        //// added only of quote no any other usese
        //public Quote Quote { get; set; }

        public object[] GetTreatmentList(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllTreatmentList");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];

            List<Treatment> list = new List<Treatment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Treatment
                {
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString(),
                    Description = ResultTable.Rows[i]["Description"].ToString(),
                    Code = ResultTable.Rows[i]["Code"].ToString(),
                    IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"].ToString()),
                    //Cost = Convert.ToDecimal(ResultTable.Rows[i]["Cost"].ToString()),
                    Image = ResultTable.Rows[i]["Image"].ToString(),
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"])
                });
            }
            object[] Obj = new object[1];
            Obj[0] = list;
            return Obj;
        }

        public object[] GetTreatmentListMater(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllTreatmentListMaster");
            _search.AddParameter("@SearchValue", Searchvalue);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];

            List<Treatment> list = new List<Treatment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Treatment
                {
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString(),
                    Description = ResultTable.Rows[i]["Description"].ToString(),
                    Code = ResultTable.Rows[i]["Code"].ToString(),
                    IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"].ToString()),
                    Cost = Convert.ToDecimal(ResultTable.Rows[i]["Cost"].ToString()),
                    Image = ResultTable.Rows[i]["Image"].ToString(),
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),

                    SubId = Guid.Parse(ResultTable.Rows[i]["SubId"].ToString()),
                    SubName = ResultTable.Rows[i]["SubName"].ToString(),

                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"])
                });
            }
            object[] Obj = new object[1];
            Obj[0] = list;
            return Obj;
        }

        public List<Treatment> GetTreatmentListMaterHomePage(string Searchvalue)
        {
            search _search = new search("GetAllTreatmentListMaster");
            _search.AddParameter("@SearchValue", Searchvalue);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];

            List<Treatment> list = new List<Treatment>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {

                var Rn = Convert.ToInt32(ResultTable.Rows[i]["Rn"].ToString());
                var Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString());
                var Name = ResultTable.Rows[i]["Name"].ToString();
                var Code = ResultTable.Rows[i]["Code"].ToString();
                var Description = ResultTable.Rows[i]["Description"].ToString();
                var Image = ResultTable.Rows[i]["Image"].ToString();
                var IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"].ToString());
                var ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString();
                var SubId = Guid.Parse(ResultTable.Rows[i]["SubId"].ToString());
                var SubName = ResultTable.Rows[i]["SubName"].ToString();
                var MinCost = Convert.ToDecimal(ResultTable.Rows[i]["MinCost"].ToString());


                list.Add(new Treatment
                {
                    Rn = Convert.ToInt32(ResultTable.Rows[i]["Rn"].ToString()),
                    Id = Guid.Parse(ResultTable.Rows[i]["Id"].ToString()),
                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    Code = ResultTable.Rows[i]["Code"].ToString(),
                    Description = ResultTable.Rows[i]["Description"].ToString(),
                    Image = ResultTable.Rows[i]["Image"].ToString(),
                    IsActive = Convert.ToBoolean(ResultTable.Rows[i]["IsActive"].ToString()),
                    ShortDescription = ResultTable.Rows[i]["ShortDescription"].ToString(),
                    SubId= Guid.Parse(ResultTable.Rows[i]["SubId"].ToString()),
                    SubName = ResultTable.Rows[i]["SubName"].ToString(),
                    MinCost = Convert.ToDecimal(ResultTable.Rows[i]["MinCost"].ToString())
                });
            }
            return list;
        }

        public int Save()
        {
            int returnbyproc = 0;
            try
            {
                var treatmentJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertTreatment_New");
                _mysave.AddParameter("@TreatmentId", this.Id.ToString());
                _mysave.AddParameter("@TreatmentJson", treatmentJson);
                _mysave.AddParameter("@SavedBy", "");
                _mysave.AddParameter("@Name", this.Name);
                _mysave.AddParameter("@IsActive", this.IsActive.ToString());
                _mysave.AddParameter("@ShortDescription", this.ShortDescription);
                _mysave.AddParameter("@Description", this.Description);
                _mysave.AddParameter("@Image", this.Image);
                _mysave.AddParameter("@Cost", this.Cost.ToString());
                _mysave.AddParameter("@ParentId", this.ParentId.ToString());

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
                var treatmentJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertTreatment");
                _mysave.AddParameter("@TreatmentId", this.Id.ToString());
                _mysave.AddParameter("@TreatmentJson", treatmentJson);
                _mysave.AddParameter("@SavedBy", "");
                returnbyproc = _mysave.ExecuteSave();

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
                _mysave.AddParameter("@ObjectType", 2);
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