using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Job
{
    public class JobViewModel
    {
        public string Company_name { get; set; }
        public List<SelectListItem> JobStatusList { get; set; }
        public List<SelectListItem> JobStatusFilterList { get; set; }
        public List<SelectListItem> EmployeeUserList { get; set; }
        public List<SelectListItem> ItemTypeList { get; set; }
        public List<SelectListItem> FactoryList { get; set; }
        public WorkViewModel WorkViewModel { get; set; }
        public RoutingViewModel RoutingViewModel { get; set; }        
        public List<SQLClass.Models.Job.Job> JobList { get; set; }
        public PagingList<SQLClass.Models.Job.Job> JobListInPaging { get; set; }
        public List<SQLClass.Models.Routing.Routing> RoutingList { get; set; }
        public List<SelectListItem> RoutingListInPrevious { get; set; }
        public PagingList<SQLClass.Models.Routing.Routing> RoutingListInPaging { get; set; }
        public string Sort_str { get; set; }
        public string Routing_Sort_str { get; set; }
        public string Item_Sort_str { get; set; }
        public Item.ItemViewModel ItemVM { get; set; }
        public List<SQLClass.Models.Item.Item> ItemList { get; set; }
        public PagingList<SQLClass.Models.Item.Item> ItemListInPaging { get; set; }

    }
}
