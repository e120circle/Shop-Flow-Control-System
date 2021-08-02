using MainForm.Models.Report;
using MainForm.ViewModels.Job;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Report
{
    public class ReportViewModel
    {
        public List<SelectListItem> OperationsResourceFilterList { get; set; } 
        public List<ReportClassModel> ReportList { get; set; }
        public PagingList<ReportClassModel> ReportListInPaging { get; set; }
        public string Sort_str { get; set; }        
        public List<SelectListItem> Operations_resource_filter { get; set; }
        public DateTime? Actually_start_date_start_filter { get; set; }
        public DateTime? Actually_start_date_end_filter { get; set; }
        public string Job_no_filter { get; set; }
        public string Item_no_filter { get; set; }
        public string Customer_no_filter { get; set; }
        public List<SelectListItem> SubmitTypeList { get; set; }
    }
}
