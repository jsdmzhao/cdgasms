using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PoliceSMS.Comm;
using System.Windows.Browser;

namespace PoliceSMS.Views
{
    public partial class LoginForm : UserControl
    {
        private bool isSuccess;
        //private DunLibrary.User.User user;

        public LoginForm()
        {
            InitializeComponent();
            txtName.UpdateLayout();
            txtName.Focus();
            Loaded += new RoutedEventHandler(LoginForm_Loaded);
            
        }

        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
        }
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public PoliceSMS.Lib.Organization.Officer LoginUser { get; set; }
      
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action CallBack { get; set; }

        void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            login();

        }

        private void login()
        {
            //检查用户名和口令
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                Tools.ShowMessage("请录入用户名!", string.Empty, false);
                return;
            }

            if (string.IsNullOrEmpty(txtPass.Password.Trim()))
            {
                Tools.ShowMessage("请录入口令!", "本系统不允许空口令!", false);
                return;
            }

            AppGlobal.CurrentUser = new Lib.Organization.Officer { Name = "测试用户" };
            CallBack();
        }
        /// <summary>
        /// 退出登录界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ScriptObject Show = HtmlPage.Window.GetProperty("closeWin") as ScriptObject;
            Show.InvokeSelf();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login();
            }
        }

    }
}
