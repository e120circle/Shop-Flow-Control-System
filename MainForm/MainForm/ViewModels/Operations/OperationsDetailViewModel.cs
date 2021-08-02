using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Operations
{
    public class OperationsDetailViewModel
    {
        [Display(Name = "Operations_resource_id")]
        public int Operations_resource_id { get; set; }

        [Display(Name = "Operations_submit_id")]
        public int Operations_submit_id { get; set; }

        [Display(Name = "Workstation_resource_id")]
        public int Workstation_resource_id { get; set; }

        [Display(Name = "Routing_resource_id")]
        public int Routing_resource_id { get; set; }

        [Display(Name = "Operations_resource_type")]
        public int Operations_resource_type { get; set; }

        [Key]
        [Display(Name = "Operations_resource_no")]
        [MaxLength(128)]
        public string Operations_resource_no { get; set; }

        [Display(Name = "Operations_resource_name")]
        [MaxLength(255)]
        public string Operations_resource_name { get; set; }

        [Display(Name = "Operations_resource_description")]
        public string Operations_resource_description { get; set; }

        [Display(Name = "Mapping_sys_user")]
        [MaxLength(255)]
        public string Mapping_sys_user { get; set; }

        [Display(Name = "Is_enable")]
        public bool Is_enable { get; set; }

        [Display(Name = "Create_by")]
        [MaxLength(255)]
        public string Create_by { get; set; }

        [Display(Name = "Create_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Create_date { get; set; }

        [Display(Name = "Last_update_by")]
        [MaxLength(255)]
        public string Last_update_by { get; set; }

        [Display(Name = "Last_update_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Last_update_date { get; set; }

        [Display(Name = "Reserved_field01")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "刀具用量不能為負數")]
        public string Reserved_field01 { get; set; }        //麥斯-加工刀具用量
    }
}
