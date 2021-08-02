using SQLClass.Models.SysCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models.SysCommon
{
    public class SysCommonModel
    {
        public ISysCommonManage _SysCommonContext { get; set; }

        public SysCommonModel(ISysCommonManage SysCommonContext)
        {
            _SysCommonContext = SysCommonContext;
        }

        public List<SelectListModel> Job_source_typeList { get; set; }

        public void GetSelectList(string key, out List<SelectList> selectList)
        {
            List<SelectList> temp;

            _SysCommonContext.GetSelectList(key, out temp);
            selectList = temp;
        }
    }
}
