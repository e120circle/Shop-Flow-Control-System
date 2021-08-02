using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Operations
{
    public class OperationsViewModel
    {
        public OperationsSubmitViewModel SubmitViewModel { get; set; }

        public OperationsDetailViewModel DetailViewModel { get; set; }

        public MainForm.ViewModels.Job.WorkViewModel WorkViewModel { get; set; }

        public MainForm.ViewModels.Job.RoutingViewModel RoutingViewModel { get; set; }

        public MainForm.ViewModels.Item.ItemViewModel ItemViewModel { get; set; }

        public List<SQLClass.Models.Routing.Routing> RoutingList { get; set; }

        public List<SQLClass.Models.OperationsSubmit.OperationsSubmit> SubmitList { get; set; }

        public PagingList<SQLClass.Models.Routing.Routing> RoutingListInPaging { get; set; }

        public string Sort_str { get; set; }

        public List<SelectListItem> SubmitTypeList { get; set; }

        public List<SQLClass.Models.Job.Job> JobList { get; set; }

        public string Resource_Sort_str { get; set; }
        public OperationsResource.OperationsResourceViewModel ResourceVM { get; set; }
        public PagingList<SQLClass.Models.OperationsResource.OperationsResource> MachineResourceInPaging { get; set; }
        public PagingList<SQLClass.Models.OperationsResource.OperationsResource> ToolResourceInPaging { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "請選擇加工設備編號")]
        public string SelectMachineStr { get; set; }
        public List<SQLClass.Models.OperationsResource.OperationsResource> SelectMachine { get; set; }

        public List<SQLClass.Models.OperationsDetail.OperationsDetail> MachineDetailList { get; set; }

        public List<SQLClass.Models.OperationsDetail.OperationsDetail> ToolDetailList { get; set; }

        public List<SelectListItem> OPGroupeList { get; set; }
    }
}
