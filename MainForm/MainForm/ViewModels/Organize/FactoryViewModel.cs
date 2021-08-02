using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Organize
{
    public class FactoryViewModel
    {
        [Key]
        [Display(Name = "Factory_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入工廠英文縮寫")]
        public int Factory_id { get; set; }

        [Display(Name = "Business_unit_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入公司代碼")]
        public int Business_unit_id { get; set; }

        [Display(Name = "Factory_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入工廠代碼")]
        [MaxLength(128)]
        public string Factory_no { get; set; }

        [Display(Name = "Factory_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入工廠全名")]
        [MaxLength(255)]
        public string Factory_name { get; set; }

        [Display(Name = "Factory_description")]
        [MaxLength(2048)]
        public string Factory_description { get; set; }

        [Display(Name = "Is_enable")]
        public bool Is_enable { get; set; }

        [Display(Name = "Create_by")]
        [MaxLength(255)]
        public string Create_by { get; set; }

        [Display(Name = "Create_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Create_date { get; set; }

        [Display(Name = "Last_update_by")]
        [MaxLength(255)]
        public string Last_update_by { get; set; }

        [Display(Name = "Last_update_date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Last_update_date { get; set; }
    }
}
