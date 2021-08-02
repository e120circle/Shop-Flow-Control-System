using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.SysCommon
{
    public class SelectListModel
    {
        public int Select_list_id { get; set; }
        public string Select_list_key { get; set; }
        public int Select_list_value { get; set; }
        public string Select_process_code { get; set; }
        public string Select_list_name { get; set; }
        public string Select_list_description { get; set; }
        public bool Is_enable { get; set; }
        public string Create_by { get; set; }
        public DateTime Create_date { get; set; }
        public string Last_update_by { get; set; }
        public DateTime Last_update_date { get; set; }
    }
}
