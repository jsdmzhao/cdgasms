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

namespace PoliceSMS.Lib.Query
{
    /// <summary>
    /// 返回sl端分页的数据
    /// </summary>
    public class JsonListResult:JsonBaseResult
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }
    }
}
