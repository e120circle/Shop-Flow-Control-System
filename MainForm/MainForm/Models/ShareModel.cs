using MainForm.Areas.Identity.Data;
using MainForm.Models.SysCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLClass.Models.AutoNumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Models
{
    public static class ShareModel
    {
        /// <summary>
        /// 取得SYS選單資料
        /// </summary>
        /// <param name="SysCommonModel"></param>
        /// <param name="key"></param>
        /// <param name="list"></param>
        public static List<SelectListItem> GetSelectList(SysCommonModel SysCommonModel, string key)
        {   //get select list
            List<SQLClass.Models.SysCommon.SelectList> list_temp;
            List<SelectListItem> temp = new List<SelectListItem>();

            SysCommonModel.GetSelectList(key, out list_temp);

            foreach (SQLClass.Models.SysCommon.SelectList tt in list_temp)
            {
                temp.Add(new SelectListItem()
                {
                    Text = tt.Select_list_name,
                    Value = tt.Select_list_value
                });
            }

            return temp;
        }

        /// <summary>
        /// 取得工單狀態選單資料
        /// </summary>
        /// <param name="SysCommonModel"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetJob_statusList(SysCommonModel SysCommonModel)
        {   //get status select list
            List<SQLClass.Models.SysCommon.SelectList> temp;

            SysCommonModel.GetSelectList("job_status", out temp);
            List<SelectListItem> StatusList = new List<SelectListItem>();

            foreach (SQLClass.Models.SysCommon.SelectList tt in temp)
            {
                StatusList.Add(new SelectListItem()
                {
                    Text = tt.Select_list_name,
                    Value = tt.Select_list_value
                });
            }

            return StatusList;
        }

        internal delegate string getLike(string format);    //for auto number get like number delegate

        internal static event getLike GetLike;              //for auto number get like umber event

        /// <summary>
        ///僅取得編碼最後流水號
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_AutoNumberContext"></param>
        /// <returns></returns>
        public static int GetAutoNumberInFormat(string key, IAutoNumber _AutoNumberContext)
        {
            SQLClass.Models.AutoNumber.AutoNumber temp;
            string format = "";
            int number = 0;
            string[] temp_array;
            List<string> format_array = new List<string>();

            _AutoNumberContext.GetAutoNumber(key, out temp);
            temp_array = temp.Auto_numbering_format.Split('[');

            for (int i = 0; i < temp_array.Length; i++)
            {
                if (temp_array[i].Split(']').Length > 1)
                    format_array.Add(temp_array[i].Split(']')[0]);
            }

            foreach (string a in format_array)
            {
                switch (a)
                {
                    case "YYYY":
                        format += DateTime.Today.Date.Year.ToString();
                        break;
                    case "YY":
                        format += (DateTime.Today.Date.Year - 2000).ToString();
                        break;
                    case "YYY":
                        format += (DateTime.Today.Date.Year - 1911).ToString();
                        break;
                    case "MM":
                        format += DateTime.Today.Date.Month.ToString("00");
                        break;
                    case "DD":
                        format += DateTime.Today.Date.Day.ToString("00");
                        break;
                    case "HH":
                        format += DateTime.Now.Hour.ToString("00");
                        break;
                    case "mm":
                        format += DateTime.Now.Minute.ToString("00");
                        break;
                    case "ss":
                        format += DateTime.Now.Second.ToString("00");
                        break;
                    default:
                        if (a.Contains("0"))
                        {
                            break;
                        }
                        else
                        {
                            format += a;
                        }
                        break;
                }
            }

            int reset_index = format_array.LastIndexOf(temp.Reset_cycle);
            int ex_digit = 0;
            for (int i = 0; i < reset_index; i++)
            {
                ex_digit += format_array[i].Length;
            }
            string exsis_no = GetLike(format.Substring(0, ex_digit + temp.Reset_cycle.Length));

            if (exsis_no == "")
            {
                number = 0;
                return number;
            }
            else
            {
                string reset_date_flag = "";

                switch (temp.Reset_cycle)
                {
                    case "YYYY":
                        reset_date_flag += DateTime.Today.Date.Year.ToString();
                        break;
                    case "YY":
                        reset_date_flag += (DateTime.Today.Date.Year - 2000).ToString();
                        break;
                    case "YYY":
                        reset_date_flag += (DateTime.Today.Date.Year - 1911).ToString();
                        break;
                    case "MM":
                        reset_date_flag += DateTime.Today.Date.Month.ToString("00");
                        break;
                    case "DD":
                        reset_date_flag += DateTime.Today.Date.Day.ToString("00");
                        break;
                    default:
                        reset_date_flag += DateTime.Today.Date.Day.ToString("00");
                        break;
                }

                if (exsis_no.Substring(ex_digit, temp.Reset_cycle.Length) != reset_date_flag)
                {
                    number = 0;
                }
                else
                {
                    number = Convert.ToInt16(exsis_no.Substring(ex_digit + temp.Reset_cycle.Length, format_array.Last().Length));
                }

                return number;
            }
        }

        /// <summary>
        /// 取得完整編碼格式(含流水號)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_AutoNumberContext"></param>
        /// <returns></returns>
        public static string GetAutoNumber(string key, IAutoNumber _AutoNumberContext)
        {
            SQLClass.Models.AutoNumber.AutoNumber temp;
            string format = "", number = "";
            string[] temp_array;
            List<string> format_array = new List<string>();

            _AutoNumberContext.GetAutoNumber(key, out temp);
            temp_array = temp.Auto_numbering_format.Split('[');

            for (int i = 0; i < temp_array.Length; i++)
            {
                if (temp_array[i].Split(']').Length > 1)
                    format_array.Add(temp_array[i].Split(']')[0]);
            }

            foreach (string a in format_array)
            {
                switch (a)
                {
                    case "YYYY":
                        format += DateTime.Today.Date.Year.ToString();
                        break;
                    case "YY":
                        format += (DateTime.Today.Date.Year - 2000).ToString();
                        break;
                    case "YYY":
                        format += (DateTime.Today.Date.Year - 1911).ToString();
                        break;
                    case "MM":
                        format += DateTime.Today.Date.Month.ToString("00");
                        break;
                    case "DD":
                        format += DateTime.Today.Date.Day.ToString("00");
                        break;
                    case "HH":
                        format += DateTime.Now.Hour.ToString("00");
                        break;
                    case "mm":
                        format += DateTime.Now.Minute.ToString("00");
                        break;
                    case "ss":
                        format += DateTime.Now.Second.ToString("00");
                        break;
                    default:
                        if (a.Contains("0"))
                        {
                            break;
                        }
                        else
                        {
                            format += a;
                        }
                        break;
                }
            }

            int reset_index = format_array.LastIndexOf(temp.Reset_cycle);
            int ex_digit = 0;
            for (int i = 0; i < reset_index; i++)
            {
                ex_digit += format_array[i].Length;
            }
            string exsis_no = GetLike(format.Substring(0, ex_digit + temp.Reset_cycle.Length));

            if (exsis_no == "")
            {
                number = 1.ToString(format_array.Last());
                return format + number;
            }
            else
            {
                string reset_date_flag = "";

                switch (temp.Reset_cycle)
                {
                    case "YYYY":
                        reset_date_flag += DateTime.Today.Date.Year.ToString();
                        break;
                    case "YY":
                        reset_date_flag += (DateTime.Today.Date.Year - 2000).ToString();
                        break;
                    case "YYY":
                        reset_date_flag += (DateTime.Today.Date.Year - 1911).ToString();
                        break;
                    case "MM":
                        reset_date_flag += DateTime.Today.Date.Month.ToString("00");
                        break;
                    case "DD":
                        reset_date_flag += DateTime.Today.Date.Day.ToString("00");
                        break;
                    default:
                        reset_date_flag += DateTime.Today.Date.Day.ToString("00");
                        break;
                }

                if (exsis_no.Substring(ex_digit, temp.Reset_cycle.Length) != reset_date_flag)
                {
                    number = 1.ToString(format_array.Last());
                }
                else
                {
                    number = (temp.Last_numeral + 1).ToString(format_array.Last());
                }

                return format + number;
            }
        }
    }
}
