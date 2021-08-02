using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Areas.Identity.Data
{
    public static class MainFormRoles
    {
        public const string PMC = "PMC";        
        public const string Manager = "管理人員";
        public const string PC = "生管人員";
        public const string OP_Manager = "現場主管";
        public const string OP = "現場人員";
    }
}
