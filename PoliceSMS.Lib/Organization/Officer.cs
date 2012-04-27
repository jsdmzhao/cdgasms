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
using PoliceSMS.Lib.Core;

namespace PoliceSMS.Lib.Organization
{
    /// <summary>
    /// 警察
    /// </summary>
    public class Officer:Item
    {
        /// <summary>
        /// 警号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// 所属机构
        /// 派出所
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Order { get; set; }
    }
}
