using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Organize
{
    public class CompanyViewModel
    {
        [Key]
        [Display(Name = "Company_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入公司英文縮寫")]
        public int Company_id { get; set; }

        [Display(Name = "Company_no")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入公司代碼")]
        [MaxLength(128)]
        public string Company_no { get; set; }

        [Display(Name = "Company_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入公司全名")]
        [MaxLength(255)]
        public string Company_name { get; set; }

        [Display(Name = "Company_description")]
        [MaxLength(2048)]
        public string Company_description { get; set; }

        [Display(Name = "Company_tax_id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入統一編號")]
        [MaxLength(8)]
        public string Company_tax_id { get; set; }

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
