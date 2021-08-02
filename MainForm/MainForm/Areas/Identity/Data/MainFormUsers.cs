using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MainForm.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MainFormUsers class
    public class MainFormUsers : IdentityUser
    {
        [PersonalData]
        [Column (TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [PersonalData]
        public int Accout_role_type { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Groupe { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(255)")]
        public string ChinessName { get; set; }

        [PersonalData]
        [Column(TypeName = "bit(1)")]
        public int is_enable { get; set; }
    }
}
