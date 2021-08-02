using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Users
{
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "UserName")]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(255)]
        public string Email { get; set; }

        [Display(Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Display(Name = "Name")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Display(Name = "ChinessName")]
        [MaxLength(255)]
        public string ChinessName { get; set; }

        [Display(Name = "Accout_role_type")]
        public int Accout_role_type { get; set; }

        [Display(Name = "Groupe")]
        [MaxLength(255)]
        public string Groupe { get; set; }

        [DisplayName("是否生效")]
        public bool Is_enable { get; set; }
    }
}
