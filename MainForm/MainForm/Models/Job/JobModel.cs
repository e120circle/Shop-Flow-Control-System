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
using Net.ConnectCode.Barcode;

namespace MainForm.Models.Job
{
    public class JobModel
    {
        public IJobManage _JobContext { get; set; }

        public IRoutingManage _RoutingContext { get; set; }

        public IItemManage _ItemContext { get; set; }

        public IItemUOMManage _ItemUOMContext { get; set; }

        public IAutoNumber _AutoNumberContext { get; set; }

        private ICompanyManage _company { get; set; }

        public WorkModel WorkViewModel { get; set; }

        public RoutingModel RoutingViewModel { get; set; }

        public SysCommonModel SysCommonModel {get; set;}        

        public JobModel(IJobManage JobContext, IRoutingManage RoutingContext, ISysCommonManage SysCommonContext,
            IItemManage ItemContext, IItemUOMManage ItemUOMContext, IAutoNumber AutoNumberContext,  
            ICompanyManage company)
        {
            _JobContext ??= JobContext;
            _RoutingContext ??= RoutingContext;
            _ItemContext ??= ItemContext;
            _ItemUOMContext ??= ItemUOMContext;
            _AutoNumberContext ??= AutoNumberContext;
            _company ??= company;

            SQLClass.Models.Company.Company temp = new SQLClass.Models.Company.Company();
            _company.GetCompany(out temp);
            Company_name = temp.Company_name;
            WorkViewModel ??= new WorkModel();
            RoutingViewModel ??= new RoutingModel();

            SysCommonModel ??= new SysCommonModel(SysCommonContext);
        }

        /// for common        
        public string Company_name { get; set; }
        public List<SelectListItem> JobStatusList { get; set; }
        public List<SelectListItem> JobStatusFilterList { get; set; }
        public List<SelectListItem> EmployeeUserList { get; set; }
        public List<SelectListItem> ItemTypeList { get; set; }
        public List<SQLClass.Models.Job.Job> JobList { get; set; }                
        public List<SQLClass.Models.Routing.Routing> RoutingList { get; set; }
        public List<SelectListItem> RoutingListInPrevious { get; set; }
        public List<SQLClass.Models.Item.Item> ItemList { get; set; }

        ///////////////////////////////////////////////////////////for job///////////////////////////////////////////////////////////
        public bool GetAllJob()
        {
            bool flag = false;
            List<SQLClass.Models.Job.Job> temp;

            flag = _JobContext.GetAllJob(out temp);
            JobList = temp;

            JobStatusList = ShareModel.GetJob_statusList(SysCommonModel);
            JobStatusFilterList = JobStatusList;
            return flag;
        }
                
        public string GetLike(string format)
        {
            List<SQLClass.Models.Job.Job> temp;

            _JobContext.GetLikeJobByJobNo(format, out temp);

            if (temp.Count != 0)
                return temp.Last().Job_no;
            else
                return "";
        }

        public void GetCreateJob(MainFormUsers user)
        {
            ShareModel.GetLike += GetLike;

            WorkViewModel.Factory_id = 1;

            WorkViewModel.JobSourceTypeList = ShareModel.GetSelectList(SysCommonModel, "job_source_type");
            WorkViewModel.JobSourceTypeList[0].Selected = true;
            WorkViewModel.Job_source_type = Convert.ToInt16(WorkViewModel.JobSourceTypeList[0].Value);
            WorkViewModel.Job_no = ShareModel.GetAutoNumber("job_no", _AutoNumberContext);
            WorkViewModel.Job_barcode = Barcode(WorkViewModel.Job_no);
            WorkViewModel.Job_date = DateTime.Today.Date;
            WorkViewModel.Job_description = "◎技術員每日9、14點測量加工部位，並由品管人員判定確認。\r\n" +
                "◎依據剖溝中心點+0.25mm，真圓度公差0.03mm，導角有無毛邊，綜合判定合格與否。\r\n" +
                "◎加工時請核對加工數量，不足時請回報生管。\r\n" +
                "◎※304含：SCS13，1.4308，1.4301，CF8，CF3  ※316含：SCS14，1.4408，1.4571，1.4581，CF8M  ※S45C材質\r\n";

            JobStatusList = ShareModel.GetJob_statusList(SysCommonModel);
            JobStatusFilterList = JobStatusList;
            JobStatusList[0].Selected = true;
            WorkViewModel.Job_status = Convert.ToInt16(JobStatusList[0].Value);

            WorkViewModel.Customer_no = "";
            WorkViewModel.Customer_name = "";

            WorkViewModel.Employee_id = user.Id;
            if (EmployeeUserList.Find(x => x.Value == user.Id) != null)
                EmployeeUserList.Find(x => x.Value == user.Id).Selected = true;

            WorkViewModel.Item_id = 0;
            WorkViewModel.Item_no = "";
            WorkViewModel.Item_name = "";
            WorkViewModel.Item_description = "";
            WorkViewModel.Item_type = 0;
            ItemTypeList = MainForm.Models.ShareModel.GetSelectList(SysCommonModel, "item_type");

            WorkViewModel.Customer_order_quantity = 0;
            WorkViewModel.Customer_order_quantity_uom = "";
            WorkViewModel.Scheduled_completion_quantity = 0;
            WorkViewModel.Scheduled_completion_quantity_uom = "";
            WorkViewModel.Actually_completion_quantity = 0;
            WorkViewModel.Actually_completion_quantity_uom = "";
            WorkViewModel.Actually_defective_quantity = 0;
            WorkViewModel.Actually_defective_quantity_uom = "";
            WorkViewModel.Input_of_inventory_quantity = 0;
            WorkViewModel.Input_of_inventory_quantity_uom = "";
            WorkViewModel.Is_classification_defective_item = false;
            WorkViewModel.Is_allow_split_submit = true;
            WorkViewModel.Is_define_desource_in_operations_submit = true;
            WorkViewModel.Is_enable = true;
            WorkViewModel.Create_by = user.Id;
            WorkViewModel.Create_date = DateTime.Today.Date;
            WorkViewModel.Reserved_field01 = "";
            WorkViewModel.Reserved_field02 = "";

            ShareModel.GetLike -= GetLike;
        }   

        public bool PostCreateJob(IFormCollection data, MainFormUsers user)
        {
            bool flag = false;

            SQLClass.Models.Job.Job job = new SQLClass.Models.Job.Job()
            {
                Factory_id = 1,
                Job_source_type = StringValues.IsNullOrEmpty(data["WorkViewModel.Job_source_type"]) ? 1 : Convert.ToInt16(data["WorkViewModel.Job_source_type"]),
                Job_source_no = data["WorkViewModel.Job_source_no"],
                Job_no = data["WorkViewModel.Job_no"],
                Job_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Job_date"]) ? DateTime.Today.Date : Convert.ToDateTime(data["WorkViewModel.Job_date"]),
                Job_description = data["WorkViewModel.Job_description"],
                Job_status = Convert.ToInt16(data["WorkViewModel.Job_status"]),
                Customer_no = data["WorkViewModel.Customer_no"],
                Customer_name = data["WorkViewModel.Customer_name"],
                Customer_shipping_date = Convert.ToDateTime(data["WorkViewModel.Customer_shipping_date"]),
                Employee_id = data["WorkViewModel.Employee_id"],
                Item_id = Convert.ToInt32(data["WorkViewModel.Item_id"]),
                Item_no = data["WorkViewModel.Item_no"],
                Item_name = data["WorkViewModel.Item_name"],
                Item_description = data["WorkViewModel.Item_description"],
                Item_type = Convert.ToInt32(data["WorkViewModel.Item_type"]),
                Customer_order_quantity = Convert.ToInt32(data["WorkViewModel.Customer_order_quantity"]),
                Scheduled_start_date = null,
                Scheduled_completion_date = Convert.ToDateTime(data["WorkViewModel.Scheduled_completion_date"]),
                Scheduled_completion_quantity = Convert.ToInt32(data["WorkViewModel.Scheduled_completion_quantity"]),
                Scheduled_completion_quantity_uom = data["WorkViewModel.Scheduled_completion_quantity_uom"],
                Actually_completion_quantity = 0,
                Actually_defective_quantity = 0,
                Input_of_inventory_quantity = 0,
                Is_classification_defective_item = false,
                Is_allow_split_submit = true,
                Is_define_desource_in_operations_submit = true,
                Is_enable = true,
                Create_by = data["WorkViewModel.Create_by"],
                Reserved_field01 = data["WorkViewModel.Reserved_field01"],
                Reserved_field02 = data["WorkViewModel.Reserved_field02"]
            };

            job.Create_date = DateTime.Now;
            job.Customer_order_quantity_uom = job.Scheduled_completion_quantity_uom;
            job.Actually_completion_quantity_uom = job.Scheduled_completion_quantity_uom;
            job.Actually_defective_quantity_uom = job.Scheduled_completion_quantity_uom;
            job.Input_of_inventory_quantity_uom = job.Scheduled_completion_quantity_uom;

            job.Job_barcode = Barcode(job.Job_no);         

            flag = _JobContext.CreateJob(job);
            ShareModel.GetLike += GetLike;
            int last_number = ShareModel.GetAutoNumberInFormat("job_no", _AutoNumberContext);
            ShareModel.GetLike -= GetLike;
            _AutoNumberContext.Setdata("job_no", "last_numeral", last_number);

            List<int> jj;
            _JobContext.Getdata<int>("item_no", job.Item_no, "job_id", out jj);

            List<SQLClass.Models.Routing.Routing> rr = new List<Routing>();

            if (jj.Count >= 2)
            {
                _RoutingContext.GetAllRoutingByJobId(jj[jj.Count - 2], out rr);
            }

            List<int> jobid;
            _JobContext.Getdata<int>("job_no", job.Job_no, "job_id", out jobid);
            job.Job_id = jobid.LastOrDefault();

            foreach (Routing routing in rr)
            {
                GetCreateRouting(user, job.Job_no);
                RoutingViewModel.Job_id = job.Job_id;

                SQLClass.Models.Routing.Routing route_temp;
                List<SQLClass.Models.Routing.Routing> create_route_temp;
                _RoutingContext.GetRoutingByRoutingId(routing.Previous_routing_id, out route_temp);
                _RoutingContext.GetAllRoutingByJobId(RoutingViewModel.Job_id, out create_route_temp);
                var tt = create_route_temp.Find(x => x.Routing_name == route_temp.Routing_name);
                if(create_route_temp.Count == 0)
                    RoutingViewModel.Previous_routing_id = 0;
                else
                    RoutingViewModel.Previous_routing_id = tt.Routing_id;

                RoutingViewModel.Standard_setup_time = routing.Standard_setup_time;
                RoutingViewModel.Standard_unit_lead_time = routing.Standard_unit_lead_time;
                RoutingViewModel.Routing_name = routing.Routing_name;
                RoutingViewModel.Create_by = user.Id;
                RoutingViewModel.Create_date = DateTime.Now;
                
                _RoutingContext.CreateRouting(RoutingViewModel);
            }

            return flag;
        }

        public void GetEditJob(string no, MainFormUsers user)
        {
            SQLClass.Models.Job.Job temp;
            _JobContext.GetJobByJobNo(no, out temp);

            WorkViewModel.Job_id = temp.Job_id;
            WorkViewModel.Job_barcode = temp.Job_barcode;
            WorkViewModel.JobSourceTypeList = ShareModel.GetSelectList(SysCommonModel, "job_source_type");
            WorkViewModel.JobSourceTypeList.Where(x => Convert.ToInt16(x.Value) == temp.Job_source_type).FirstOrDefault().Selected = true;
            WorkViewModel.Job_source_type = temp.Job_source_type.Value;

            WorkViewModel.Job_source_id = temp.Job_source_id;
            WorkViewModel.Job_source_no = temp.Job_source_no;
            WorkViewModel.Job_no = temp.Job_no;
            WorkViewModel.Job_date = temp.Job_date;
            WorkViewModel.Job_released_date = temp.Job_released_date;
            WorkViewModel.Job_description = temp.Job_description;

            JobStatusList = ShareModel.GetJob_statusList(SysCommonModel);
            JobStatusFilterList = JobStatusList;
            JobStatusList.Where(x => Convert.ToInt16(x.Value) == temp.Job_status).FirstOrDefault().Selected = true;
            WorkViewModel.Job_status = temp.Job_status;

            WorkViewModel.Customer_no = temp.Customer_no;
            WorkViewModel.Customer_name = temp.Customer_name;
            WorkViewModel.Customer_order_id = temp.Customer_order_id;
            WorkViewModel.Customer_order_no = temp.Customer_order_no;
            WorkViewModel.Customer_shipping_date = temp.Customer_shipping_date;
            WorkViewModel.Customer_shipping_address = temp.Customer_shipping_address;

            if (EmployeeUserList.Find(x => x.Value == user.Id) != null)
                EmployeeUserList.Find(x => x.Value == user.Id).Selected = true;
            WorkViewModel.Employee_id = EmployeeUserList.Find(x => x.Value == temp.Employee_id).Value;

            WorkViewModel.Item_id= temp.Item_id;
            WorkViewModel.Item_no = temp.Item_no;
            WorkViewModel.Item_name = temp.Item_name;
            WorkViewModel.Item_description = temp.Item_description;
            WorkViewModel.Item_type = temp.Item_type;
            ItemTypeList = MainForm.Models.ShareModel.GetSelectList(SysCommonModel, "item_type");
            ItemTypeList.Find(x => x.Value == temp.Item_type.ToString()).Selected = true;

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
            WorkViewModel.Job_closed_date = temp.Job_closed_date;
            WorkViewModel.Is_classification_defective_item = temp.Is_classification_defective_item;
            WorkViewModel.Is_allow_split_submit = temp.Is_allow_split_submit;
            WorkViewModel.Is_define_desource_in_operations_submit = temp.Is_define_desource_in_operations_submit;
            WorkViewModel.Is_enable = temp.Is_enable;
            WorkViewModel.Create_by = temp.Create_by;
            WorkViewModel.Create_date = temp.Create_date;
            WorkViewModel.Last_update_by = user.Id;
            WorkViewModel.Last_update_date = DateTime.Now;
            WorkViewModel.Reserved_field01 = temp.Reserved_field01;
            WorkViewModel.Reserved_field02 = temp.Reserved_field02;

            GetRoutingByJob_id(temp.Job_id);
        }

        public bool PostEditJob(string no, IFormCollection data, MainFormUsers user)
        {
            bool flag = false;

            SQLClass.Models.Job.Job job = new SQLClass.Models.Job.Job()
            {
                Factory_id = Convert.ToInt16(data["WorkViewModel.Factory_id"]),
                Job_source_type = Convert.ToInt16(data["WorkViewModel.Job_source_type"]),
                Job_source_id = Convert.ToInt32(data["WorkViewModel.Job_source_id"]),
                Job_source_no = data["WorkViewModel.Job_source_no"],
                Job_no = data["WorkViewModel.Job_no"],
                Job_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Job_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Job_date"]),
                Job_released_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Job_released_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Job_released_date"]),
                Job_description = data["WorkViewModel.Job_description"],
                Job_status = Convert.ToInt16(data["WorkViewModel.Job_status"]),
                Customer_no = data["WorkViewModel.Customer_no"],
                Customer_name = data["WorkViewModel.Customer_name"],
                Customer_order_id = Convert.ToInt32(data["WorkViewModel.Customer_order_id"]),
                Customer_order_no = data["WorkViewModel.Customer_order_id"],
                Customer_shipping_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Customer_shipping_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Customer_shipping_date"]),
                Customer_shipping_address = data["WorkViewModel.Customer_shipping_address"],
                Employee_id = data["WorkViewModel.Employee_id"],
                Item_id = Convert.ToInt32(data["WorkViewModel.Item_id"]),
                Item_no = data["WorkViewModel.Item_no"],
                Item_name = data["WorkViewModel.Item_name"],
                Item_description = data["WorkViewModel.Item_description"],
                Item_type = Convert.ToInt32(data["WorkViewModel.Item_type"]),
                Customer_order_quantity = Convert.ToInt32(data["WorkViewModel.Customer_order_quantity"]),
                Customer_order_quantity_uom = data["WorkViewModel.Customer_order_quantity_uom"],
                Scheduled_start_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Scheduled_start_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Scheduled_start_date"]),
                Scheduled_completion_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Scheduled_completion_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Scheduled_completion_date"]),
                Scheduled_completion_quantity = Convert.ToInt32(data["WorkViewModel.Scheduled_completion_quantity"]),
                Scheduled_completion_quantity_uom = data["WorkViewModel.Scheduled_completion_quantity_uom"],
                Actually_start_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Actually_start_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Actually_start_date"]),
                Actually_completion_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Actually_completion_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Actually_completion_date"]),
                Actually_completion_quantity = Convert.ToInt32(data["WorkViewModel.Actually_completion_quantity"]),
                Actually_completion_quantity_uom = data["WorkViewModel.Actually_completion_quantity_uom"],
                Actually_defective_quantity = Convert.ToInt32(data["WorkViewModel.Actually_defective_quantity"]),
                Actually_defective_quantity_uom = data["WorkViewModel.Actually_defective_quantity_uom"],
                Input_of_inventory_quantity = Convert.ToInt32(data["WorkViewModel.Input_of_inventory_quantity"]),
                Input_of_inventory_quantity_uom = data["WorkViewModel.Input_of_inventory_quantity_uom"],
                Job_closed_date = StringValues.IsNullOrEmpty(data["WorkViewModel.Job_closed_date"]) ? (DateTime?)null : Convert.ToDateTime(data["WorkViewModel.Job_closed_date"]),
                Is_classification_defective_item = data["WorkViewModel.Is_classification_defective_item"].Contains("true"),
                Is_allow_split_submit = data["WorkViewModel.Is_allow_split_submit"].Contains("true"),
                Is_define_desource_in_operations_submit = data["WorkViewModel.Is_define_desource_in_operations_submit"].Contains("true"),
                Is_enable = data["WorkViewModel.Is_enable"].Contains("true"),
                Last_update_by = data["WorkViewModel.Last_update_by"],
                Reserved_field01 = data["WorkViewModel.Reserved_field01"],
                Reserved_field02 = data["WorkViewModel.Reserved_field02"]
            };

            job.Last_update_date = DateTime.Now;
            flag = _JobContext.EditJob(no, job);

            return flag;
        }

        public bool DisableJob(string no)
        {
            bool flag0 = false, flag1 = false;
            flag0 = _JobContext.Setdata(no, "is_enable", 0);
            flag1 = _JobContext.Setdata(no, "last_update_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return flag0 || flag1;
        }

        public bool ReleasedJob(string no)
        {
            bool flag0 = false, flag1 = false;
            flag0 = _JobContext.Setdata(no, "job_status", 2);
            flag1 = _JobContext.Setdata(no, "job_released_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return flag0 || flag1;
        }

        public bool ClosedJob(string no)
        {
            bool flag0 = false, flag1 = false;
            flag0 = _JobContext.Setdata(no, "job_status", 5);
            flag1 = _JobContext.Setdata(no, "job_closed_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return flag0 || flag1;
        }

        public void GetAllItem()
        {
            bool flag = false;
            List<SQLClass.Models.Item.Item> temp = new List<SQLClass.Models.Item.Item>();

            flag = _ItemContext.GetAllItem(out temp);
            ItemList = temp;
        }

        public SQLClass.Models.Item.Item GetItem(string no)
        {
            bool flag = false;
            SQLClass.Models.Item.Item temp = new SQLClass.Models.Item.Item();

            flag = _ItemContext.GetItemByItemNo(no , out temp);            

            return temp;
        }

        public List<SQLClass.Models.ItemUOM.ItemUOM> GetItem_UOM(string no)
        {
            bool flag = false;
            SQLClass.Models.Item.Item tt = new SQLClass.Models.Item.Item();

            _ItemContext.GetItemByItemNo(no, out tt);
            List<SQLClass.Models.ItemUOM.ItemUOM> temp = new List<ItemUOM>();

            flag = _ItemUOMContext.GetItemUOMByItemUOMId(tt.Item_id, out temp);
            return temp;
        }

        private Byte[] QRcode(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return BitmapToBytes(qrCodeImage);
        }

        public static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public string Barcode(string barText)
        {
            BarcodeFonts enc = new BarcodeFonts();
            enc.BarcodeType = BarcodeFonts.BarcodeEnum.Code128Auto;
            enc.CheckDigit = BarcodeFonts.YesNoEnum.No;
            enc.Data = barText;
            enc.encode();
            return enc.EncodedData;
        }

        public byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        ///////////////////////////////////////////////////////////for routing///////////////////////////////////////////////////////////
        public void GetRoutingByJob_id(int job_id)
        {
            List<SQLClass.Models.Routing.Routing> tempRoutingList;
            GetAllRouting(out tempRoutingList);
            RoutingList = new List<SQLClass.Models.Routing.Routing>();
            foreach (SQLClass.Models.Routing.Routing a in tempRoutingList)
            {
                if (a.Job_id == job_id)
                    RoutingList.Add(a);
            }
        }

        public bool GetAllRouting(out List<SQLClass.Models.Routing.Routing> tempRoutingList)
        {
            bool flag = false;
            List<SQLClass.Models.Routing.Routing> temp = new List<SQLClass.Models.Routing.Routing>();

            flag = _RoutingContext.GetAllRouting(out temp);
            tempRoutingList = temp;

            return flag;
       }

        public void GetCreateRouting(MainFormUsers user, string job_no)
        {
            SQLClass.Models.Job.Job temp;
            _JobContext.GetJobByJobNo(job_no, out temp);

            RoutingViewModel.Job_id = temp.Job_id;
            RoutingViewModel.Routing_no = GetRoutingAutoNumber(job_no);
            RoutingViewModel.Routing_name = "";
            RoutingViewModel.Routing_description = "";
            RoutingViewModel.Routing_workstation_id = 0;
            RoutingViewModel.Routing_workstation_no = "";
            RoutingViewModel.Routing_workstation_name = "";
            RoutingViewModel.Routing_workstation_description = "";
            RoutingViewModel.Standard_setup_time = 0;
            RoutingViewModel.Standard_unit_lead_time = 0;
            RoutingViewModel.Actually_setup_time = 0;
            RoutingViewModel.Actually_unit_lead_time = 0;
            RoutingViewModel.Scheduled_completion_quantity_uom = temp.Scheduled_completion_quantity_uom;
            RoutingViewModel.Actually_completion_quantity = 0;
            RoutingViewModel.Actually_completion_quantity_uom = "";
            RoutingViewModel.Actually_defective_quantity = 0;
            RoutingViewModel.Actually_defective_quantity_uom = "";
            RoutingViewModel.Transfer_out_quantity = 0;
            RoutingViewModel.Transfer_out_quantity_uom = "";
            RoutingViewModel.Is_classification_defective_item = false;
            RoutingViewModel.Is_allow_split_submit = true;
            RoutingViewModel.Is_define_resource_in_opeations_submit = true;
            RoutingViewModel.Is_enable = true;
            RoutingViewModel.Create_by = user.Id;
            RoutingViewModel.Create_date = DateTime.Today.Date;

            List<SQLClass.Models.Routing.Routing> routing_temp;
            RoutingListInPrevious = new List<SelectListItem>();
            _RoutingContext.GetLikeRoutingByRoutingNo(job_no, out routing_temp);
            //RoutingListInPrevious.Add(new SelectListItem() { Value = "0", Text = "首站" });
            for (int i = 0; i < routing_temp.Count; i++)
            {
                RoutingListInPrevious.Add(new SelectListItem() { Value = routing_temp[i].Routing_id.ToString(), Text = routing_temp[i].Routing_name});
            }
        }

        private string GetRoutingAutoNumber(string job_no)
        {
            List<SQLClass.Models.Routing.Routing> temp;

            _RoutingContext.GetLikeRoutingByRoutingNo(job_no, out temp);

            if (temp.Count > 0)
            {
                string no = temp.Last().Routing_no;
                //int tt = Convert.ToInt32(no.Substring((no.Length - 2), 2));
                int tt = Convert.ToInt32(no.Split('-')[1]);

                return job_no + "-" + (tt + 10).ToString("000");
            }
            else
            {
                return job_no + "-010";
            }
        }

        public bool PostCreateRouting(IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Job.Job temp;
            _JobContext.GetJobByJobId(Convert.ToInt32(data["RoutingViewModel.Job_id"]), out temp);

            SQLClass.Models.Routing.Routing routing = new SQLClass.Models.Routing.Routing()
            {
                Job_id = Convert.ToInt32(data["RoutingViewModel.Job_id"]),
                Previous_routing_id = Convert.ToInt32(data["RoutingViewModel.Previous_routing_id"]),
                Routing_no = data["RoutingViewModel.Routing_no"],
                Routing_name = data["RoutingViewModel.Routing_name"],
                Routing_description = data["RoutingViewModel.Routing_description"],
                Standard_setup_time = Convert.ToInt32(data["RoutingViewModel.Standard_setup_time"]),
                Standard_unit_lead_time = Convert.ToInt32(data["RoutingViewModel.Standard_unit_lead_time"]),
                Actually_setup_time = Convert.ToInt32(data["RoutingViewModel.Actually_setup_time"]),
                Actually_unit_lead_time = Convert.ToInt32(data["RoutingViewModel.Actually_unit_lead_time"]),
                Scheduled_completion_quantity_uom = temp.Scheduled_completion_quantity_uom,
                Actually_completion_quantity = Convert.ToInt32(data["RoutingViewModel.Actually_completion_quantity"]),
                Actually_completion_quantity_uom = temp.Actually_completion_quantity_uom,
                Actually_defective_quantity = Convert.ToInt32(data["RoutingViewModel.Actually_defective_quantity"]),
                Actually_defective_quantity_uom = temp.Actually_defective_quantity_uom,
                Transfer_out_quantity = Convert.ToInt32(data["RoutingViewModel.Transfer_out_quantity"]),
                Transfer_out_quantity_uom = temp.Input_of_inventory_quantity_uom,
                Is_classification_defective_item = temp.Is_classification_defective_item,
                Is_allow_split_submit = temp.Is_allow_split_submit,
                Is_define_resource_in_opeations_submit = temp.Is_define_desource_in_operations_submit,
                Is_enable = data["RoutingViewModel.Is_enable"].Contains("true"),
                Create_by = data["RoutingViewModel.Create_by"]
            };

            routing.Create_date = DateTime.Now;

            if(routing.Previous_routing_id == 0)
            {
                routing.Scheduled_completion_quantity = temp.Scheduled_completion_quantity;
            }

            flag = _RoutingContext.CreateRouting(routing);

            GetRoutingByJob_id(routing.Job_id);

            return flag;
        }

        public void GetEditRouting(string route_no, MainFormUsers user)
        {
            SQLClass.Models.Routing.Routing temp = new SQLClass.Models.Routing.Routing();
            _RoutingContext.GetRoutingByRoutingNo(route_no, out temp);

            //RoutingViewModel.Routing_id = "";
            RoutingViewModel.Job_id = temp.Job_id;
            RoutingViewModel.Previous_routing_id = temp.Previous_routing_id;
            RoutingViewModel.Routing_no = temp.Routing_no;
            RoutingViewModel.Routing_name = temp.Routing_name;
            RoutingViewModel.Routing_description = temp.Routing_description;
            RoutingViewModel.Routing_workstation_id = temp.Routing_workstation_id;
            RoutingViewModel.Routing_workstation_no = temp.Routing_workstation_no;
            RoutingViewModel.Routing_workstation_name = temp.Routing_workstation_name;
            RoutingViewModel.Routing_workstation_description = temp.Routing_workstation_description;
            RoutingViewModel.Standard_setup_time = temp.Standard_setup_time;
            RoutingViewModel.Standard_unit_setup_time = temp.Standard_unit_setup_time;
            RoutingViewModel.Standard_unit_run_time = temp.Standard_unit_run_time;
            RoutingViewModel.Standard_unit_total_time = temp.Standard_unit_total_time;
            RoutingViewModel.Standard_unit_lead_time = temp.Standard_unit_lead_time;
            RoutingViewModel.Actually_setup_time = temp.Actually_setup_time;
            RoutingViewModel.Actually_unit_setup_time = temp.Actually_unit_setup_time;
            RoutingViewModel.Actually_unit_run_time = temp.Actually_unit_run_time;
            RoutingViewModel.Actually_unit_total_time = temp.Actually_unit_total_time;
            RoutingViewModel.Actually_unit_lead_time = temp.Actually_unit_lead_time;
            RoutingViewModel.Scheduled_start_date = temp.Scheduled_start_date;
            RoutingViewModel.Scheduled_completion_date = temp.Scheduled_completion_date;
            RoutingViewModel.Actually_start_date = temp.Actually_start_date;
            RoutingViewModel.Actually_completion_date = temp.Actually_completion_date;
            RoutingViewModel.Scheduled_completion_quantity = temp.Scheduled_completion_quantity;
            RoutingViewModel.Scheduled_completion_quantity_uom = temp.Scheduled_completion_quantity_uom;
            RoutingViewModel.Actually_completion_quantity = temp.Actually_completion_quantity;
            RoutingViewModel.Actually_completion_quantity_uom = temp.Actually_completion_quantity_uom;
            RoutingViewModel.Actually_defective_quantity = temp.Actually_defective_quantity;
            RoutingViewModel.Actually_defective_quantity_uom = temp.Actually_defective_quantity_uom;
            RoutingViewModel.Transfer_out_quantity = temp.Transfer_out_quantity;
            RoutingViewModel.Transfer_out_quantity_uom = temp.Transfer_out_quantity_uom;
            RoutingViewModel.Is_classification_defective_item = temp.Is_classification_defective_item;
            RoutingViewModel.Is_allow_split_submit = temp.Is_allow_split_submit;
            RoutingViewModel.Is_define_resource_in_opeations_submit = temp.Is_define_resource_in_opeations_submit;
            RoutingViewModel.Is_enable = temp.Is_enable;
            RoutingViewModel.Create_by = user.Id;
            RoutingViewModel.Create_date = temp.Create_date;
            RoutingViewModel.Last_update_by = user.Id;
            RoutingViewModel.Last_update_date = DateTime.Today;

            List<string> job_temp;
            List<SQLClass.Models.Routing.Routing> routing_temp;
            _JobContext.Getdata<string>("job_id", temp.Job_id.ToString(), "job_no", out job_temp);
            RoutingListInPrevious = new List<SelectListItem>();
            _RoutingContext.GetLikeRoutingByRoutingNo(job_temp.FirstOrDefault(), out routing_temp);
            //RoutingListInPrevious.Add(new SelectListItem() { Value = "0", Text = "首站" });
            for (int i = 0; i < routing_temp.Count; i++)
            {
                RoutingListInPrevious.Add(new SelectListItem() { Value = routing_temp[i].Routing_id.ToString(), Text = routing_temp[i].Routing_name });
            }
        }

        public bool PostEditRouting(string no, IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Routing.Routing routing = new SQLClass.Models.Routing.Routing()
            {
                //RoutingViewModel.Routing_id = "",
                Job_id = Convert.ToInt32(data["RoutingViewModel.Job_id"]),
                Previous_routing_id = Convert.ToInt32(data["RoutingViewModel.Previous_routing_id"]),
                Routing_no = data["RoutingViewModel.Routing_no"],
                Routing_name = data["RoutingViewModel.Routing_name"],
                Routing_description = data["RoutingViewModel.Routing_description"],
                Routing_workstation_id = Convert.ToInt32(data["RoutingViewModel.Routing_workstation_id"]),
                Routing_workstation_no = data["RoutingViewModel.Routing_workstation_no"],
                Routing_workstation_name = data["RoutingViewModel.Routing_workstation_name"],
                Routing_workstation_description = data["RoutingViewModel.Routing_workstation_description"],
                Standard_setup_time = Convert.ToInt32(data["RoutingViewModel.Standard_setup_time"]),
                Standard_unit_setup_time = Convert.ToInt32(data["RoutingViewModel.Standard_unit_setup_time"]),
                Standard_unit_run_time = Convert.ToInt32(data["RoutingViewModel.Standard_unit_run_time"]),
                Standard_unit_total_time = Convert.ToInt32(data["RoutingViewModel.Standard_unit_total_time"]),
                Standard_unit_lead_time = Convert.ToInt32(data["RoutingViewModel.Standard_unit_lead_time"]),
                Actually_setup_time = Convert.ToInt32(data["RoutingViewModel.Actually_setup_time"]),
                Actually_unit_setup_time = Convert.ToInt32(data["RoutingViewModel.Actually_unit_setup_time"]),
                Actually_unit_run_time = Convert.ToInt32(data["RoutingViewModel.Actually_unit_run_time"]),
                Actually_unit_total_time = Convert.ToInt32(data["RoutingViewModel.Actually_unit_total_time"]),
                Actually_unit_lead_time = Convert.ToInt32(data["RoutingViewModel.Actually_unit_lead_time"]),
                Scheduled_start_date = StringValues.IsNullOrEmpty(data["RoutingViewModel.Scheduled_start_date"]) ? (DateTime?)null : Convert.ToDateTime(data["RoutingViewModel.Scheduled_start_date"]),
                Scheduled_completion_date = StringValues.IsNullOrEmpty(data["RoutingViewModel.Scheduled_completion_date"]) ? (DateTime?)null : Convert.ToDateTime(data["RoutingViewModel.Scheduled_completion_date"]),
                Actually_start_date = StringValues.IsNullOrEmpty(data["RoutingViewModel.Actually_start_date"]) ? (DateTime?)null : Convert.ToDateTime(data["RoutingViewModel.Actually_start_date"]),
                Actually_completion_date = StringValues.IsNullOrEmpty(data["RoutingViewModel.Actually_completion_date"]) ? (DateTime?)null : Convert.ToDateTime(data["RoutingViewModel.Actually_completion_date"]),
                Scheduled_completion_quantity = Convert.ToInt32(data["RoutingViewModel.Scheduled_completion_quantity"]),
                Scheduled_completion_quantity_uom = data["RoutingViewModel.Scheduled_completion_quantity_uom"],
                Actually_completion_quantity = Convert.ToInt32(data["RoutingViewModel.Actually_completion_quantity"]),
                Actually_completion_quantity_uom = data["RoutingViewModel.Actually_completion_quantity_uom"],
                Actually_defective_quantity = Convert.ToInt32(data["RoutingViewModel.Actually_defective_quantity"]),
                Actually_defective_quantity_uom = data["RoutingViewModel.Actually_defective_quantity_uom"],
                Transfer_out_quantity = Convert.ToInt32(data["RoutingViewModel.Transfer_out_quantity"]),
                Transfer_out_quantity_uom = data["RoutingViewModel.Transfer_out_quantity_uom"],
                Is_classification_defective_item = data["RoutingViewModel.Is_classification_defective_item"].Contains("true"),
                Is_allow_split_submit = data["RoutingViewModel.Is_allow_split_submit"].Contains("true"),
                Is_define_resource_in_opeations_submit = data["RoutingViewModel.Is_define_resource_in_opeations_submit"].Contains("true"),
                Is_enable = data["RoutingViewModel.Is_enable"].Contains("true"),
                Create_by = data["RoutingViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["RoutingViewModel.Create_date"]),
                Last_update_by = data["RoutingViewModel.Last_update_by"]
            };

            routing.Last_update_date = DateTime.Now;
            flag = _RoutingContext.EditRouting(no, routing);

            return flag;
        }

        public bool DisableRouting(string no)
        {
            bool flag0 = false, flag1 = false;
            flag0 = _RoutingContext.Setdata(no, "is_enable", 0);
            flag1 = _RoutingContext.Setdata(no, "last_update_by", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return flag0 || flag1;
        }
    }
}
