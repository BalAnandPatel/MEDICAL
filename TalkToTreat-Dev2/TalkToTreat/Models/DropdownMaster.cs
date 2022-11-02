using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TalkToTreatService.Entities;

namespace TalkToTreat.Models
{
    public class DropdownMaster : BaseEntity
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        
        public DropdownMaster()
        {

        }
        public List<SelectListItem> GetDropdownMaster(int Type)
        {
            search _search = new search("procSearchObject");
            _search.AddParameter("@SearchType", Type);
            _search.AddParameter("@SearchText", "");
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0];


            List<SelectListItem> list = new List<SelectListItem>();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            { 
                list.Add(new SelectListItem
                {  
                    Text = ResultTable.Rows[i]["Name"].ToString(),
                    Value = ResultTable.Rows[i]["RecordId"].ToString() 
                }); 
            }

            //Step 16 Create and update the results in object array
             
            return list;
        } 
    }
}