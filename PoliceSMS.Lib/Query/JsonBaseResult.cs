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
    /// 返回的JSON格式
    /// </summary>
    public class JsonBaseResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public string Success { get; set; }
        /// <summary>
        /// json格式的对象
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }
}
