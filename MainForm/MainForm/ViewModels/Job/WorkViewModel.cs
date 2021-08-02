using MainForm.Models;
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
    public class WorkViewModel
    {        
        [Display(Name = "Job_id")]
        public string Job_id { get; set; }

        [Display(Name = "Job_qrcode")]
        public Byte[] Job_qrcode { get; set; }

        [Display(Name = "Job_barcode")]
        [MaxLength(1024)]
        public string Job_barcode { get; set; }

        [Display(Name = "Factory_id")]
        public int Factory_id { get; set; }
                       
        [Display(Name = "Job_source_type")]
        public int Job_source_type { get; set; }
        public List<SelectListItem> JobSourceTypeList { get; set; }

        [Display(Name = "Job_source_id")]
        public int Job_source_id { get; set; }

        [Display(Name = "Job_source_no")]
        public string Job_source_no { get; set; }

        [Key]
        [Display(Name = "Job_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入工單編號")]
        [MaxLength(128)]
        public string Job_no { get; set; }

        [Display(Name = "Job_date")]
        [Required(ErrorMessage = "請輸入工單日期")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Job_date { get; set; }

        [Display(Name = "Job_released_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Job_released_date { get; set; }

        [Display(Name = "Job_description")]
        [MaxLength(2048)]
        [DataType(DataType.MultilineText)]
        public string Job_description { get; set; }

        [Display(Name = "Job_status")]
        //[EnumDataType(typeof(MainForm.Models.Job.JobModel.Status))]
        public int Job_status { get; set; }

        [Display(Name = "Customer_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶編號")]
        [MaxLength(128)]
        public string Customer_no { get; set; }

        [Display(Name = "Customer_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶名稱")]
        [MaxLength(255)]
        public string Customer_name { get; set; }

        [Display(Name = "Customer_order_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶訂單識別碼")]
        public int Customer_order_id { get; set; }

        [Display(Name = "Customer_order_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶訂單編號")]
        [MaxLength(128)]
        public string Customer_order_no { get; set; }
                
        [Display(Name = "Customer_shipping_date")]
        [Remote(action: "Customer_shipping_dateValidate", controller: "Job")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定訂單預訂出貨日")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Customer_shipping_date { get; set; }

        [Display(Name = "Customer_shipping_address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶訂單出貨地址")]
        [MaxLength(2048)]
        public string Customer_shipping_address { get; set; }

        [Display(Name = "Employee_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入製單人員")]
        [MaxLength(255)]
        public string Employee_id { get; set; }

        [Display(Name = "Item_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入完工料號")]
        public int Item_id { get; set; }

        [Display(Name = "Item_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定完工料號")]
        [MaxLength(128)]
        public string Item_no { get; set; }

        [Display(Name = "Item_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定完工料號")]
        [MaxLength(255)]
        public string Item_name { get; set; }

        [Display(Name = "Item_description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定完工料號")]
        [MaxLength(2048)]
        public string Item_description { get; set; }

        [Display(Name = "Item_type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定完工料號種類")]
        public int Item_type { get; set; }

        [Display(Name = "Customer_order_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入客戶訂單數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "訂單數量不能為負數")]
        public int Customer_order_quantity { get; set; }

        [Display(Name = "Customer_order_quantity_uom")]
        [MaxLength(255)]
        public string Customer_order_quantity_uom { get; set; }

        [Display(Name = "Scheduled_start_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Scheduled_start_date { get; set; }

        [Display(Name = "Scheduled_completion_date")]
        [Remote(action: "Scheduled_completion_dateValidate", controller: "Job")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請指定預定完工日期")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Scheduled_completion_date { get; set; }

        [Display(Name = "Scheduled_completion_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入預定完工數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "完工數量不能為負數")]
        public int Scheduled_completion_quantity { get; set; }

        [Display(Name = "Scheduled_completion_quantity_uom")]
        [MaxLength(255)]
        public string Scheduled_completion_quantity_uom { get; set; }

        [Display(Name = "Actually_start_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Actually_start_date { get; set; }

        [Display(Name = "Actually_completion_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Actually_completion_date { get; set; }

        [Display(Name = "Actually_completion_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入實際完工數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "完工數量不能為負數")]
        public int Actually_completion_quantity { get; set; }

        [Display(Name = "Actually_completion_quantity_uom")]
        [MaxLength(255)]
        public string Actually_completion_quantity_uom { get; set; }

        [Display(Name = "Actually_defective_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入不良品數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "不良品數量不能為負數")]
        public int Actually_defective_quantity { get; set; }

        [Display(Name = "Actually_defective_quantity_uom")]
        [MaxLength(255)]
        public string Actually_defective_quantity_uom { get; set; }

        [Display(Name = "Input_of_inventory_quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入入庫數量")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "入庫數量不能為負數")]
        public int Input_of_inventory_quantity { get; set; }

        [Display(Name = "Input_of_inventory_quantity_uom")]
        [MaxLength(255)]
        public string Input_of_inventory_quantity_uom { get; set; }

        [Display(Name = "Job_closed_date")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Job_closed_date { get; set; }

        [DisplayName("不良品數量強制分類")]
        public bool Is_classification_defective_item { get; set; }

        [DisplayName("允需分段報工")]
        public bool Is_allow_split_submit { get; set; }

        [DisplayName("報工時決定工序資源")]
        public bool Is_define_desource_in_operations_submit { get; set; }

        [DisplayName("是否生效")]
        public bool Is_enable { get; set; }

        [DisplayName("新增人員：")]
        [MaxLength(255)]
        public string Create_by { get; set; }

        [DisplayName("新增日期：")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Create_date { get; set; }

        [DisplayName("最後更新人員：")]
        [MaxLength(255)]
        public string Last_update_by { get; set; }

        [DisplayName("最後更新日期：")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> Last_update_date { get; set; }

        [Display(Name = "Reserved_field01")]
        [Required(ErrorMessage = "請輸入採購單號")]
        [MaxLength(128)]
        public string Reserved_field01 { get; set; }        //麥斯-採購單號

        [Display(Name = "Reserved_field02")]
        [MaxLength(128)]
        public string Reserved_field02 { get; set; }        //麥斯-打字
    }
}
