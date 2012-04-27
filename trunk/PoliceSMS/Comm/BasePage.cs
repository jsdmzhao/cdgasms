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

namespace PoliceSMS.Comm
{
    public class BasePage : Page
    {
        public BasePage()
        {
        }

        /// <summary>
        /// 验证登陆用户是否有权限进入当前页面
        /// </summary>
        protected virtual bool PermissionVerify()
        {
            throw new NotImplementedException();
        }
    }
}
