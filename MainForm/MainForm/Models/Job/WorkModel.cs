using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Job
{
    public class WorkModel : SQLClass.Models.Job.Job
    {
        public List<SelectListItem> JobSourceTypeList { get; set; }
    }
}
