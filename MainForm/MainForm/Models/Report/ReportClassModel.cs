using MainForm.Models.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Report
{
    public class ReportClassModel
    {
        public SQLClass.Models.Job.Job Job {get; set;}

        public SQLClass.Models.Routing.Routing Routing { get; set; }

        public SQLClass.Models.OperationsSubmit.OperationsSubmit Submit { get; set; }

        public double Total_time { get; set; }

        public SQLClass.Models.OperationsDetail.OperationsDetail MachineDetail { get; set; }

        public List<SQLClass.Models.OperationsDetail.OperationsDetail> ToolDetailList { get; set; }
    }
}
