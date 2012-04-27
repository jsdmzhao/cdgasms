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
    /// 查询条件
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 查询的hql语言
        /// </summary>
        public string HQL { get; set; }
        /// <summary>
        /// 获取总数的hql
        /// </summary>
        public string TotalHQL { get; set; }
        /// <summary>
        /// 开始查询的记录数
        /// </summary>
        public int FirstResult { get; set; }
        /// <summary>
        /// 需要返回的最大记录数
        /// </summary>
        public int MaxResults { get; set; }

    }
}
