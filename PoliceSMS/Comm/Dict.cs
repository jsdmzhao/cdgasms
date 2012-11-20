using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace PoliceSMS.Comm
{
    public class Dict
    {
        private static readonly Dictionary<string, string> dict = new Dictionary<string, string>();
        static Dict()
        {
            dict.Add("SMSRecordList", "群众办事登记");
            dict.Add("SMSRecordListNew", "群众办事登记");
            dict.Add("WorkTypeList", "办事内容管理");
            dict.Add("GradeTypeList", "评分内容管理");
            dict.Add("NoticeList", "系统通告");
            dict.Add("SupervisionList", "督查情况管理");
            dict.Add("Index", "首页");
        }

        public static string GetStr(string key)
        {
            if (dict.ContainsKey(key))
                return dict[key];
            return string.Empty;
        }
    }
}
