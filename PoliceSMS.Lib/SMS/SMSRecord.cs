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
using PoliceSMS.Lib.Organization;

namespace PoliceSMS.Lib.SMS
{
    /// <summary>
    /// 短信记录
    /// </summary>
    public class SMSRecord
    {
        public virtual int Id { get; set; }

        public virtual string PersonName { get; set; }

        public virtual Sex PersonSex { get; set; }

        public virtual string PersonIdCard { get; set; }

        public virtual string PersonMobile { get; set; }

        public virtual string Address { get; set; }

        public virtual DateTime? WorkDate { get; set; }
        /// <summary>
        /// 工作类别
        /// </summary>
        public virtual WorkType WorkType { get; set; }
        /// <summary>
        /// 办事内容
        /// </summary>
        public virtual string WorkContent { get; set; }
        /// <summary>
        /// 机构
        /// </summary>
        public virtual Organization.Organization Organization { get; set; }
        /// <summary>
        /// 记录警员
        /// </summary>
        public virtual Officer LoginOfficer { get; set; }
        /// <summary>
        /// 办事警员
        /// </summary>
        public virtual Officer WorkOfficer { get; set; }

        public virtual OfficerType OfficerType { get; set; }
        /// <summary>
        /// 评分类别
        /// </summary>
        public virtual GradeType GradeType { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public virtual string Score { get; set; }
        /// <summary>
        /// 是否发送
        /// </summary>
        public virtual bool IsSend { get; set; }
        /// <summary>
        /// 是否反馈信息
        /// </summary>
        public virtual bool IsResponse { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public virtual string ReceiveContent { get; set; }
        /// <summary>
        /// 记录号
        /// </summary>
        public virtual string WorkNo { get; set; }

    }
}
