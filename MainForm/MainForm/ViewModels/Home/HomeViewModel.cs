using MainForm.ViewModels.Job;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Home
{
    public class HomeViewModel
    {    
        public List<SQLClass.Models.Job.Job> JobList { get; set; }
        public PagingList<SQLClass.Models.Job.Job> JobListInPaging { get; set; }
        public string Sort_str { get; set; }
        public List<List<SQLClass.Models.Routing.Routing>> RoutingsList { get; set; }
        public List<SQLClass.Models.OperationsSubmit.OperationsSubmit> SubmitList { get; set; }
    }
}
