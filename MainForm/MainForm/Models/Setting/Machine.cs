using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Setting
{
    public class Machine
    {
        public string No { get; set; }
        public string GR1 { get; set; }
        public string GR2 { get; set; }
        public string GR3 { get; set; }
        public string GR4 { get; set; }
        public string GR5 { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public string C_By { get; set; }
        public string C_Date { get; set; }
        public string U_By { get; set; }
        public string U_Date { get; set; }
    }
}
