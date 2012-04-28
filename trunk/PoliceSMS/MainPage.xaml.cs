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

            

            //SMSRecord record = new SMSRecord();

            //record.Organization = AppGlobal.CurrentOrganization;
            //record.LoginOfficer = AppGlobal.CurrentUser;

            //SMSRecordForm smsFrom = new SMSRecordForm();
            //smsFrom.SMSRecord = record;

            //Tools.OpenWindow("评警信息", smsFrom,null,600,375);

        }


        private void onLoad()
        {
            AppGlobal.CurrentUser = new Lib.Organization.Officer { Name = "测试用户",Password="111111",Id=1,Organization= new Lib.Organization.Organization { Name = "青羊区分局草市街派出所", Id = 742 }};
            
            NameBlock.Text = AppGlobal.CurrentUser.Name;
            contentGrid.Visibility = Visibility.Visible;

            //LoginForm logFrm = new LoginForm();
            //logFrm.CallBack = () =>
            //{
            //    NameBlock.Text = AppGlobal.CurrentUser.Name;
            //    contentGrid.Visibility = Visibility.Visible;

            //    sy.IsChecked = true;
            //    (logFrm.Parent as RadWindow).Close();

            //};
            //Tools.OpenWindow("登录", logFrm, null);
            
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
          
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
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

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            ListBoxItem item = lb.SelectedItem as ListBoxItem;
            if (item != null && item.Tag != null)
                contentFrame.Navigate(new Uri("/Views/" + item.Tag.ToString() + ".xaml", UriKind.Relative));
        }

        private void menu_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            var bar = (sender as Telerik.Windows.Controls.RadPanelBar);
            var baritem = bar.SelectedItem as Telerik.Windows.Controls.RadPanelBarItem;
            foreach (var item in baritem.Items)
            {
                if (item is ListBox)
                {
                    (item as ListBox).SelectedIndex = -1;
                    break;
                }
            }
        }



    }
}
