using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.Master
{
    public class State
    {
        public string State_name { get; set; }
        public int State_Id { get; set; }
    }
    public class District
    {
        public string Dist_name { get; set; }
        public int Dist_Id { get; set; }
        public int State_Id { get; set; }
    }

    public class Category
    {
        public int Cat_id { get; set; }
        public int Act_id { get; set; }
        public string Cat_name { get; set; }
        public string Cat_type { get; set; }
    }
    public class Resource
    {
        public int Item_no { get; set; }
        public string Item_name { get; set; }
        public int Cat_Id { get; set; }
         
    }

    public class ResourceDetail
    {
    
        public int  contact_id { get; set; }

        public System.Nullable<int>  state { get; set; }

        public System.Nullable<int>  dist { get; set; }

        public string  dept_name { get; set; }

        public string  dept_addr { get; set; }

        public string  cont_per { get; set; }

        public string  cont_per_desg { get; set; }

        public string  cont_addr { get; set; }
        public string  pincode { get; set; }

        public string  cont_tel1 { get; set; }

        public string  cont_tel2 { get; set; }

        public string  cont_tel3 { get; set; }

        public string  mobile { get; set; }

        public string  fax { get; set; }

        public string  email { get; set; }

        public string  ent_source { get; set; }

        public string  status { get; set; }

        public System.Nullable<System.DateTime>  creation_date { get; set; }

        public System.Nullable<System.DateTime>  updation_date { get; set; }

        public int  item_id { get; set; }

        public string  quantity { get; set; }

        public string  unit { get; set; }

        public string  avail_time1 { get; set; }

        public string  avail_time2 { get; set; }

        public string  descrip { get; set; }

        public string  item_loc { get; set; }

        public string  Operator { get; set; }

        public string  expr { get; set; }

        public string  training { get; set; }

        public string  road;
        
        public string  train;
        
        public string  air;
        
        public string  water;
        
        public string  trans_na;
        
        public System.DateTime  todaydate;
        
        public System.Nullable<System.DateTime>  update_date;

    }
}
 