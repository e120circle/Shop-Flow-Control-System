using MainForm.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Roles
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<MainFormUsers> Members { get; set; }
        public IEnumerable<MainFormUsers> NonMembers { get; set; }
    }
}
