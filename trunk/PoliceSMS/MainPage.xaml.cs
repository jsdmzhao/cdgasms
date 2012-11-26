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
using PoliceSMS.Views;
using PoliceSMS.Lib.SMS;
using Telerik.Windows.Controls;

using mc = System.Windows.Controls;
using System.Reflection;

namespace PoliceSMS
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }



        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            onLoad();
            
        }


        private void onLoad()
        {
            LoginForm logFrm = new LoginForm();
            logFrm.CallBack = () =>
            {
                NameBlock.Text = AppGlobal.CurrentUser.Name;
                userPnl.Visibility = Visibility.Visible;
                dataGrid.Visibility = Visibility.Visible;
                (logFrm.Parent as RadWindow).Close();
                if (AppGlobal.HasPermission())
                    superVisionItem.Visibility = workTypeItem.Visibility = Visibility.Visible;
            };
            Tools.OpenWindow("登录", logFrm, null);
            
        }

        private void onExit()
        {
            HtmlWindow html = HtmlPage.Window;
            html.Navigate(new Uri("PoliceSMSTestPage.aspx", UriKind.Relative));//相对
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            onExit();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PassWordForm form = new PassWordForm();
            form.CallBack = () =>
            {
                Tools.ShowMessage("修改密码成功！", "", true);
                (form.Parent as RadWindow).Close();

            };
            Tools.OpenWindow("修改密码", form, null, 450, 250);
        }


        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private bool VerifyRight(string tag)
        {
            return true;
        }
    }
}
