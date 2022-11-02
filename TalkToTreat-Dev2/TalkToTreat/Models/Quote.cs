using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using BaseApiApp.Entities.Enum;

namespace TalkToTreat.Models
{
    public class Quote
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }

        public Guid TreatmentId { get; set; }

        public string Subject { get; set; }
        public string Query { get; set; }

        public string TravelingForTreatmentTime { get; set; }

        public Guid SubTreatmentId { get; set; }

        public string ProcedureInterestedIn { get; set; }
        public string SeenDoctorRelatedProcedure { get; set; }
        public string ConsiderTravelOrTreatmentIndiaCountry { get; set; }
        public string ConsiderTravelOrTreatmentIndiaCity { get; set; }
        public string SpecialistsReach { get; set; }
        public string FileUrl { get; set; }

        public string CountryName { get; set; }
        public string Cityname { get; set; }
        public string TreatmentName { get; set; }

        //Total Record for pagging
        public int TotalRecord { get; set; }

        public List<QuoteFiles> lstQuoteFiles { get; set; }

        public object[] GetQuoteList(int? pagenum, string Searchvalue)
        {
            search _search = new search("GetAllQuote");
            _search.AddParameter("@SearchValue", Searchvalue);
            _search.AddParameter("@pagenum", pagenum);
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];

            List<Quote> list = new List<Quote>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.Add(new Quote
                {
                    ID = Guid.Parse(ResultTable.Rows[i]["ID"].ToString()),

                    Name = ResultTable.Rows[i]["Name"].ToString(),
                    Email = ResultTable.Rows[i]["Email"].ToString(),
                    SpecialistsReach = ResultTable.Rows[i]["SpecialistsReach"].ToString(),
                    Subject = ResultTable.Rows[i]["Subject"].ToString(),
                    CountryName = ResultTable.Rows[i]["CountryName"].ToString(),
                    Cityname = ResultTable.Rows[i]["Cityname"].ToString(),
                    TreatmentName = ResultTable.Rows[i]["TreatmentName"].ToString(),   

                    TotalRecord = ResultTable.Rows[i]["TotalRecord"] == DBNull.Value ? TotalRecord : Convert.ToInt32(ResultTable.Rows[i]["TotalRecord"]),
                });
            }


            object[] Obj = new object[1];
            Obj[0] = list;
            return Obj;
        }

        public Quote GetQuoteDetail(Guid Id)
        {
            search _search = new search("GetQuoteDetail");
            _search.AddParameter("@Id", Id.ToString());

            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];

            Quote list = new Quote();
            List<QuoteFiles> quoteFile = new List<QuoteFiles>();

            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                list.ID = Guid.Parse(ResultTable.Rows[i]["ID"].ToString());

                list.Name = ResultTable.Rows[i]["Name"].ToString();
                list.Country = ResultTable.Rows[i]["Country"].ToString();
                list.City = ResultTable.Rows[i]["City"].ToString();
                list.PhoneNo = ResultTable.Rows[i]["PhoneNo"].ToString();
                list.Email = ResultTable.Rows[i]["Email"].ToString();
                list.TreatmentId = Guid.Parse(ResultTable.Rows[i]["TreatmentId"].ToString());
                list.Subject = ResultTable.Rows[i]["Subject"].ToString();
                list.Query = ResultTable.Rows[i]["Query"].ToString();

                list.TravelingForTreatmentTime = ResultTable.Rows[i]["TravelingForTreatmentTime"].ToString();

                list.SubTreatmentId = Guid.Parse(ResultTable.Rows[i]["SubTreatmentId"].ToString());
                list.ProcedureInterestedIn = ResultTable.Rows[i]["ProcedureInterestedIn"].ToString();
                list.SeenDoctorRelatedProcedure = ResultTable.Rows[i]["SeenDoctorRelatedProcedure"].ToString();
                list.ConsiderTravelOrTreatmentIndiaCountry = ResultTable.Rows[i]["ConsiderTravelOrTreatmentIndiaCountry"].ToString();
                list.ConsiderTravelOrTreatmentIndiaCity = ResultTable.Rows[i]["ConsiderTravelOrTreatmentIndiaCity"].ToString();
                list.SpecialistsReach = ResultTable.Rows[i]["SpecialistsReach"].ToString();
                
                if (!string.IsNullOrEmpty(ResultTable.Rows[i]["fileId"] == DBNull.Value ?"": ResultTable.Rows[i]["fileId"].ToString()))
                {
                    QuoteFiles quoteFiles = new QuoteFiles();
                    quoteFiles.ID = Guid.Parse(ResultTable.Rows[i]["fileId"].ToString());
                    quoteFiles.Name = ResultTable.Rows[i]["fileName"].ToString();
                    quoteFile.Add(quoteFiles);
                }

                list.lstQuoteFiles = quoteFile;
            }

            return list;
        }

        public int Save()
        {
            int returnbyproc = 0;
            try
            {
                var treatmentJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procInsertQuote");
                _mysave.AddParameter("@ID", this.ID.ToString());
                _mysave.AddParameter("@Name", this.Name);
                _mysave.AddParameter("@Country", this.Country);
                _mysave.AddParameter("@City", this.City);
                _mysave.AddParameter("@PhoneNo", this.PhoneNo);
                _mysave.AddParameter("@Email", this.Email);
                
                _mysave.AddParameter("@TreatmentId", this.TreatmentId.ToString());
                
                _mysave.AddParameter("@Subject", this.Subject);
                _mysave.AddParameter("@Query", this.Query);
                
                _mysave.AddParameter("@TravelingForTreatmentTime", this.TravelingForTreatmentTime);
                _mysave.AddParameter("@SubTreatmentId", this.SubTreatmentId.ToString());
                _mysave.AddParameter("@ProcedureInterestedIn", this.ProcedureInterestedIn);
                _mysave.AddParameter("@SeenDoctorRelatedProcedure", this.SeenDoctorRelatedProcedure);
                _mysave.AddParameter("@ConsiderTravelOrTreatmentIndiaCountry", this.ConsiderTravelOrTreatmentIndiaCountry);
                _mysave.AddParameter("@ConsiderTravelOrTreatmentIndiaCity", this.ConsiderTravelOrTreatmentIndiaCity);
                _mysave.AddParameter("@SpecialistsReach", this.SpecialistsReach);
                _mysave.AddParameter("@FileUrl", this.FileUrl);

                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception ex)
            {
                throw;
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
                _mysave.AddParameter("@ObjectId", ID.ToString());
                _mysave.AddParameter("@ObjectType", 5);
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
    public class QuoteFiles
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }


    }
