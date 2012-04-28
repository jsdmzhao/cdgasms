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
using Telerik.Windows.Controls;
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

            sy.IsChecked = true;

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

        private void RadRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadRadioButton btn = sender as RadRadioButton;
            if (btn != null && btn.Tag != null && VerifyRight(btn.Tag.ToString()))
            {
                if (btn.Tag.ToString().IndexOf("!") != -1)
                {
                    StackPanel panel = this.FindName(btn.Tag.ToString().Replace("!", "")) as StackPanel;
                    IEnumerable<RadRadioButton> btns = panel.ChildrenOfType<RadRadioButton>();
                    foreach (RadRadioButton bn in btns)
                    {
                        if (bn.IsChecked.Value && bn.Tag != null)
                        {
                            if (bn.Tag.ToString().IndexOf("@") != -1)
                            {
                                Type type = Type.GetType("PoliceSMS.Views." + bn.Tag.ToString().Replace("@", ""));
                                Object obj = Activator.CreateInstance(type);
                                Tools.OpenWindow(bn.Content.ToString(), obj, null);
                            }
                            else
                            {
                                contentFrame.Navigate(new Uri("/Views/" + bn.Tag.ToString() + ".xaml", UriKind.Relative));
                            }
                        }
                    }
                }
                else if (btn.Tag.ToString().IndexOf("@") != -1)
                {
                    Type type = Type.GetType("PoliceSMS.Views." + btn.Tag.ToString().Replace("@", ""));
                    Object obj = Activator.CreateInstance(type);
                    Tools.OpenWindow(btn.Content.ToString(), obj, null);
                }
                else
                {
                    contentFrame.Navigate(new Uri("/Views/" + btn.Tag.ToString() + ".xaml", UriKind.Relative));
                }
            }
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
