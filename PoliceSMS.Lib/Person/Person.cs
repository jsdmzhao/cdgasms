using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoliceSMS.Lib.Core;
using PoliceSMS.Lib.Base;

namespace PoliceSMS.Lib.Person
{
    public class Person:Item
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual  string IdCard { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public virtual  string Mobile { get; set; }       
    }
}
