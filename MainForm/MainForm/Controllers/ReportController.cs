using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Job;
using MainForm.ViewModels.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLClass.Models.Company;
using SQLClass.Models.Routing;
using Microsoft.AspNetCore.Http;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Item;
using SQLClass.Models.ItemUOM;
using SQLClass.Models.AutoNumber;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using MainForm.Models.Report;
using SQLClass.Models.Job;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.Util;
using MainForm.ViewModels.Job;
using SQLClass.Models.OperationsResource;
using SQLClass.Models.OperationsSubmit;
using SQLClass.Models.OperationsDetail;
using SQLClass.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainForm.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public static ReportModel Model { get; set; }

        public static ReportViewModel ViewModel { get; set; }

        private readonly ILogger<ReportController> _logger;

        private readonly IMapper _mapper;

        public ReportController(IJobManage JobContext, IRoutingManage RoutingContext, 
            IOperationsSubmitManage SubmitContext, IOperationsDetailManage DetailContext, 
            ILogger<ReportController> logger, ISysCommonManage SysCommonContext, IUsersManage UsersManage, 
            IOperationsResourceManage ResourceContext, IMapper mapper)
        {
            _logger ??= logger;
            _mapper ??= mapper;

            Model ??= new ReportModel(JobContext, RoutingContext, SubmitContext, DetailContext, SysCommonContext, UsersManage, ResourceContext);

            ViewModel ??= new ReportViewModel();

            Filtered_Data ??= new List<ReportClassModel>();
        }

        public static List<ReportClassModel> Filtered_Data;

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> Index(string Item_no_filter, string Customer_no_filter, 
            List<string> Operations_resource_filter, DateTime Actually_start_date_start_filter, DateTime Actually_start_date_end_filter,
            int? page = 1, string sort = "-Job_no")
        {
            if (Request.Method == "POST")
            {
                Model.GetJobByResource(Operations_resource_filter);

                ViewModel = _mapper.Map<ReportViewModel>(Model);
                ViewModel.Sort_str = sort;

                List<ReportClassModel> Filtered_Data = new List<ReportClassModel>();
                Filtered_Data = Filter_Data(Model.ReportList, Item_no_filter, Customer_no_filter, Operations_resource_filter, Actually_start_date_start_filter, Actually_start_date_end_filter);

                ViewModel.ReportList = Filtered_Data;
            }
            else
            {
                ViewModel = _mapper.Map<ReportViewModel>(Model);
                ViewModel.Sort_str = sort;
            }

            return await Task.Run(() => View(ViewModel));
        }

        public List<ReportClassModel> Filter_Data(List<ReportClassModel> Data, string Item_no_filter,
            string Customer_no_filter, List<string> Operations_resource_filter,
            DateTime Actually_start_date_start_filter, DateTime Actually_start_date_end_filter)
        {
            if ((Actually_start_date_start_filter != null) && (Actually_start_date_start_filter != DateTime.MinValue)
                && (Actually_start_date_end_filter != null) && (Actually_start_date_end_filter != DateTime.MinValue))
            {
                List<ReportClassModel> Filtered_Data_Date = new List<ReportClassModel>();
                Filtered_Data = new List<ReportClassModel>();

                //將選擇日期加入起始(00:00:00)與結束時間(23:59:59)
                string[] start_Date_string = (Actually_start_date_start_filter.Date.ToString()).Split(' ');
                string[] end_Date_string = (Actually_start_date_end_filter.Date.ToString()).Split(' ');
                DateTime Date_start = Convert.ToDateTime(start_Date_string[0] + " 00:00:00");
                DateTime Date_end = Convert.ToDateTime(end_Date_string[0] + " 23:59:59");

                foreach (ReportClassModel date in Data)
                {
                    if ((DateTime.Compare(Convert.ToDateTime(date.Submit.Actually_start_date), Date_start) >= 0) &&
                        (DateTime.Compare(Convert.ToDateTime(date.Submit.Actually_start_date), Date_end) <= 0)) //判斷日期大小
                    {
                        Filtered_Data.Add(date);
                    }
                }

                ViewModel.Actually_start_date_start_filter = Actually_start_date_start_filter;
                ViewModel.Actually_start_date_end_filter = Actually_start_date_end_filter;
            }
            else
            {
                Filtered_Data = Data;
            } 

            if (!string.IsNullOrWhiteSpace(Item_no_filter))
            {
                Filtered_Data = Filtered_Data.Where(f => f.Job.Item_no.Contains(Item_no_filter)).ToList();
                ViewModel.Item_no_filter = Item_no_filter;
            }

            if (!string.IsNullOrWhiteSpace(Customer_no_filter))
            {
                Filtered_Data = Filtered_Data.Where(f => f.Job.Customer_no.Contains(Customer_no_filter)).ToList();
                ViewModel.Customer_no_filter = Customer_no_filter;
            }

            foreach (ReportClassModel a in Filtered_Data)
            {
                if ((a.Submit.Actually_completion_date.HasValue) && (a.Submit.Actually_start_date.HasValue))
                {
                    a.Total_time = (a.Submit.Actually_completion_date - a.Submit.Actually_start_date).Value.TotalHours;
                }
                else
                {
                    a.Total_time = 0;
                }
            }

            return Filtered_Data;
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetExport(IFormCollection data, string Item_no_filter,
            DateTime Create_date, string Customer_no_filter, string Job_status_filter)
        {
            if (Filtered_Data.Count > 0)
            {
                Create_file(Filtered_Data);

                byte[] fileBytes = System.IO.File.ReadAllBytes(Filename);
                string fileName = Filename;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);  //下載Excel
            }
            else
            {
                return await Task.Run(() => View("Index", ViewModel));
            }
        }

        string Filename = "每日數控產值紀錄表.xlsx";
        public void Create_file(List<ReportClassModel> Data)
        {
            IWorkbook Workbook = new XSSFWorkbook();
            ISheet WorkSheet = Workbook.CreateSheet("Sheet1");
            WorkSheet.PrintSetup.Landscape = true;
            WorkSheet.PrintSetup.PaperSize = (short)PaperSize.A4_TRANSVERSE_PAPERSIZE;

            WorkSheet.SetMargin(MarginType.TopMargin, 0.3);
            WorkSheet.SetMargin(MarginType.BottomMargin, 0.3);
            WorkSheet.SetMargin(MarginType.RightMargin, 0.2);
            WorkSheet.SetMargin(MarginType.LeftMargin, 0.2);
            WorkSheet.PrintSetup.Landscape = true;
            //設定樣式(表單標題格式)
            ICellStyle headerStyle = Workbook.CreateCellStyle();
            IFont headerfont = Workbook.CreateFont();
            headerStyle.Alignment = HorizontalAlignment.Center; //水平置中
            headerStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            headerfont.FontName = "標楷體";
            headerfont.FontHeightInPoints = 14;
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            headerStyle.SetFont(headerfont);

            //設定樣式(表單內容格式)
            ICellStyle headerStyle_content = Workbook.CreateCellStyle();
            IFont headerfont_content = Workbook.CreateFont();
            headerStyle_content.Alignment = HorizontalAlignment.Left; //水平置中
            headerStyle_content.VerticalAlignment = VerticalAlignment.Center; //垂直置中           
            headerfont_content.FontName = "標楷體";
            headerfont_content.FontHeightInPoints = 10;
            headerStyle_content.SetFont(headerfont_content);

            //設定樣式(表單內容格式-title)自動換行
            ICellStyle headerStyle_content2 = Workbook.CreateCellStyle();
            IFont headerfont_content2 = Workbook.CreateFont();
            headerStyle_content2.Alignment = HorizontalAlignment.Center; //水平置中
            headerStyle_content2.VerticalAlignment = VerticalAlignment.Center; //垂直置中       
            headerStyle_content2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;//粗
            headerStyle_content2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//細實線
            headerStyle_content2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//虛線
            headerStyle_content2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//... 
            headerfont_content2.FontName = "標楷體";
            headerfont_content2.FontHeightInPoints = 10;
            headerStyle_content2.WrapText = true;
            headerStyle_content2.SetFont(headerfont_content2);

            //設定樣式(表單_值_格式)
            ICellStyle headerStyle_content3 = Workbook.CreateCellStyle();
            IFont headerfont_content3 = Workbook.CreateFont();
            headerStyle_content3.Alignment = HorizontalAlignment.Left;
            headerStyle_content3.VerticalAlignment = VerticalAlignment.Center; //垂直置中           
            headerfont_content3.FontName = "標楷體";
            headerfont_content3.FontHeightInPoints = 10;
            headerStyle_content3.SetFont(headerfont_content3);

            //垂直輸入自動換行
            ICellStyle headerStyle_content4 = Workbook.CreateCellStyle();
            IFont headerfont_content4 = Workbook.CreateFont();
            headerStyle_content4.Alignment = HorizontalAlignment.Center; //水平置中
            headerStyle_content4.VerticalAlignment = VerticalAlignment.Top; //垂直置中           
            headerfont_content4.FontName = "標楷體";
            headerfont_content4.FontHeightInPoints = 6;
            headerStyle_content4.SetFont(headerfont_content4);
            headerStyle_content4.WrapText = true;

            //設定樣式(表單內容格式-值)自動換行
            ICellStyle headerStyle_content5 = Workbook.CreateCellStyle();
            IFont headerfont_content5 = Workbook.CreateFont();
            headerStyle_content5.Alignment = HorizontalAlignment.Left; //水平置中
            headerStyle_content5.VerticalAlignment = VerticalAlignment.Center; //垂直置中       
            headerStyle_content5.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;//粗
            headerStyle_content5.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//細實線
            headerStyle_content5.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//虛線
            headerStyle_content5.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//... 
            headerfont_content5.FontName = "標楷體";
            headerfont_content5.FontHeightInPoints = 10;
            headerStyle_content5.WrapText = true;
            headerStyle_content5.SetFont(headerfont_content5);

            //設定樣式(表單內容格式-值)自動換行for訂單號碼 製造爐號
            ICellStyle headerStyle_content6 = Workbook.CreateCellStyle();
            IFont headerfont_content6 = Workbook.CreateFont();
            headerStyle_content6.Alignment = HorizontalAlignment.Left;
            headerStyle_content6.VerticalAlignment = VerticalAlignment.Center; //垂直置中           
            headerfont_content6.FontName = "標楷體";
            headerfont_content6.FontHeightInPoints = 6;
            headerStyle_content6.WrapText = true;
            headerStyle_content6.SetFont(headerfont_content6);

            ICellStyle headerStyle_contentLEFT = Workbook.CreateCellStyle();
            IFont headerfont_contentLEFT = Workbook.CreateFont();
            headerStyle_contentLEFT.Alignment = HorizontalAlignment.Left; //水平置中
            headerStyle_contentLEFT.VerticalAlignment = VerticalAlignment.Center; //垂直置中       
            headerStyle_contentLEFT.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//細實線
            headerStyle_contentLEFT.BorderBottom = BorderStyle.None;
            headerfont_contentLEFT.FontName = "標楷體";
            headerfont_contentLEFT.FontHeightInPoints = 8;
            headerStyle_contentLEFT.WrapText = true;
            headerStyle_contentLEFT.SetFont(headerfont_contentLEFT);

            ICellStyle headerStyle_contentLEFTBOTTOM = Workbook.CreateCellStyle();
            IFont headerfont_contentLEFTBOTTOM = Workbook.CreateFont();
            headerStyle_contentLEFTBOTTOM.Alignment = HorizontalAlignment.Left; //水平置中
            headerStyle_contentLEFTBOTTOM.VerticalAlignment = VerticalAlignment.Center; //垂直置中       
            headerStyle_contentLEFTBOTTOM.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//細實線
            headerStyle_contentLEFTBOTTOM.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headerfont_contentLEFTBOTTOM.FontName = "標楷體";
            headerfont_contentLEFTBOTTOM.FontHeightInPoints = 10;
            headerStyle_contentLEFTBOTTOM.WrapText = true;
            headerStyle_contentLEFTBOTTOM.SetFont(headerfont_contentLEFTBOTTOM);

            ICellStyle headerStyle_contentBOTTOM = Workbook.CreateCellStyle();
            IFont headerfont_contentBOTTOM = Workbook.CreateFont();
            headerStyle_contentBOTTOM.Alignment = HorizontalAlignment.Left; //水平置中
            headerStyle_contentBOTTOM.VerticalAlignment = VerticalAlignment.Center; //垂直置中       
            headerStyle_contentBOTTOM.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headerfont_contentBOTTOM.FontName = "標楷體";
            headerfont_contentBOTTOM.FontHeightInPoints = 10;
            headerStyle_contentBOTTOM.WrapText = true;
            headerStyle_contentBOTTOM.SetFont(headerfont_contentBOTTOM);

            //設定欄位寬度
            WorkSheet.SetColumnWidth(0, Convert.ToInt32((6 + 0.72) * 256));
            WorkSheet.SetColumnWidth(1, Convert.ToInt32((10 + 0.72) * 256));
            WorkSheet.SetColumnWidth(2, Convert.ToInt32((10 + 0.72) * 256));
            WorkSheet.SetColumnWidth(3, Convert.ToInt32((20 + 0.72) * 256));
            WorkSheet.SetColumnWidth(4, Convert.ToInt32((15 + 0.72) * 256));
            WorkSheet.SetColumnWidth(5, Convert.ToInt32((12 + 0.72) * 256));
            WorkSheet.SetColumnWidth(6, Convert.ToInt32((15 + 0.72) * 256));
            WorkSheet.SetColumnWidth(7, Convert.ToInt32((15 + 0.72) * 256));
            WorkSheet.SetColumnWidth(8, Convert.ToInt32((15 + 0.72) * 256));
            WorkSheet.SetColumnWidth(9, Convert.ToInt32((6.5 + 0.72) * 256));
            WorkSheet.SetColumnWidth(10, Convert.ToInt32((6.5 + 0.72) * 256));
            WorkSheet.SetColumnWidth(11, Convert.ToInt32((6.5 + 0.72) * 256));
            WorkSheet.SetColumnWidth(12, Convert.ToInt32((6 + 0.72) * 256));
            WorkSheet.SetColumnWidth(13, Convert.ToInt32((6 + 0.72) * 256));
            WorkSheet.SetColumnWidth(14, Convert.ToInt32((6 + 0.72) * 256));
            WorkSheet.SetColumnWidth(15, Convert.ToInt32((6 + 0.72) * 256));
            WorkSheet.SetColumnWidth(16, Convert.ToInt32((10 + 0.72) * 256));
            WorkSheet.SetColumnWidth(17, Convert.ToInt32((10 + 0.72) * 256));
            WorkSheet.SetColumnWidth(18, Convert.ToInt32((10 + 0.72) * 256));

            //新增標題列
            WorkSheet.CreateRow(0); //需先用CreateRow建立,才可通过GetRow取得該欄位
            WorkSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 18)); //合併1~1列及B~I欄儲存格
            WorkSheet.GetRow(0).CreateCell(0).SetCellValue("每日數控產值紀錄表");
            WorkSheet.GetRow(0).GetCell(0).CellStyle = headerStyle; //套用樣式

            WorkSheet.CreateRow(1);
            WorkSheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 18)); //合併2~2列及B~I欄儲存格
            WorkSheet.GetRow(1).CreateCell(0).SetCellValue("　　　年　　　月　　　日");
            WorkSheet.GetRow(1).GetCell(0).CellStyle = headerStyle; //套用樣式

            WorkSheet.CreateRow(2);
            WorkSheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2)); //合併1~1列及B~I欄儲存格
            WorkSheet.GetRow(2).CreateCell(0).SetCellValue("機台編號：");
            WorkSheet.GetRow(2).GetCell(0).CellStyle = headerStyle_content; //套用樣式

            WorkSheet.CreateRow(3);
            WorkSheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 2)); //合併1~1列及B~I欄儲存格
            WorkSheet.GetRow(3).CreateCell(0).SetCellValue("校刀簽章：");
            WorkSheet.GetRow(3).GetCell(0).CellStyle = headerStyle_content; //套用樣式

            WorkSheet.CreateRow(4);
            WorkSheet.CreateRow(5);

            WorkSheet.GetRow(4).CreateCell(0).SetCellValue("日期");
            WorkSheet.GetRow(4).GetCell(0).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(0);
            WorkSheet.GetRow(5).GetCell(0).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(1).SetCellValue("途程");
            WorkSheet.GetRow(4).GetCell(1).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(1);
            WorkSheet.GetRow(5).GetCell(1).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(2).SetCellValue("累計時間\r\n(Hr)");
            WorkSheet.GetRow(4).GetCell(2).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(2);
            WorkSheet.GetRow(5).GetCell(2).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(3).SetCellValue("開始-結束");
            WorkSheet.GetRow(4).GetCell(3).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(3);
            WorkSheet.GetRow(5).GetCell(3).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(4).SetCellValue("機台編號");
            WorkSheet.GetRow(4).GetCell(4).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(4);
            WorkSheet.GetRow(5).GetCell(4).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 4, 4)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(5).SetCellValue("客戶");
            WorkSheet.GetRow(4).GetCell(5).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(5);
            WorkSheet.GetRow(5).GetCell(5).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 5, 5)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(6).SetCellValue("貨品編號");
            WorkSheet.GetRow(4).GetCell(6).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(6);
            WorkSheet.GetRow(5).GetCell(6).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 6, 6)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(7).SetCellValue("貨品名稱");
            WorkSheet.GetRow(4).GetCell(7).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(7);
            WorkSheet.GetRow(5).GetCell(7).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 7, 7)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(8).SetCellValue("鑄件商及採單");
            WorkSheet.GetRow(4).GetCell(8).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(8);
            WorkSheet.GetRow(5).GetCell(8).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 8, 8)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(9).SetCellValue("領料數量");
            WorkSheet.GetRow(4).GetCell(9).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(9);
            WorkSheet.GetRow(5).GetCell(9).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 9, 9)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(10).SetCellValue("良品");
            WorkSheet.GetRow(4).GetCell(10).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(10);
            WorkSheet.GetRow(5).GetCell(10).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 10, 10)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(11).SetCellValue("不良品");
            WorkSheet.GetRow(4).GetCell(11).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(11);
            WorkSheet.GetRow(5).GetCell(11).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 11, 11)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(12).SetCellValue("刀具耗用");
            WorkSheet.GetRow(4).GetCell(12).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(4).CreateCell(13);
            WorkSheet.GetRow(4).GetCell(13).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(4).CreateCell(14);
            WorkSheet.GetRow(4).GetCell(14).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(4).CreateCell(15);
            WorkSheet.GetRow(4).GetCell(15).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 4, 12, 15)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(5).CreateCell(12).SetCellValue("0/角");
            WorkSheet.GetRow(5).GetCell(12).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(13).SetCellValue("△/角");
            WorkSheet.GetRow(5).GetCell(13).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(14).SetCellValue("◇/角");
            WorkSheet.GetRow(5).GetCell(14).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(15).SetCellValue("/角");
            WorkSheet.GetRow(5).GetCell(15).CellStyle = headerStyle_content2; //套用樣式

            WorkSheet.GetRow(4).CreateCell(16).SetCellValue("簽章");
            WorkSheet.GetRow(4).GetCell(16).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(16);
            WorkSheet.GetRow(5).GetCell(16).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 16, 16)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(17).SetCellValue("材料重量");
            WorkSheet.GetRow(4).GetCell(17).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(17);
            WorkSheet.GetRow(5).GetCell(17).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 17, 17)); //合併1~1列及B~I欄儲存格

            WorkSheet.GetRow(4).CreateCell(18).SetCellValue("加工完重量");
            WorkSheet.GetRow(4).GetCell(18).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(5).CreateCell(18);
            WorkSheet.GetRow(5).GetCell(18).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(4, 5, 18, 18)); //合併1~1列及B~I欄儲存格

            int index = 0;
            foreach (ReportClassModel i in Data)
            {
                index = Data.IndexOf(i) + 6;
                WorkSheet.CreateRow(index);
                WorkSheet.GetRow(index).CreateCell(0).SetCellValue(i.Submit.Actually_start_date.Value.ToString("MM/dd"));
                WorkSheet.GetRow(index).GetCell(0).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(1).SetCellValue(
                    i.Routing.Routing_name + "\r\n"
                    + ViewModel.SubmitTypeList.Find(x => x.Value == i.Submit.Operations_submit_type.ToString()).Text);
                WorkSheet.GetRow(index).GetCell(1).CellStyle = headerStyle_content5; //套用樣式
                WorkSheet.GetRow(index).GetCell(1).CellStyle.Alignment = HorizontalAlignment.Center;

                WorkSheet.GetRow(index).CreateCell(2).SetCellValue(i.Total_time.ToString("0.0"));
                WorkSheet.GetRow(index).GetCell(2).CellStyle = headerStyle_content5; //套用樣式
                WorkSheet.GetRow(index).GetCell(2).CellStyle.Alignment = HorizontalAlignment.Center;

                WorkSheet.GetRow(index).CreateCell(3).SetCellValue(i.Submit.Actually_start_date + "\r\n|\r\n" + i.Submit.Actually_completion_date);
                WorkSheet.GetRow(index).GetCell(3).CellStyle = headerStyle_content5; //套用樣式   

                WorkSheet.GetRow(index).CreateCell(4).SetCellValue(i.MachineDetail.Operations_resource_no);
                WorkSheet.GetRow(index).GetCell(4).CellStyle = headerStyle_content5; //套用樣式   

                WorkSheet.GetRow(index).CreateCell(5).SetCellValue(i.Job.Customer_no);
                WorkSheet.GetRow(index).GetCell(5).CellStyle = headerStyle_content5; //套用樣式    
                
                WorkSheet.GetRow(index).CreateCell(6).SetCellValue(i.Job.Item_no);
                WorkSheet.GetRow(index).GetCell(6).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(7).SetCellValue(i.Job.Item_name);
                WorkSheet.GetRow(index).GetCell(7).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(8).SetCellValue(i.Job.Reserved_field01);
                WorkSheet.GetRow(index).GetCell(8).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(9).SetCellValue(i.Submit.Scheduled_completion_quantity);
                WorkSheet.GetRow(index).GetCell(9).CellStyle = headerStyle_content5; //套用樣式
                WorkSheet.GetRow(index).GetCell(9).CellStyle.Alignment = HorizontalAlignment.Center;

                WorkSheet.GetRow(index).CreateCell(10).SetCellValue(i.Submit.Actually_completion_quantity);
                WorkSheet.GetRow(index).GetCell(10).CellStyle = headerStyle_content5; //套用樣式
                WorkSheet.GetRow(index).GetCell(10).CellStyle.Alignment = HorizontalAlignment.Center;

                WorkSheet.GetRow(index).CreateCell(11).SetCellValue(i.Submit.Actually_defective_quantity);
                WorkSheet.GetRow(index).GetCell(11).CellStyle = headerStyle_content5; //套用樣式   
                WorkSheet.GetRow(index).GetCell(11).CellStyle.Alignment = HorizontalAlignment.Center;

                for(int j = 0; j < i.ToolDetailList.Count; j++)
                {
                    WorkSheet.GetRow(index).CreateCell(12 + j).SetCellValue(
                        i.ToolDetailList[j].Operations_resource_name + "\r\n" + i.ToolDetailList[j].Reserved_field01);
                    WorkSheet.GetRow(index).GetCell(12 + j).CellStyle = headerStyle_content5; //套用樣式
                }
                for(int j = 0; j < 4 - i.ToolDetailList.Count; j++)
                {
                    WorkSheet.GetRow(index).CreateCell(15 - j).SetCellValue("");
                    WorkSheet.GetRow(index).GetCell(15 - j).CellStyle = headerStyle_content5; //套用樣式
                }

                WorkSheet.GetRow(index).CreateCell(16).SetCellValue(i.Submit.Operator_id);
                WorkSheet.GetRow(index).GetCell(16).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(17).SetCellValue(i.Submit.Reserved_field01);
                WorkSheet.GetRow(index).GetCell(17).CellStyle = headerStyle_content5; //套用樣式

                WorkSheet.GetRow(index).CreateCell(18).SetCellValue(i.Submit.Reserved_field02);
                WorkSheet.GetRow(index).GetCell(18).CellStyle = headerStyle_content5; //套用樣式
            }

            WorkSheet.CreateRow(index + 1);
            WorkSheet.GetRow(index + 1).CreateCell(0).SetCellValue("備註");
            WorkSheet.GetRow(index + 1).GetCell(0).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.GetRow(index + 1).CreateCell(1);
            WorkSheet.GetRow(index + 1).GetCell(1).CellStyle = headerStyle_content2; //套用樣式
            WorkSheet.AddMergedRegion(new CellRangeAddress(index + 1, index + 1, 0, 1)); //合併1~1列及B~I欄儲存格

            for (int i = 2; i <= 18; i++)
            {
                WorkSheet.GetRow(index + 1).CreateCell(i);
                WorkSheet.GetRow(index + 1).GetCell(i).CellStyle = headerStyle_content2; //套用樣式
            }
            WorkSheet.AddMergedRegion(new CellRangeAddress(index + 1, index + 1, 2, 18)); //合併1~1列及B~I欄儲存格

            string FilePath = Filename;
            FileStream parFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            Workbook.Write(parFile);
            parFile.Close();
        }
    }
}
