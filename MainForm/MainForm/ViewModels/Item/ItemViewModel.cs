using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Item
{
    public class ItemViewModel
    {
        [Display(Name = "Item_id")]
        public int Item_id { get; set; }

        [Key]
        [Display(Name = "Item_no")]
        [MaxLength(128)]
        public string Item_no { get; set; }

        [Display(Name = "Item_name")]
        [MaxLength(255)]
        public string Item_name { get; set; }

        [Display(Name = "Item_description")]
        [MaxLength(2048)]
        public string Item_description { get; set; }

        [Display(Name = "Item_eng_name")]
        [MaxLength(255)]
        public string Item_eng_name { get; set; }

        [Display(Name = "Item_eng_description")]
        [MaxLength(2048)]
        public string Item_eng_description { get; set; }

        [Display(Name = "Item_type")]
        public int Item_type { get; set; }

        [Display(Name = "Minimum_unit_of_measure")]
        [MaxLength(128)]
        public string Minimum_unit_of_measure { get; set; }

        [Display(Name = "Is_inv_enable")]
        public bool Is_inv_enable { get; set; }

        [Display(Name = "On_hand_quantity")]
        public int? On_hand_quantity { get; set; }

        [Display(Name = "Scheduled_receipts_quantity")]
        public int? Scheduled_receipts_quantity { get; set; }

        [Display(Name = "Allocated_quantity")]
        public int? Allocated_quantity { get; set; }

        [Display(Name = "Available_quantity")]
        public int? Available_quantity { get; set; }

        [Display(Name = "Safety_stock_quantity")]
        public int? Safety_stock_quantity { get; set; }

        [Display(Name = "Minimum_order_quantity")]
        public int? Minimum_order_quantity { get; set; }

        [Display(Name = "Economic_order_quantity")]
        public int? Economic_order_quantity { get; set; }

        [Display(Name = "Is_manufacture_enable")]
        public bool Is_manufacture_enable { get; set; }

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

        [Display(Name = "Is_classification_defective_item")]
        public bool Is_classification_defective_item { get; set; }

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
