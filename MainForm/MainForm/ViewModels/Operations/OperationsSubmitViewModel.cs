using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Operations
{
    public class OperationsSubmitViewModel
    {
        [Display(Name = "Operations_submit_id")]
        public int Operations_submit_id { get; set; }

        [Display(Name = "Routing_id")]
        public int Routing_id { get; set; }

        [Display(Name = "Item_id")]
        public int Item_id { get; set; }

        [Display(Name = "Operator_groupe_id")]
        [Required(ErrorMessage = "請選擇作業人員組別")]
        [MaxLength(255)]
        public string Operator_groupe_id { get; set; }

        [Display(Name = "Operator_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請選擇作業人員")]
        [MaxLength(255)]
        public string Operator_id { get; set; }

        [Key]
        [Display(Name = "Operations_submit_no")]
        [MaxLength(128)]
        public string Operations_submit_no { get; set; }

        [Display(Name = "Operations_submit_type")]
        public int Operations_submit_type { get; set; }

        [Display(Name = "Operations_submit_description")]
        [MaxLength(2048)]
        public string Operations_submit_description { get; set; }

        [Display(Name = "Standard_setup_time")]
        public int? Standard_setup_time { get; set; }

        [Display(Name = "Standard_unit_setup_time")]
        public int? Standard_unit_setup_time { get; set; }

        [Display(Name = "Standard_unit_run_time")]
        public int? Standard_unit_run_time { get; set; }

        [Display(Name = "Standard_unit_total_time")]
        public int? Standard_unit_total_time { get; set; }

        [Display(Name = "Standard_unit_lead_time")]
        public int? Standard_unit_lead_time { get; set; }

        [Display(Name = "Actually_setup_time")]
        public int? Actually_setup_time { get; set; }

        [Display(Name = "Actually_unit_setup_time")]
        public int? Actually_unit_setup_time { get; set; }

        [Display(Name = "Actually_unit_run_time")]
        public int? Actually_unit_run_time { get; set; }

        [Display(Name = "Actually_unit_total_time")]
        public int? Actually_unit_total_time { get; set; }

        [Display(Name = "Actually_unit_lead_time")]
        public int? Actually_unit_lead_time { get; set; }

        [Display(Name = "Scheduled_start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Scheduled_start_date { get; set; }

        [Display(Name = "Scheduled_completion_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Scheduled_completion_date { get; set; }

        [Display(Name = "Actually_start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? Actually_start_date { get; set; }

        [Display(Name = "Actually_completion_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? Actually_completion_date { get; set; }

        [Display(Name = "Scheduled_completion_quantity")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "開工數量不能為負數")]
        public int Scheduled_completion_quantity { get; set; }

        [Display(Name = "Scheduled_completion_quantity_uom")]
        [MaxLength(255)]
        public string Scheduled_completion_quantity_uom { get; set; }

        [Display(Name = "Scheduled_target_quantity")]
        public int Scheduled_target_quantity { get; set; }

        [Display(Name = "Scheduled_target_quantity_uom")]
        [MaxLength(255)]
        public string Scheduled_target_quantity_uom { get; set; }

        [Display(Name = "Actually_completion_quantity")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "實際完工數量不能為負數")]
        public int Actually_completion_quantity { get; set; }

        [Display(Name = "Actually_completion_quantity_uom")]
        [MaxLength(255)]
        public string Actually_completion_quantity_uom { get; set; }

        [Display(Name = "Actually_defective_quantity")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "不良品數量不能為負數")]
        public int Actually_defective_quantity { get; set; }

        [Display(Name = "Actually_defective_quantity_uom")]
        [MaxLength(255)]
        public string Actually_defective_quantity_uom { get; set; }

        [Display(Name = "Transfer_out_quantity")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "轉移數量不能為負數")]
        public int Transfer_out_quantity { get; set; }

        [Display(Name = "Transfer_out_quantity_uom")]
        [MaxLength(255)]
        public string Transfer_out_quantity_uom { get; set; }

        [Display(Name = "Is_classification_defective_item")]
        public bool Is_classification_defective_item { get; set; }

        [Display(Name = "Is_allow_split_submit")]
        public bool Is_allow_split_submit { get; set; }

        [Display(Name = "Is_define_resource_in_operations_submit")]
        public bool Is_define_resource_in_operations_submit { get; set; }        

        [Display(Name = "Is_enable")]
        public bool Is_enable { get; set; }

        [Display(Name = "Create_by")]
        [MaxLength(255)]
        public string Create_by { get; set; }

        [Display(Name = "Create_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Create_date { get; set; }

        [Display(Name = "Last_update_by")]
        [MaxLength(255)]
        public string Last_update_by { get; set; }

        [Display(Name = "Last_update_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Last_update_date { get; set; }

        [Display(Name = "Reserved_field01")]
        [MaxLength(128)]
        [RegularExpression(@"^\+?[0-9]*\.?[0-9]*$", ErrorMessage = "請輸入正確數字")]
        public string Reserved_field01 { get; set; }        //麥斯-加工前重量

        [Display(Name = "Reserved_field02")]
        [MaxLength(128)]
        [RegularExpression(@"^\+?[0-9]*\.?[0-9]*$", ErrorMessage = "請輸入正確數字")]
        public string Reserved_field02 { get; set; }        //麥斯-加工後重量
    }
}
