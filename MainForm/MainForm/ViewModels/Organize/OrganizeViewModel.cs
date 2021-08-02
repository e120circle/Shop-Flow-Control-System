using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Organize
{
    public class OrganizeViewModel
    {
        public CompanyViewModel CompanyViewModel { get; set; }

        public BusinessUnitViewModel BusinessUnitViewModel { get; set; }

        public FactoryViewModel FactoryViewModel { get; set; }

        public List<SQLClass.Models.Company.BusinessUnit> BusinessUnitList { get; set; }

        public string SelectBusinessID { get; set; }

        public List<SQLClass.Models.Company.Factory> FactoryList { get; set; }

        public List<SQLClass.Models.Company.Factory> FactoryListWithBusinessUnit { get; set; }

        public List<SelectListItem> LastUpdateUserList { get; set; }
    }
}
