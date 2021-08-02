using MainForm.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLClass.Models.Company;
using SQLClass.Models.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using QRCoder;
using SQLClass.Models.Routing;
using Microsoft.AspNetCore.Http;
using MainForm.Models.SysCommon;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Item;
using SQLClass.Models.ItemUOM;
using Microsoft.Extensions.Primitives;
using SQLClass.Models.AutoNumber;
using ReflectionIT.Mvc.Paging;
using Microsoft.EntityFrameworkCore;
using MainForm.Models.Job;
using SQLClass.Models.OperationsResource;
using SQLClass.Models.OperationsSubmit;
using SQLClass.Models.OperationsDetail;
using SQLClass.Models.Users;

namespace MainForm.Models.Report
{
    public class ReportModel
    {
        public IJobManage _JobContext { get; set; }

        public IRoutingManage _RoutingContext { get; set; }

        public IOperationsSubmitManage _SubmitContext { get; set; }

        public IOperationsDetailManage _DetailContext { get; set; }

        public IUsersManage _UsersContext { get; set; }

        public IOperationsResourceManage _ResourceContext { get; set; }

        public SysCommonModel SysCommonModel {get; set;}        

        public ReportModel(IJobManage JobContext, IRoutingManage RoutingContext,
            IOperationsSubmitManage SubmitContext, IOperationsDetailManage DetailContext, 
            ISysCommonManage SysCommonContext, IUsersManage UsersManage, IOperationsResourceManage ResourceContext)
        {
            _JobContext ??= JobContext;
            _RoutingContext ??= RoutingContext;
            _SubmitContext ??= SubmitContext;
            _DetailContext ??= DetailContext;
            _UsersContext ??= UsersManage;
            _ResourceContext ??= ResourceContext;

            SysCommonModel ??= new SysCommonModel(SysCommonContext);

            GetOperationsResource();            
        }

        /// for common        
        public List<SelectListItem> OperationsResourceFilterList { get; set; }
        public List<ReportClassModel> ReportList { get; set; }
        public List<SelectListItem> SubmitTypeList { get; set; }

        public bool GetOperationsResource()
        {
            bool flag = false;
            List<SQLClass.Models.OperationsResource.OperationsResource> temp;
            OperationsResourceFilterList = new List<SelectListItem>();

            flag = _ResourceContext.GetAllResourceByResourceType(1, out temp);
            List<SelectListItem> StatusList = new List<SelectListItem>();

            foreach (SQLClass.Models.OperationsResource.OperationsResource tt in temp)
            {
                OperationsResourceFilterList.Add(new SelectListItem()
                {
                    Text = "(" + tt.Resource_no + ") " + tt.Resource_name,
                    Value = tt.Resource_no
                });
            }            

            return flag;
        }

        public bool GetJobByResource(List<string> resource)
        {
            bool flag = false;
            SQLClass.Models.Job.Job job_temp;
            SQLClass.Models.Routing.Routing routing_temp;
            SQLClass.Models.OperationsSubmit.OperationsSubmit submit_temp;
            List<SQLClass.Models.OperationsDetail.OperationsDetail> detail_list = new List<SQLClass.Models.OperationsDetail.OperationsDetail>();
            List<SQLClass.Models.OperationsDetail.OperationsDetail> tool_detail_list = new List<SQLClass.Models.OperationsDetail.OperationsDetail>();
            ReportList = new List<ReportClassModel>();

            SubmitTypeList = ShareModel.GetSelectList(SysCommonModel, "operations_submit_type");

            if ((resource != null) && (resource.Count > 0))
            {
                foreach(SelectListItem a in OperationsResourceFilterList)
                {
                    a.Selected = false;
                }

                foreach (string b in resource)
                {
                    OperationsResourceFilterList.Find(x => x.Value == b).Selected = true;
                    flag = _DetailContext.GetOperationsDetailByOperationsResourceNo(b, out detail_list);

                    foreach (SQLClass.Models.OperationsDetail.OperationsDetail a in detail_list)
                    {
                        flag = _SubmitContext.GetOperationsSubmitByOperationsSubmitId(a.Operations_submit_id, out submit_temp);

                        SQLClass.Models.Users.Users user_temp;
                        _UsersContext.GetUsersById(submit_temp.Operator_id, out user_temp);
                        submit_temp.Operator_id = user_temp.Name;

                        flag = _DetailContext.GetOperationsDetailByOperationsSubmitId(a.Operations_submit_id, out tool_detail_list);
                        tool_detail_list = tool_detail_list.Where(x => x.Operations_resource_type == 3).ToList();

                        flag = _RoutingContext.GetRoutingByRoutingId(submit_temp.Routing_id, out routing_temp);

                        flag = _JobContext.GetJobByJobId(routing_temp.Job_id, out job_temp);

                        ReportList.Add(new ReportClassModel()
                        {
                            Job = job_temp,
                            Routing = routing_temp,
                            Submit = submit_temp,
                            MachineDetail = a,
                            ToolDetailList = tool_detail_list
                        });
                    }
                }
            }
            else
            {
                _DetailContext.GetAllOperationsDetail(out detail_list);
                detail_list = detail_list.Where(x => x.Operations_resource_type == 1).ToList();

                foreach (SQLClass.Models.OperationsDetail.OperationsDetail a in detail_list)
                {
                    flag = _SubmitContext.GetOperationsSubmitByOperationsSubmitId(a.Operations_submit_id, out submit_temp);

                    SQLClass.Models.Users.Users user_temp;
                    _UsersContext.GetUsersById(submit_temp.Operator_id, out user_temp);
                    submit_temp.Operator_id = user_temp.Name + "\r\n" + user_temp.ChinessName;

                    flag = _DetailContext.GetOperationsDetailByOperationsSubmitId(a.Operations_submit_id, out tool_detail_list);
                    tool_detail_list = tool_detail_list.Where(x => x.Operations_resource_type == 3).ToList();

                    flag = _RoutingContext.GetRoutingByRoutingId(submit_temp.Routing_id, out routing_temp);

                    flag = _JobContext.GetJobByJobId(routing_temp.Job_id, out job_temp);

                    ReportList.Add(new ReportClassModel()
                    {
                        Job = job_temp,
                        Routing = routing_temp,
                        Submit = submit_temp,
                        MachineDetail = a,
                        ToolDetailList = tool_detail_list
                    });
                }
            }

            return flag;
        }     
    }
}
