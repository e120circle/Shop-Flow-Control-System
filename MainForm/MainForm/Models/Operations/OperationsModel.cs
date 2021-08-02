using MainForm.Areas.Identity.Data;
using MainForm.Models.SysCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using SQLClass.Models.Item;
using SQLClass.Models.Job;
using SQLClass.Models.OperationsDetail;
using SQLClass.Models.OperationsResource;
using SQLClass.Models.OperationsSubmit;
using SQLClass.Models.Routing;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Operations
{
    public class OperationsModel
    {
        public IJobManage _JobContext { get; set; }

        public IRoutingManage _RoutingContext { get; set; }

        public IUsersManage _UsersContext { get; set; }

        public IOperationsSubmitManage _SubmitContext { get; set; }

        public IOperationsDetailManage _DetailContext { get; set; }

        public IOperationsResourceManage _ResourceContext { get; set; }

        public OperationsSubmitModel SubmitViewModel { get; set; }

        public OperationsDetailModel DetailViewModel { get; set; }

        public MainForm.Models.Job.WorkModel WorkViewModel { get; set; }

        public MainForm.Models.Job.RoutingModel RoutingViewModel { get; set; }

        public MainForm.Models.Item.ItemModel ItemViewModel { get; set; }

        public OperationsModel(IJobManage JobContext, IRoutingManage RoutingContext, IUsersManage UsersManage,
            IOperationsSubmitManage SubmitContext, IOperationsDetailManage DetailContext, 
            IOperationsResourceManage ResourceContext, ISysCommonManage SysCommonContext)
        {
            _JobContext ??= JobContext;
            _RoutingContext ??= RoutingContext;
            _UsersContext ??= UsersManage;
            _SubmitContext ??= SubmitContext;
            _DetailContext ??= DetailContext;
            _ResourceContext ??= ResourceContext;
            WorkViewModel ??= new Job.WorkModel();
            RoutingViewModel ??= new Job.RoutingModel();
            SubmitViewModel ??= new OperationsSubmitModel();
            DetailViewModel ??= new OperationsDetailModel();

            SysCommonModel ??= new SysCommonModel(SysCommonContext);

            OPGroupeList = ShareModel.GetSelectList(SysCommonModel, "operations_groupe");
        }
        
        public SysCommonModel SysCommonModel { get; set; }

        public List<SQLClass.Models.Routing.Routing> RoutingList { get; set; }

        public List<SelectListItem> SubmitTypeList { get; set; }

        public List<SQLClass.Models.Job.Job> JobList { get; set; }

        public List<SQLClass.Models.OperationsResource.OperationsResource> MachineResource { get; set; }
        public List<SQLClass.Models.OperationsResource.OperationsResource> ToolResource { get; set; }

        public List<SQLClass.Models.OperationsDetail.OperationsDetail> MachineDetailList { get; set; }

        public List<SelectListItem> OPGroupeList { get; set; }

        public void GetJob(string job_no)
        {
            if (job_no == null)
            {
                WorkViewModel = new Job.WorkModel();
                return;
            }
            else
            {
                SQLClass.Models.Job.Job temp;
                _JobContext.GetJobByJobNo(job_no, out temp);

                WorkViewModel.Job_id = temp.Job_id;
                WorkViewModel.Job_barcode = temp.Job_barcode;
                WorkViewModel.Job_qrcode = temp.Job_qrcode;
                WorkViewModel.JobSourceTypeList = ShareModel.GetSelectList(SysCommonModel, "job_source_type");
                if (temp.Job_source_type != null)
                {
                    WorkViewModel.JobSourceTypeList.Where(x => Convert.ToInt16(x.Value) == temp.Job_source_type).FirstOrDefault().Selected = true;
                }
                WorkViewModel.Job_source_type = temp.Job_source_type.Value;
                WorkViewModel.Job_source_id = temp.Job_source_id;
                WorkViewModel.Job_source_no = temp.Job_source_no;
                WorkViewModel.Job_no = temp.Job_no;
                WorkViewModel.Job_date = temp.Job_date;
                WorkViewModel.Job_released_date = temp.Job_released_date;
                WorkViewModel.Job_description = temp.Job_description;
                WorkViewModel.Job_status = temp.Job_status;
                WorkViewModel.Customer_no = temp.Customer_no;
                WorkViewModel.Customer_name = temp.Customer_name;
                WorkViewModel.Customer_order_id = temp.Customer_order_id;
                WorkViewModel.Customer_order_no = temp.Customer_order_no;
                WorkViewModel.Customer_shipping_date = temp.Customer_shipping_date;
                WorkViewModel.Customer_shipping_address = temp.Customer_shipping_address;
                WorkViewModel.Employee_id = temp.Employee_id;
                WorkViewModel.Item_id = temp.Item_id;
                WorkViewModel.Item_no = temp.Item_no;
                WorkViewModel.Item_name = temp.Item_name;
                WorkViewModel.Item_description = temp.Item_description;
                WorkViewModel.Customer_order_quantity = temp.Customer_order_quantity;
                WorkViewModel.Customer_order_quantity_uom = temp.Customer_order_quantity_uom;
                WorkViewModel.Scheduled_start_date = temp.Scheduled_start_date;
                WorkViewModel.Scheduled_completion_date = temp.Scheduled_completion_date;
                WorkViewModel.Scheduled_completion_quantity = temp.Scheduled_completion_quantity;
                WorkViewModel.Scheduled_completion_quantity_uom = temp.Scheduled_completion_quantity_uom;
                WorkViewModel.Actually_start_date = temp.Actually_start_date;
                WorkViewModel.Actually_completion_date = temp.Actually_completion_date;
                WorkViewModel.Actually_completion_quantity = temp.Actually_completion_quantity.Value;
                WorkViewModel.Actually_completion_quantity_uom = temp.Actually_completion_quantity_uom;
                WorkViewModel.Actually_defective_quantity = temp.Actually_defective_quantity.Value;
                WorkViewModel.Actually_defective_quantity_uom = temp.Actually_defective_quantity_uom;
                WorkViewModel.Input_of_inventory_quantity = temp.Input_of_inventory_quantity.Value;
                WorkViewModel.Input_of_inventory_quantity_uom = temp.Input_of_inventory_quantity_uom;
                WorkViewModel.Reserved_field01 = temp.Reserved_field01;
            }
        }

        /// for routing
        public void GetRoutingByJob_no(string job_no)
        {
            if (job_no == null)
            {
                RoutingList = new List<Routing>();
                return;
            }
            else
            {
                List<SQLClass.Models.Routing.Routing> temp;
                _RoutingContext.GetLikeRoutingByRoutingNo(job_no, out temp);

                RoutingList = temp;
            }
        }

        private string GetSubmitAutoNumber(string format)
        {
            List<SQLClass.Models.OperationsSubmit.OperationsSubmit> temp;

            _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(format, out temp);

            if (temp.Count > 0)
            {
                string no = temp.Last().Operations_submit_no;
                int tt = Convert.ToInt32(no.Split('-')[2]);

                return format + "-" + (tt + 1).ToString("000");
            }
            else
            {
                return format + "-001";
            }
        }

        public void GetAllJob()
        {
            bool flag = false;
            List<SQLClass.Models.Job.Job> temp = new List<SQLClass.Models.Job.Job>();

            flag = _JobContext.GetAllJob(out temp);
            JobList = temp;
        }

        public List<SelectListItem> GetSubmitType()
        {
            return ShareModel.GetSelectList(SysCommonModel, "operations_submit_type");
        }

        public void GetAllMachine()
        {
            List<SQLClass.Models.OperationsResource.OperationsResource> temp;

            _ResourceContext.GetAllResourceByResourceType(1, out temp);
            MachineResource = temp;
        }

        public void GetAllTool()
        {
            List<SQLClass.Models.OperationsResource.OperationsResource> temp;

            _ResourceContext.GetAllResourceByResourceType(3, out temp);
            ToolResource = temp;
        }

        public SQLClass.Models.OperationsResource.OperationsResource GetResource(string no)
        {
            SQLClass.Models.OperationsResource.OperationsResource temp;

            _ResourceContext.GetResourceByResourceNo(no, out temp);
            return temp;
        }

        public List<SQLClass.Models.Users.Users> GetGroupeUser(string groupe)
        {
            bool flag = false;
            List<SQLClass.Models.Users.Users> temp = new List<SQLClass.Models.Users.Users>();
            List<SQLClass.Models.Users.Users> result = new List<SQLClass.Models.Users.Users>();

            flag = _UsersContext.GetAllUsersByRoleType(3, out temp);
            foreach (var tt in temp.Where(x => x.Groupe == groupe).ToList())
            {
                result.Add(tt);
            }            

            flag = _UsersContext.GetAllUsersByRoleType(4, out temp);
            foreach (var tt in temp.Where(x => x.Groupe == groupe).ToList())
            {
                result.Add(tt);
            }

            flag = _UsersContext.GetAllUsersByRoleType(5, out temp);
            foreach (var tt in temp.Where(x => x.Groupe == groupe).ToList())
            {
                result.Add(tt);
            }

            return result;
        }

        public void GetCreateOperations(int job_id, string routing_no)
        {
            SubmitTypeList = ShareModel.GetSelectList(SysCommonModel, "operations_submit_type");

            SQLClass.Models.Job.Job job_temp;
            SQLClass.Models.Routing.Routing route_temp;

            _JobContext.GetJobByJobId(job_id, out job_temp);
            WorkViewModel.Job_no = job_temp.Job_no;
            WorkViewModel.Item_no = job_temp.Item_no;
            WorkViewModel.Item_name = job_temp.Item_name;
            WorkViewModel.Item_description = job_temp.Item_description;

            List<SQLClass.Models.Routing.Routing> ui;
            _RoutingContext.GetLikeRoutingByRoutingNo(routing_no, out ui);
            route_temp = ui.LastOrDefault();
            RoutingViewModel.Job_id = route_temp.Job_id;
            RoutingViewModel.Routing_no = route_temp.Routing_no;
            RoutingViewModel.Routing_name = route_temp.Routing_name;
            RoutingViewModel.Scheduled_completion_quantity = route_temp.Scheduled_completion_quantity;

            SubmitViewModel.Routing_id = route_temp.Routing_id;
            SubmitViewModel.Item_id = job_temp.Item_id;
            SubmitViewModel.Operator_id = "???";
            SubmitViewModel.Operations_submit_no = GetSubmitAutoNumber(route_temp.Routing_no);
            SubmitViewModel.Operations_submit_description = "";
            SubmitViewModel.Standard_setup_time = route_temp.Standard_setup_time;
            SubmitViewModel.Standard_unit_lead_time = route_temp.Standard_unit_lead_time;
            SubmitViewModel.Scheduled_start_date = route_temp.Scheduled_start_date;
            SubmitViewModel.Scheduled_completion_date = route_temp.Scheduled_completion_date;
            SubmitViewModel.Actually_start_date = DateTime.Now;

            if(route_temp.Previous_routing_id == 0)
            {
                List<OperationsSubmit> get_quantity;
                int quantity = job_temp.Scheduled_completion_quantity;

                _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(SubmitViewModel.Operations_submit_no.Substring(0, 14), out get_quantity);

                foreach (OperationsSubmit a in get_quantity)
                {
                    quantity -= a.Actually_completion_quantity;
                }

                SubmitViewModel.Scheduled_target_quantity = ( quantity < 0) ? 0 : quantity;
            }
            else
            {
                Routing get_route;
                List<OperationsSubmit> get_quantity;
                int quantity = 0;

                _RoutingContext.GetRoutingByRoutingId(route_temp.Previous_routing_id, out get_route);

                _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(get_route.Routing_no, out get_quantity);

                foreach (OperationsSubmit a in get_quantity)
                {
                    quantity += a.Transfer_out_quantity;
                }

                _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(SubmitViewModel.Operations_submit_no.Substring(0, 14), out get_quantity);

                foreach (OperationsSubmit a in get_quantity)
                {
                    quantity -= a.Transfer_out_quantity;
                }

                SubmitViewModel.Scheduled_target_quantity = quantity;
            }
            
            SubmitViewModel.Scheduled_target_quantity_uom = route_temp.Scheduled_completion_quantity_uom;
            SubmitViewModel.Scheduled_target_quantity_uom = route_temp.Scheduled_completion_quantity_uom;
            SubmitViewModel.Is_classification_defective_item = route_temp.Is_classification_defective_item;
            SubmitViewModel.Is_allow_split_submit = route_temp.Is_allow_split_submit;
            SubmitViewModel.Is_define_resource_in_operations_submit = route_temp.Is_define_resource_in_opeations_submit;
            SubmitViewModel.Is_enable = true;
            SubmitViewModel.Create_by = "";
            SubmitViewModel.Create_date = DateTime.Now;
            SubmitViewModel.Reserved_field01 = "";
            SubmitViewModel.Reserved_field02 = "0";

            DetailViewModel.Operations_resource_no = "";
            DetailViewModel.Operations_resource_name = "";
            DetailViewModel.Operations_resource_description = "";            
        }

        public bool PostCreateOperations(IFormCollection data, List<SQLClass.Models.OperationsResource.OperationsResource> SelectMachine)
        {
            bool flag = false;

            SQLClass.Models.OperationsSubmit.OperationsSubmit submit = new OperationsSubmit() { 
                Routing_id = Convert.ToInt32(data["SubmitViewModel.Routing_id"]),
                Item_id = Convert.ToInt32(data["SubmitViewModel.Item_id"]),
                Operator_groupe_id = data["SubmitViewModel.Operator_groupe_id"],
                Operator_id = data["SubmitViewModel.Operator_id"],
                Operations_submit_no = data["SubmitViewModel.Operations_submit_no"],
                Operations_submit_type = Convert.ToInt16(data["SubmitViewModel.Operations_submit_type"]),
                Operations_submit_description = data["SubmitViewModel.Operations_submit_description"],
                Standard_setup_time = Convert.ToInt32(data["SubmitViewModel.Standard_setup_time"]),
                Standard_unit_lead_time = Convert.ToInt32(data["SubmitViewModel.Standard_unit_lead_time"]),
                Actually_start_date = DateTime.Now,
                Scheduled_completion_quantity = Convert.ToInt32(data["SubmitViewModel.Scheduled_completion_quantity"]),
                Scheduled_completion_quantity_uom = data["SubmitViewModel.Scheduled_completion_quantity_uom"],
                Scheduled_target_quantity = Convert.ToInt32(data["SubmitViewModel.Scheduled_target_quantity"]),
                Scheduled_target_quantity_uom = data["SubmitViewModel.Scheduled_target_quantity_uom"],
                Is_classification_defective_item = data["SubmitViewModel.Is_classification_defective_item"].Contains("true"),
                Is_allow_split_submit = data["SubmitViewModel.Is_allow_split_submit"].Contains("true"),
                Is_define_resource_in_operations_submit = data["SubmitViewModel.Is_define_resource_in_operations_submit"].Contains("true"),
                Is_enable = data["SubmitViewModel.Is_enable"].Contains("true"),
                Create_by = data["SubmitViewModel.Operator_id"],
                Create_date = DateTime.Now,
                Reserved_field01 = data["SubmitViewModel.Reserved_field01"]
            };

            flag = _SubmitContext.CreateOperationsSubmit(submit);

            List<int> previous_routing_id;
            List<string> routing_no;
            List<OperationsSubmit> like_submit;
            _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "previous_routing_id", out previous_routing_id);
            _RoutingContext.Getdata<string>("routing_id", submit.Routing_id.ToString(), "routing_no", out routing_no);            
            _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(routing_no.LastOrDefault(), out like_submit);

            if (like_submit.Count() == 1)
            {   //表示為該站第一次開工故回填至途程主檔
                _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_start_date", submit.Actually_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (previous_routing_id.LastOrDefault() == 0)
            {   
                if(like_submit.Count() == 1)
                {   //表示為首站且為第一次開工故回填至工單主檔
                    List<int> job_id;
                    _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "job_id", out job_id);

                    List<string> job_no;
                    _JobContext.Getdata<string>("job_id", job_id.LastOrDefault().ToString(), "job_no", out job_no);

                    _JobContext.Setdata(job_no.LastOrDefault(), "actually_start_date", submit.Actually_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            _SubmitContext.GetOperationsSubmitByOperationsSubmitNo(data["SubmitViewModel.Operations_submit_no"], out submit);

            foreach (SQLClass.Models.OperationsResource.OperationsResource a in SelectMachine)
            {
                SQLClass.Models.OperationsDetail.OperationsDetail detail = new OperationsDetail()
                {   //儲存生產資源(設備)資訊
                    Operations_submit_id = submit.Operations_submit_id,
                    Workstation_resource_id = 0,
                    Routing_resource_id = 0,
                    Operations_resource_type = a.Operations_resource_type,
                    Operations_resource_no = a.Resource_no,
                    Operations_resource_name = a.Resource_name,
                    Operations_resource_description = a.Resource_description,
                    Mapping_sys_user = "",
                    Is_enable = data["SubmitViewModel.Is_enable"].Contains("true"),
                    Create_by = submit.Create_by,
                    Create_date = submit.Create_date
                };

                flag = _DetailContext.CreateOperationsDetail(detail);
            }

            return flag;
        }

        public List<SQLClass.Models.OperationsSubmit.OperationsSubmit> GetSubmitLike(string format)
        {
            List<SQLClass.Models.OperationsSubmit.OperationsSubmit> temp;

            _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(format, out temp);

            return temp;
        }

        public List<SQLClass.Models.OperationsDetail.OperationsDetail> GetDetail(List<SQLClass.Models.OperationsSubmit.OperationsSubmit> submit)
        {
            List<SQLClass.Models.OperationsDetail.OperationsDetail> temp;
            List<SQLClass.Models.OperationsDetail.OperationsDetail> result = new List<OperationsDetail>();

            foreach (SQLClass.Models.OperationsSubmit.OperationsSubmit a in submit)
            {
                _DetailContext.GetOperationsDetailByOperationsSubmitId(a.Operations_submit_id, out temp);
                result.AddRange(temp.Where(x => x.Operations_resource_type == 1).ToList());
            }

            return result;
        }

        public void GetEditOperations(int job_id, string routing_no, string submit_no)
        {
            SubmitTypeList = ShareModel.GetSelectList(SysCommonModel, "operations_submit_type");

            SQLClass.Models.Job.Job job_temp;
            SQLClass.Models.Routing.Routing route_temp;

            _JobContext.GetJobByJobId(job_id, out job_temp);
            WorkViewModel.Job_no = job_temp.Job_no;
            WorkViewModel.Item_no = job_temp.Item_no;
            WorkViewModel.Item_name = job_temp.Item_name;
            WorkViewModel.Item_description = job_temp.Item_description;

            List<SQLClass.Models.Routing.Routing> ui;
            _RoutingContext.GetLikeRoutingByRoutingNo(routing_no, out ui);
            route_temp = ui.LastOrDefault();
            RoutingViewModel.Routing_name = route_temp.Routing_name;
            RoutingViewModel.Scheduled_completion_quantity = route_temp.Scheduled_completion_quantity;

            SQLClass.Models.OperationsSubmit.OperationsSubmit temp;
            _SubmitContext.GetOperationsSubmitByOperationsSubmitNo(submit_no, out temp);

            SubmitViewModel.Operations_submit_id = temp.Operations_submit_id;
            SubmitViewModel.Routing_id = temp.Routing_id;
            SubmitViewModel.Item_id = temp.Item_id;
            SubmitViewModel.Operator_groupe_id = OPGroupeList.Find(x => x.Value == temp.Operator_groupe_id).Text;

            SQLClass.Models.Users.Users user_temp;
            _UsersContext.GetUsersById(temp.Operator_id, out user_temp);
            SubmitViewModel.Operator_id = user_temp.Name + user_temp.ChinessName;

            SubmitViewModel.Operations_submit_no = temp.Operations_submit_no;
            SubmitViewModel.Operations_submit_type = temp.Operations_submit_type;
            SubmitViewModel.Operations_submit_description = temp.Operations_submit_description;
            SubmitViewModel.Standard_setup_time = temp.Standard_setup_time;
            SubmitViewModel.Standard_unit_setup_time = temp.Standard_unit_setup_time;
            SubmitViewModel.Standard_unit_run_time = temp.Standard_unit_run_time;
            SubmitViewModel.Standard_unit_total_time = temp.Standard_unit_total_time;
            SubmitViewModel.Standard_unit_lead_time = temp.Standard_unit_lead_time;
            SubmitViewModel.Actually_setup_time = temp.Actually_setup_time;
            SubmitViewModel.Actually_unit_setup_time = temp.Actually_unit_setup_time;
            SubmitViewModel.Actually_unit_run_time = temp.Actually_unit_run_time;
            SubmitViewModel.Actually_unit_total_time = temp.Actually_unit_total_time;
            SubmitViewModel.Actually_unit_lead_time = temp.Actually_unit_lead_time;
            SubmitViewModel.Scheduled_start_date = temp.Scheduled_start_date;
            SubmitViewModel.Scheduled_completion_date = temp.Scheduled_completion_date;
            SubmitViewModel.Actually_start_date = temp.Actually_start_date;
            SubmitViewModel.Actually_completion_date = (temp.Actually_completion_date == null) ? DateTime.Now : temp.Actually_completion_date;
            SubmitViewModel.Scheduled_completion_quantity = temp.Scheduled_completion_quantity;
            SubmitViewModel.Scheduled_completion_quantity_uom = temp.Scheduled_completion_quantity_uom;
            SubmitViewModel.Scheduled_target_quantity = temp.Scheduled_target_quantity;
            SubmitViewModel.Scheduled_target_quantity_uom = temp.Scheduled_target_quantity_uom;
            SubmitViewModel.Actually_completion_quantity = temp.Actually_completion_quantity;
            SubmitViewModel.Actually_completion_quantity_uom = temp.Actually_completion_quantity_uom;
            SubmitViewModel.Actually_defective_quantity = temp.Actually_defective_quantity;
            SubmitViewModel.Actually_defective_quantity_uom = temp.Actually_defective_quantity_uom;
            SubmitViewModel.Is_classification_defective_item = temp.Is_classification_defective_item;
            SubmitViewModel.Is_allow_split_submit = temp.Is_allow_split_submit;
            SubmitViewModel.Is_define_resource_in_operations_submit = temp.Is_define_resource_in_operations_submit;
            SubmitViewModel.Transfer_out_quantity = temp.Transfer_out_quantity;
            SubmitViewModel.Transfer_out_quantity_uom = temp.Transfer_out_quantity_uom;
            SubmitViewModel.Is_enable = temp.Is_enable;
            SubmitViewModel.Create_by = temp.Create_by;
            SubmitViewModel.Create_date = temp.Create_date;
            SubmitViewModel.Last_update_by = temp.Create_by;
            SubmitViewModel.Last_update_date = DateTime.Now;
            SubmitViewModel.Reserved_field01 = temp.Reserved_field01;
            SubmitViewModel.Reserved_field02 = temp.Reserved_field02;

            List<SQLClass.Models.OperationsDetail.OperationsDetail> details_temp;
            _DetailContext.GetOperationsDetailByOperationsSubmitId(temp.Operations_submit_id, out details_temp);

            MachineDetailList = details_temp;
            //MachineDetailList = details_temp.Where(x => x.Operations_resource_type == 1).ToList();
        }

        public bool PostEditOperations(string no, IFormCollection data, List<SQLClass.Models.OperationsDetail.OperationsDetail> details)
        {
            bool flag = false;

            SQLClass.Models.OperationsSubmit.OperationsSubmit submit = new SQLClass.Models.OperationsSubmit.OperationsSubmit()
            {
                Operations_submit_id = Convert.ToInt16(data["SubmitViewModel.Operations_submit_id"]),
                Routing_id = Convert.ToInt16(data["SubmitViewModel.Routing_id"]),
                Item_id = Convert.ToInt16(data["SubmitViewModel.Item_id"]),
                Operations_submit_no = data["SubmitViewModel.Operations_submit_no"],
                Operations_submit_type = Convert.ToInt16(data["SubmitViewModel.Operations_submit_type"]),
                Operations_submit_description = data["SubmitViewModel.Operations_submit_description"],
                Standard_setup_time = Convert.ToInt32(data["SubmitViewModel.Standard_setup_time"]),
                Standard_unit_setup_time = Convert.ToInt32(data["SubmitViewModel.Standard_unit_setup_time"]),
                Standard_unit_run_time = Convert.ToInt32(data["SubmitViewModel.Standard_unit_run_time"]),
                Standard_unit_total_time = Convert.ToInt32(data["SubmitViewModel.Standard_unit_total_time"]),
                Standard_unit_lead_time = Convert.ToInt32(data["SubmitViewModel.Standard_unit_lead_time"]),
                Actually_setup_time = Convert.ToInt32(data["SubmitViewModel.Actually_setup_time"]),
                Actually_unit_setup_time = Convert.ToInt32(data["SubmitViewModel.Actually_unit_setup_time"]),
                Actually_unit_run_time = Convert.ToInt32(data["SubmitViewModel.Actually_unit_run_time"]),
                Actually_unit_total_time = Convert.ToInt32(data["SubmitViewModel.Actually_unit_total_time"]),
                Actually_unit_lead_time = Convert.ToInt32(data["SubmitViewModel.Actually_unit_lead_time"]),
                Scheduled_start_date = StringValues.IsNullOrEmpty(data["SubmitViewModel.Scheduled_start_date"]) ? (DateTime?)null : Convert.ToDateTime(data["SubmitViewModel.Scheduled_start_date"]),
                Scheduled_completion_date = StringValues.IsNullOrEmpty(data["SubmitViewModel.Scheduled_completion_date"]) ? (DateTime?)null : Convert.ToDateTime(data["SubmitViewModel.Scheduled_completion_date"]),
                Actually_completion_date = DateTime.Now,
                Scheduled_completion_quantity = Convert.ToInt32(data["SubmitViewModel.Scheduled_completion_quantity"]),
                Scheduled_completion_quantity_uom = data["SubmitViewModel.Scheduled_completion_quantity_uom"],
                Scheduled_target_quantity = Convert.ToInt32(data["SubmitViewModel.Scheduled_target_quantity"]),
                Scheduled_target_quantity_uom = data["SubmitViewModel.Scheduled_target_quantity_uom"],
                Actually_completion_quantity = Convert.ToInt32(data["SubmitViewModel.Actually_completion_quantity"]),
                Actually_completion_quantity_uom = data["SubmitViewModel.Actually_completion_quantity_uom"],                
                Actually_defective_quantity = Convert.ToInt32(data["SubmitViewModel.Actually_defective_quantity"]),
                Actually_defective_quantity_uom = data["SubmitViewModel.Actually_defective_quantity_uom"],
                Is_classification_defective_item = data["RoutingViewModel.Is_classification_defective_item"].Contains("true"),
                Is_allow_split_submit = data["RoutingViewModel.Is_allow_split_submit"].Contains("true"),
                Is_define_resource_in_operations_submit = data["RoutingViewModel.Is_define_resource_in_opeations_submit"].Contains("true"),
                Transfer_out_quantity = Convert.ToInt32(data["SubmitViewModel.Transfer_out_quantity"]),
                Transfer_out_quantity_uom = data["SubmitViewModel.Transfer_out_quantity_uom"],
                Is_enable = data["RoutingViewModel.Is_enable"].Contains("true"),
                Create_by = data["RoutingViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["SubmitViewModel.Create_date"]),
                Last_update_by = data["SubmitViewModel.Last_update_by"],
                Last_update_date = DateTime.Now,
                Reserved_field01 = data["SubmitViewModel.Reserved_field01"],
                Reserved_field02 = data["SubmitViewModel.Reserved_field02"]
            };

            flag = _SubmitContext.EditOperationsSubmit(no, submit);

            foreach (SQLClass.Models.OperationsDetail.OperationsDetail detail in details)
            {
                detail.Operations_submit_id = submit.Operations_submit_id;
                detail.Workstation_resource_id = 0;
                detail.Routing_resource_id = 0;
                detail.Mapping_sys_user = "";
                detail.Is_enable = data["SubmitViewModel.Is_enable"].Contains("true");
                detail.Create_by = data["SubmitViewModel.Create_by"];
                detail.Create_date = Convert.ToDateTime(data["SubmitViewModel.Create_date"]);

                flag = _DetailContext.CreateOperationsDetail(detail);
            }

            List<string> routing_no;
            _RoutingContext.Getdata<string>("routing_id", submit.Routing_id.ToString(), "routing_no", out routing_no);

            List<int> job_id;
            _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "job_id", out job_id);

            List<string> job_no;
            _JobContext.Getdata<string>("job_id", job_id.LastOrDefault().ToString(), "job_no", out job_no);

            List<Routing> same_job;
            _RoutingContext.GetAllRoutingByJobId(job_id.LastOrDefault(), out same_job);

            List<int> submit_id;
            _SubmitContext.Getdata<int>("operations_submit_no", submit.Operations_submit_no, "routing_id", out submit_id);

            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_setup_time", submit.Actually_setup_time);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_unit_lead_time", submit.Actually_unit_lead_time);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_unit_setup_time", submit.Actually_unit_setup_time);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_unit_run_time", submit.Actually_unit_run_time);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_unit_total_time", submit.Actually_unit_total_time);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_completion_date", submit.Actually_completion_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            List<int> temp_completion_quantity;
            _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "actually_completion_quantity", out temp_completion_quantity);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_completion_quantity", submit.Actually_completion_quantity + temp_completion_quantity.LastOrDefault());
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_completion_quantity_uom", submit.Actually_completion_quantity_uom);

            List<int> temp_defective_quantity;
            _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "actually_defective_quantity", out temp_defective_quantity);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_defective_quantity", submit.Actually_defective_quantity + temp_defective_quantity.LastOrDefault());
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "actually_defective_quantity_uom", submit.Actually_defective_quantity_uom);

            List<int> temp_transfer_quantity;
            _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "transfer_out_quantity", out temp_transfer_quantity);
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "transfer_out_quantity", submit.Transfer_out_quantity + temp_transfer_quantity.LastOrDefault());
            _RoutingContext.Setdata(routing_no.LastOrDefault(), "transfer_out_quantity_uom", submit.Transfer_out_quantity_uom);

            var tt = same_job.Where(x => x.Previous_routing_id == submit_id.LastOrDefault());

            if (tt.Count() <= 0)
            {
                List<UInt32> completion_quantity;
                _JobContext.Getdata<UInt32>("job_id", job_id.LastOrDefault().ToString(), "actually_completion_quantity", out completion_quantity);
                _JobContext.Setdata(job_no.LastOrDefault(), "actually_completion_quantity", submit.Actually_completion_quantity + completion_quantity.LastOrDefault());

                List<UInt32> defective_quantity;
                _JobContext.Getdata<UInt32>("job_id", job_id.LastOrDefault().ToString(), "actually_defective_quantity", out defective_quantity);
                _JobContext.Setdata(job_no.LastOrDefault(), "actually_defective_quantity", submit.Actually_defective_quantity + defective_quantity.LastOrDefault());

                List<UInt32> input_of_inventory_quantity;
                _JobContext.Getdata<UInt32>("job_id", job_id.LastOrDefault().ToString(), "input_of_inventory_quantity", out input_of_inventory_quantity);
                _JobContext.Setdata(job_no.LastOrDefault(), "input_of_inventory_quantity", submit.Transfer_out_quantity + input_of_inventory_quantity.LastOrDefault());

                List<int> routing_transfer_quantity;
                List<UInt32> start_quantity;
                _RoutingContext.Getdata<int>("routing_id", submit.Routing_id.ToString(), "transfer_out_quantity", out routing_transfer_quantity);
                _JobContext.Getdata<UInt32>("job_id", job_id.LastOrDefault().ToString(), "scheduled_completion_quantity", out start_quantity);

                if(routing_transfer_quantity.LastOrDefault() >= start_quantity.LastOrDefault())
                {
                    _JobContext.Setdata(job_no.LastOrDefault(), "actually_completion_date", submit.Actually_completion_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    _JobContext.Setdata(job_no.LastOrDefault(), "job_status", 4);
                }
            }

            return flag;
        }
    }
}
