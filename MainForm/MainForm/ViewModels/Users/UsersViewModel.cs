using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.ViewModels.Users
{
    public class UsersViewModel
    {
        public UserViewModel UserViewModel { get; set; }

        public List<SQLClass.Models.Users.Users> UsersList { get; set; }

        public PagingList<SQLClass.Models.Users.Users> UsersListInPaging { get; set; }

        public string Sort_str { get; set; }

        public List<SelectListItem> AccountRoleTypeList { get; set; }

        public List<SelectListItem> GroupeList { get; set; }
    }
}
