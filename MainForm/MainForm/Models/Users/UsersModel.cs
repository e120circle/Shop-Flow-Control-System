using MainForm.Areas.Identity.Data;
using MainForm.Models.SysCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.Users
{
    public class UsersModel : SQLClass.Models.Users.Users
    {
        public IUsersManage _UsersContext { get; set; }

        public UserModel UserViewModel { get; set; }

        public SysCommonModel SysCommonModel { get; set; }

        public UsersModel(IUsersManage UsersContext, ISysCommonManage SysCommonContext)
        {
            _UsersContext ??= UsersContext;
            SysCommonModel ??= new SysCommonModel(SysCommonContext);

            UserViewModel ??= new UserModel();
        }

        public List<SQLClass.Models.Users.Users> UsersList { get; set; }

        public List<SelectListItem> AccountRoleTypeList { get; set; }

        public List<SelectListItem> GroupeList { get; set; }

        public bool GetAllUsers()
        {
            bool flag = false;
            List<SQLClass.Models.Users.Users> temp;

            flag = _UsersContext.GetAllUsers(out temp);
            UsersList = temp;

            return flag;
        }

        public void GetEditUsers(string name)
        {
            SQLClass.Models.Users.Users temp;
            _UsersContext.GetUsersByName(name, out temp);

            UserViewModel.Id = temp.Id;
            UserViewModel.UserName = temp.UserName;
            UserViewModel.Email = temp.Email;
            UserViewModel.PasswordHash = temp.PasswordHash;
            UserViewModel.Name = temp.Name;
            UserViewModel.ChinessName = temp.ChinessName;
            UserViewModel.Accout_role_type = temp.Accout_role_type;
            UserViewModel.Groupe = temp.Groupe;
            UserViewModel.Is_enable = temp.Is_enable;

            AccountRoleTypeList = ShareModel.GetSelectList(SysCommonModel, "accout_role_type");
            GroupeList = ShareModel.GetSelectList(SysCommonModel, "operations_groupe");            
        }

        public bool PostEditUsers(string name, IFormCollection data)
        {
            bool flag = false;

            SQLClass.Models.Users.Users users = new SQLClass.Models.Users.Users()
            {
                Id = data["UserViewModel.Id"],
                UserName = data["UserViewModel.Email"],
                Email = data["UserViewModel.Email"],
                PasswordHash = data["UserViewModel.PasswordHash"],
                Name = data["UserViewModel.Name"],
                ChinessName = data["UserViewModel.ChinessName"],
                Accout_role_type = Convert.ToInt32(data["UserViewModel.Accout_role_type"][0]),
                Groupe = data["UserViewModel.Groupe"]
                //Is_enable = data["UserViewModel.Is_enable"].Contains("true")                
            };

            flag = _UsersContext.EditUsers(name, users);

            return flag;
        }
    }
}
