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
using PoliceSMS.Lib.Organization;
using PoliceSMS.Comm;
using System.ServiceModel;
using System.Windows.Browser;

namespace PoliceSMS
{
    public partial class LoginForm : UserControl
    {
        private bool isSuccess;

        public LoginForm()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(LoginForm_Loaded);

            
        }

        void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();

            txtName.Focus(); 
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
        public Officer LoginUser { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        public Action CallBack { get; set; }

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

            

            OfficerService.OfficerServiceClient ser1 = new OfficerService.OfficerServiceClient();
            //登录完成的事件
            ser1.LoginCompleted += (object sender1, OfficerService.LoginCompletedEventArgs e1) =>
            {
                LoginUser = JsonSerializerHelper.JsonToEntity<Officer>(e1.Result);
                if (LoginUser != null)
                {
                    isSuccess = true;
                    AppGlobal.CurrentUser = LoginUser;
                    try
                    {
                        string cookie = CookiesUtils.GetCookie("PageSize");
                        if (!string.IsNullOrEmpty(cookie))
                        {
                            foreach (var items in cookie.Split(','))
                            {
                                var item = items.Split(':');
                                if (item[0] == LoginUser.Id.ToString())
                                {
                                    string size = item[1];
                                    if (!string.IsNullOrEmpty(size))
                                    {
                                        int pageSize = 0;
                                        if (int.TryParse(size, out pageSize) && pageSize > 0)
                                            AppGlobal.PageSize = pageSize;

                                        break;
                                    }
                                }
                            }
                        }

                    }
                    catch(Exception e)
                    {

                    }
                    CallBack();
                }
            };
            ser1.LoginAsync(txtName.Text.Trim(), txtPass.Password.Trim());//异步调用服务器端登录
        }

        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScriptObject Show = HtmlPage.Window.GetProperty("closeWin") as ScriptObject;
                Show.InvokeSelf();
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login();
            }
        }

        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPass.ClearValue(PasswordBox.PasswordProperty);
        }
    }
}
