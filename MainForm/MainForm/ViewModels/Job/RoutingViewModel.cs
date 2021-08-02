using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Job
{
    public class RoutingViewModel
    {        
        [Display(Name = "Routing_id")]        
        public string Routing_id { get; set; }

        [Display(Name = "Job_id")]
        public int Job_id { get; set; }

        [Display(Name = "Previous_routing_id")]
        public int Previous_routing_id { get; set; }

        [Key]
        [Display(Name = "Routing_no")]
        [MaxLength(128)]
        public string Routing_no { get; set; }

        [Display(Name = "Routing_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入工單途程名稱")]
        [MaxLength(255)]
        public string Routing_name { get; set; }

        [Display(Name = "Routing_description")]
        [MaxLength(2048)]
        public string Routing_description { get; set; }

        [Display(Name = "Routing_workstation_id")]
        public int Routing_workstation_id { get; set; }

        [Display(Name = "Routing_workstation_no")]
        [MaxLength(128)]
        public string Routing_workstation_no { get; set; }

        [Display(Name = "Routing_workstation_name")]
        [MaxLength(255)]
        public string Routing_workstation_name { get; set; }

        [Display(Name = "Routing_workstation_description")]
        [MaxLength(2048)]
        public string Routing_workstation_description { get; set; }

        [Display(Name = "Standard_setup_time")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "請輸入標準換線時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "標準換線時間不能為負數")]
        public int? Standard_setup_time { get; set; }

        [Display(Name = "Standard_unit_setup_time")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "請輸入標準單件準備時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "標準單件準備時間不能為負數")]
        public int? Standard_unit_setup_time { get; set; }

        [Display(Name = "Standard_unit_run_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入標準單件加工時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "標準單件加工時間不能為負數")]
        public int? Standard_unit_run_time { get; set; }

        [Display(Name = "Standard_unit_total_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入標準單件生產時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "標準單件生產時間不能為負數")]

        public int? Standard_unit_total_time { get; set; }

        [Display(Name = "Standard_unit_lead_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入標準工時(單件)")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "標準工時不能為負數")]
        public int? Standard_unit_lead_time { get; set; }

        [Display(Name = "Actually_setup_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際換線時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際換線時間不能為負數")]
        public int? Actually_setup_time { get; set; }

        [Display(Name = "Actually_unit_setup_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際單件準備時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際單件準備時間不能為負數")]
        public int? Actually_unit_setup_time { get; set; }

        [Display(Name = "Actually_unit_run_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際單件加工時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際單件加工時間不能為負數")]
        public int? Actually_unit_run_time { get; set; }

        [Display(Name = "Actually_unit_total_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際單件生產時間")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際單件生產時間不能為負數")]
        public int? Actually_unit_total_time { get; set; }

        [Display(Name = "Actually_unit_lead_time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際工時(單件)")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際工時不能為負數")]
        public int? Actually_unit_lead_time { get; set; }

        [Display(Name = "Scheduled_start_date")]
        [Remote(action: "Scheduled_start_dateValidate", controller: "Job")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Scheduled_start_date { get; set; }

        [Display(Name = "Scheduled_completion_date")]
        [Remote(action: "Scheduled_completion_dateValidate", controller: "Job")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Scheduled_completion_date { get; set; }

        [Display(Name = "Actually_start_date")]
        [Required(ErrorMessage = "請輸入實際開工日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Actually_start_date { get; set; }

        [Display(Name = "Actually_completion_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Actually_completion_date { get; set; }

        [Display(Name = "Scheduled_completion_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入預定完工數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "預定完工數量不能為負數")]
        public int Scheduled_completion_quantity { get; set; }

        [Display(Name = "Scheduled_completion_quantity_uom")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入預定完工數量單位")]
        [MaxLength(255)]
        public string Scheduled_completion_quantity_uom { get; set; }

        [Display(Name = "Actually_completion_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際完工數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際完工數量不能為負數")]
        public int Actually_completion_quantity { get; set; }

        [Display(Name = "Actually_completion_quantity_uom")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際完工數量單位")]
        [MaxLength(255)]
        public string Actually_completion_quantity_uom { get; set; }

        [Display(Name = "Actually_defective_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入不良品數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "不良品數量不能為負數")]
        public int Actually_defective_quantity { get; set; }

        [Display(Name = "Actually_defective_quantity_uom")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入不良品數量單位")]
        [MaxLength(255)]
        public string Actually_defective_quantity_uom { get; set; }

        [Display(Name = "Transfer_out_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入已移轉數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "已移轉數量不能為負數")]
        public int Transfer_out_quantity { get; set; }

        [Display(Name = "Transfer_out_quantity_uom")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入已移轉數量單位")]
        [MaxLength(255)]
        public string Transfer_out_quantity_uom { get; set; }

        [DisplayName("不良品數量強制分類")]
        public bool Is_classification_defective_item { get; set; }

        [DisplayName("允需分段報工")]
        public bool Is_allow_split_submit { get; set; }

        [DisplayName("報工時決定工序資源")]
        public bool Is_define_resource_in_opeations_submit { get; set; }

        [DisplayName("是否生效")]
        public bool Is_enable { get; set; }

        [DisplayName("新增人員：")]
        [MaxLength(255)]
        public string Create_by { get; set; }

        [DisplayName("新增日期：")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Create_date { get; set; }

        [DisplayName("最後更新人員：")]
        [MaxLength(255)]
        public string Last_update_by { get; set; }

        [DisplayName("最後更新日期：")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Last_update_date { get; set; }
    }
}
