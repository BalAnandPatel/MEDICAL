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

namespace TalkToTreat.Models
{
    public class Home
    {
         
        public int TotalDoctors { get; set; }
        public int TotalEnquiry { get; set; }
        public int TotalAppointment { get; set; }
        public int TotalHospital { get; set; }
        public List<TalkToTreat.Models.Appointment> Appointments { get; set; }
        public object[] getdata(int? pagenum)
        {
            search _search = new search("get_homedashboard"); 
            DataSet ds = _search.ExecuteSearch();
            DataTable ResultTable = ds.Tables[0]; 
             
            Home  list = new Home();
            for (int i = 0; i < ResultTable.Rows.Count; i++)
            { 
                list= (new Home
                {
                    TotalHospital = Convert.ToInt32(ResultTable.Rows[i]["TotalHospital"]),
                    TotalAppointment = Convert.ToInt32(ResultTable.Rows[i]["TotalAppointment"]),
                    TotalDoctors = Convert.ToInt32(ResultTable.Rows[i]["TotalDoctors"]),
                    TotalEnquiry = Convert.ToInt32(ResultTable.Rows[i]["TotalEnquiry"])
                }); 
            } 
            //Step 16 Create and update the results in object array
            object[] Obj = new object[1];
            Obj[0] = list; 
            return Obj;
        }
    }
}