using MainForm.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLClass.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Organize
{
    public class OrganizeModel
    {
        public ICompanyManage _CompanyContext { get; set; }

        private UserManager<MainFormUsers> _userManager { get; set; }

        public CompanyModel CompanyViewModel { get; set; }

        public BusinessUnitModel BusinessUnitViewModel { get; set; }

        public FactoryModel FactoryViewModel { get; set; }

        public OrganizeModel(ICompanyManage CompanyContext, UserManager<MainFormUsers> userManager)
        {

            _CompanyContext = CompanyContext;
            _userManager = userManager;

            CompanyViewModel = new CompanyModel();
            BusinessUnitViewModel = new BusinessUnitModel();
            FactoryViewModel = new FactoryModel();
        }

        public List<SQLClass.Models.Company.BusinessUnit> BusinessUnitList { get; set; }

        public string SelectBusinessID { get; set; }

        public List<SQLClass.Models.Company.Factory> FactoryList { get; set; }

        public List<SQLClass.Models.Company.Factory> FactoryListWithBusinessUnit { get; set; }

        public List<SelectListItem> LastUpdateUserList { get; set; }

        public void GetCompany()
        {
            SQLClass.Models.Company.Company Company_temp = new SQLClass.Models.Company.Company();
            _CompanyContext.GetCompany(out Company_temp);
            CompanyViewModel.Company_id = Company_temp.Company_id;
            CompanyViewModel.Company_no = Company_temp.Company_no;
            CompanyViewModel.Company_name = Company_temp.Company_name;
            CompanyViewModel.Company_description = Company_temp.Company_description;
            CompanyViewModel.Company_tax_id = Company_temp.Company_tax_id;
            CompanyViewModel.Create_by = Company_temp.Create_by;
            CompanyViewModel.Create_date = Company_temp.Create_date;
            CompanyViewModel.Last_update_by = Company_temp.Last_update_by;
            CompanyViewModel.Last_update_date = Company_temp.Last_update_date;
        }

        public void GetAllBusinessUnit()
        {
            BusinessUnitList = new List<BusinessUnit>();
            List<SQLClass.Models.Company.BusinessUnit> temp = new List<SQLClass.Models.Company.BusinessUnit>();
             _CompanyContext.GetAllBusinessUnit(out temp);

            foreach (var tt in temp)
            {
                if (tt.Is_enable) BusinessUnitList.Add(tt);
            }
        }

        public void GetFactoryList()
        {
            List<Factory> temp = new List<Factory>();
            _CompanyContext.GetAllFactory(out temp);
            FactoryList = new List<Factory>();
            foreach (var tt in temp)
            {
                if (tt.Is_enable) FactoryList.Add(tt);
            }

            FactoryListWithBusinessUnit = FactoryList.Where(x => x.Business_unit_id == SelectBusinessID).ToList();
        }

        public void GetCreateCompany(MainFormUsers user)
        {
            CompanyViewModel.Company_no = "";
            CompanyViewModel.Company_name = "";
            CompanyViewModel.Company_description = "";
            CompanyViewModel.Company_tax_id = "";
            CompanyViewModel.Create_by = user.Name;
            CompanyViewModel.Create_date = DateTime.Today.Date;
            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == user.Name).FirstOrDefault().Selected = true;

            CompanyViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostCreateCompany(IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Company.Company company = new SQLClass.Models.Company.Company()
            {
                Company_no = data["CompanyViewModel.Company_id"],
                Company_name = data["CompanyViewModel.Company_name"],
                Company_description = data["CompanyViewModel.Company_description"],
                Company_tax_id = data["CompanyViewModel.Company_tax_id"],
                Create_by = data["CompanyViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["CompanyViewModel.Create_date"]),
                Last_update_by = data["CompanyViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["CompanyViewModel.Last_update_date"])
            };

            flag = _CompanyContext.CreateCompany(company);

            return flag;
        }

        public void GetEditCompany(MainFormUsers user)
        {
            SQLClass.Models.Company.Company temp = new SQLClass.Models.Company.Company();
            _CompanyContext.GetCompany(out temp);

            CompanyViewModel.Company_id = temp.Company_id;
            CompanyViewModel.Company_no = temp.Company_no;
            CompanyViewModel.Company_name = temp.Company_name;
            CompanyViewModel.Company_description = temp.Company_description;
            CompanyViewModel.Company_tax_id = temp.Company_tax_id;
            CompanyViewModel.Create_by = temp.Create_by;
            CompanyViewModel.Create_date = temp.Create_date;

            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == temp.Last_update_by).FirstOrDefault().Selected = true;

            CompanyViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostEditCompany(string id, IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Company.Company company = new SQLClass.Models.Company.Company()
            {
                Company_no = data["CompanyViewModel.Company_id"],
                Company_name = data["CompanyViewModel.Company_name"],
                Company_description = data["CompanyViewModel.Company_description"],
                Company_tax_id = data["CompanyViewModel.Company_tax_id"],
                Create_by = data["CompanyViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["CompanyViewModel.Create_date"]),
                Last_update_by = data["CompanyViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["CompanyViewModel.Last_update_date"])
            };

            flag = _CompanyContext.EditCompany(id, company);

            return flag;
        }

        public void GetCreateBusinessUnit(MainFormUsers user, string company_id)
        {
            BusinessUnitViewModel.Company_id = company_id;
            BusinessUnitViewModel.Business_unit_no = "";
            BusinessUnitViewModel.Business_unit_name = "";
            BusinessUnitViewModel.Business_unit_description = "";
            BusinessUnitViewModel.Is_enable = true;
            BusinessUnitViewModel.Create_by = user.Name;
            BusinessUnitViewModel.Create_date = DateTime.Today.Date;

            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == user.Name).FirstOrDefault().Selected = true;

            BusinessUnitViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostCreateBusinessUnit(IFormCollection data)
        {
            bool flag = false;
            
            SQLClass.Models.Company.BusinessUnit businessUnit = new SQLClass.Models.Company.BusinessUnit()
            {
                Company_id = data["BusinessUnitViewModel.Company_id"],
                Business_unit_no = data["BusinessUnitViewModel.Business_unit_no"],
                Business_unit_name = data["BusinessUnitViewModel.Business_unit_name"],
                Business_unit_description = data["BusinessUnitViewModel.Business_unit_description"],
                Is_enable = data["BusinessUnitViewModel.Is_enable"].Contains("true"),
                Create_by = data["BusinessUnitViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["BusinessUnitViewModel.Create_date"]),
                Last_update_by = data["BusinessUnitViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["BusinessUnitViewModel.Last_update_date"])
            };

            flag = _CompanyContext.CreateBusinessUnit(businessUnit);

            return flag;
        }

        public void GetEditBusinessUnit(string id, MainFormUsers user)
        {
            SQLClass.Models.Company.BusinessUnit temp = new SQLClass.Models.Company.BusinessUnit();
            _CompanyContext.GetBusinessUnit(id, out temp);

            BusinessUnitViewModel.Business_unit_id = temp.Business_unit_id;
            BusinessUnitViewModel.Company_id = temp.Company_id;
            BusinessUnitViewModel.Business_unit_no = temp.Business_unit_no;
            BusinessUnitViewModel.Business_unit_name = temp.Business_unit_name;
            BusinessUnitViewModel.Business_unit_description = temp.Business_unit_description;
            BusinessUnitViewModel.Is_enable = temp.Is_enable;
            BusinessUnitViewModel.Create_by = user.Name;
            BusinessUnitViewModel.Create_date = temp.Create_date;

            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == temp.Last_update_by).FirstOrDefault().Selected = true;

            BusinessUnitViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostEditBusinessUnit(string id, IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Company.BusinessUnit businessUnit = new SQLClass.Models.Company.BusinessUnit()
            {
                Company_id = data["BusinessUnitViewModel.Company_id"],
                Business_unit_no = data["BusinessUnitViewModel.Business_unit_no"],
                Business_unit_name = data["BusinessUnitViewModel.Business_unit_name"],
                Business_unit_description = data["BusinessUnitViewModel.Business_unit_description"],
                Is_enable = data["BusinessUnitViewModel.Is_enable"].Contains("true"),
                Create_by = data["BusinessUnitViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["BusinessUnitViewModel.Create_date"]),
                Last_update_by = data["BusinessUnitViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["BusinessUnitViewModel.Last_update_date"])
            };

            flag = _CompanyContext.EditBusinessUnit(id, businessUnit);

            return flag;
        }

        public bool DisableBusinessUnit(string id)
        {
            bool flag = false;
            flag = _CompanyContext.DisableBusinessUnit(id);

            return flag;
        }

        public void GetCreateFactory(MainFormUsers user, string businessUnit_id)
        {
            FactoryViewModel.Business_unit_id = businessUnit_id;
            FactoryViewModel.Factory_no = "";
            FactoryViewModel.Factory_name = "";
            FactoryViewModel.Factory_description = "";
            FactoryViewModel.Is_enable = true;
            FactoryViewModel.Create_by = user.Name;
            FactoryViewModel.Create_date = DateTime.Today.Date;

            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == user.Name).FirstOrDefault().Selected = true;

            FactoryViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostCreateFactory(IFormCollection data)
        {
            bool flag = false;
            
            SQLClass.Models.Company.Factory factory = new SQLClass.Models.Company.Factory()
            {
                Business_unit_id = data["FactoryViewModel.Business_unit_id"],
                Factory_no = data["FactoryViewModel.Factory_no"],
                Factory_name = data["FactoryViewModel.Factory_name"],
                Factory_description = data["FactoryViewModel.Factory_description"],
                Is_enable = data["FactoryViewModel.Is_enable"].Contains("true"),
                Create_by = data["FactoryViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["FactoryViewModel.Create_date"]),
                Last_update_by = data["FactoryViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["FactoryViewModel.Last_update_date"])
            };

            flag = _CompanyContext.CreateFactory(factory);

            return flag;
        }

        public void GetEditFactory(string id, MainFormUsers user)
        {
            SQLClass.Models.Company.Factory temp = new SQLClass.Models.Company.Factory();
            _CompanyContext.GetFactory(id, out temp);

            FactoryViewModel.Factory_id = temp.Factory_id;
            FactoryViewModel.Business_unit_id = temp.Business_unit_id;
            FactoryViewModel.Factory_no = temp.Factory_no;
            FactoryViewModel.Factory_name = temp.Factory_name;
            FactoryViewModel.Factory_description = temp.Factory_description;
            FactoryViewModel.Is_enable = temp.Is_enable;
            FactoryViewModel.Create_by = user.Name;
            FactoryViewModel.Create_date = temp.Create_date;

            LastUpdateUserList = new List<SelectListItem>();
            foreach (MainFormUsers a in _userManager.Users)
            {
                LastUpdateUserList.Add(new SelectListItem() { Value = a.Name, Text = a.Name, Selected = false });
            }
            LastUpdateUserList.Where(x => x.Value == temp.Last_update_by).FirstOrDefault().Selected = true;

            FactoryViewModel.Last_update_date = DateTime.Today.Date;
        }

        public bool PostEditFactory(string id, IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Company.Factory routing = new SQLClass.Models.Company.Factory()
            {
                Business_unit_id = data["FactoryViewModel.Business_unit_id"],
                Factory_no = data["FactoryViewModel.Factory_no"],
                Factory_name = data["FactoryViewModel.Factory_name"],
                Factory_description = data["FactoryViewModel.Factory_description"],
                Is_enable = data["FactoryViewModel.Is_enable"].Contains("true"),
                Create_by = data["FactoryViewModel.Create_by"],
                Create_date = Convert.ToDateTime(data["FactoryViewModel.Create_date"]),
                Last_update_by = data["FactoryViewModel.Last_update_by"],
                Last_update_date = Convert.ToDateTime(data["FactoryViewModel.Last_update_date"])
            };

            flag = _CompanyContext.EditFactory(id, routing);

            return flag;
        }

        public bool DisableFactory(string id)
        {
            bool flag = false;
            flag = _CompanyContext.DisableFactory(id);

            return flag;
        }
    }
}
