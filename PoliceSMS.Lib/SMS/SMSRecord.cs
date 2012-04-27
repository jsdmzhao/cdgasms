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
using PoliceSMS.Lib.Base;

namespace PoliceSMS.Lib.SMS
{
    /// <summary>
    /// 短信记录
    /// </summary>
    public class SMSRecord
    {
        public int Id { get; set; }

        public string PersonName { get; set; }

        public Sex PersonSex { get; set; }

        public string PersonIdCard { get; set; }

        public string PersonMobile { get; set; }

        public string Address { get; set; }

        public DateTime WorkDate { get; set; }
        /// <summary>
        /// 工作类别
        /// </summary>
        public WorkType WorkType { get; set; }
        /// <summary>
        /// 办事内容
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 机构
        /// </summary>
        public Organization.Organization Organization { get; set; }
        /// <summary>
        /// 办事警员
        /// </summary>
        public Organization.Officer LoginOfficer { get; set; }
        /// <summary>
        /// 评分类别
        /// </summary>
        public GradeType GradeType { get; set; }
        /// <summary>
        /// 是否发送
        /// </summary>
        public bool IsSend { get; set; }
        /// <summary>
        /// 是否反馈信息
        /// </summary>
        public bool IsResponse { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReceiveContent { get; set; }
        /// <summary>
        /// 记录号
        /// </summary>
        public string WorkNo { get; set; }

    }
}
