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
using SQLClass.Models.OperationsSubmit;

namespace MainForm.Models.Home
{
    public class HomeModel
    {
        public IJobManage _JobContext { get; set; }

        public IRoutingManage _RoutingContext { get; set; }
        public IOperationsSubmitManage _SubmitContext { get; set; }

        public HomeModel(IJobManage JobContext, IRoutingManage RoutingContext, IOperationsSubmitManage SubmitContext,
            ISysCommonManage SysCommonContext)
        {
            _JobContext ??= JobContext;
            _RoutingContext ??= RoutingContext;
            _SubmitContext ??= SubmitContext;
        }

        /// for common        
        public List<SQLClass.Models.Job.Job> JobList { get; set; }                
        public List<SQLClass.Models.Routing.Routing> RoutingList { get; set; }
        public List<SQLClass.Models.OperationsSubmit.OperationsSubmit> SubmitList { get; set; }
        ///////////////////////////////////////////////////////////for job///////////////////////////////////////////////////////////

        // 修改該地方
        public bool GetAllJobByJobDate()
        {
            bool flag = false;
            List<SQLClass.Models.Job.Job> temp;
            List<SQLClass.Models.Job.Job> JobList_temp = new List<SQLClass.Models.Job.Job>();

                flag = _JobContext.GetAllJobByJobDate(out temp);
                JobList_temp.AddRange(temp);


            JobList = JobList_temp;

            return flag;
        }
            
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

                List<SQLClass.Models.OperationsSubmit.OperationsSubmit> submit_temp;
                foreach (Routing a in RoutingList)
                {
                    _SubmitContext.GetLikeOperationsSubmitByOperationsSubmitNo(a.Routing_no, out submit_temp);
                    SubmitList.AddRange(submit_temp);
                }
            }
        }
    }
}
