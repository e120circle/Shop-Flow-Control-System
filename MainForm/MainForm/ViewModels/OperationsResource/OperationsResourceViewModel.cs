using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.OperationsResource
{
    public class OperationsResourceViewModel
    {
        [Display(Name = "Resource_id")]
        public int Resource_id { get; set; }

        [Key]
        [Display(Name = "Resource_no")]
        public string Resource_no { get; set; }

        [Display(Name = "Resource_name")]
        public string Resource_name { get; set; }

        [Display(Name = "Resource_description")]
        public string Resource_description { get; set; }

        [Display(Name = "Operations_resource_type")]
        public int Operations_resource_type { get; set; }

        [Display(Name = "Is_enable")]
        public bool Is_enable { get; set; }

        [Display(Name = "Create_by")]
        [MaxLength(45)]
        public string Create_by { get; set; }

        [Display(Name = "Create_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Create_date { get; set; }

        [Display(Name = "Last_update_by")]
        [MaxLength(45)]
        public string Last_update_by { get; set; }

        [Display(Name = "Last_update_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Last_update_date { get; set; }
    }
}
