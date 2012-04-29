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
            //AppGlobal.CurrentUser = new Lib.Organization.Officer { Name = "测试用户",Password="111111",Id=1,Organization= new Lib.Organization.Organization { Name = "青羊区分局草市街派出所", Id = 742 }};
            
            //NameBlock.Text = AppGlobal.CurrentUser.Name;
            //contentGrid.Visibility = Visibility.Visible;

            LoginForm logFrm = new LoginForm();
            logFrm.CallBack = () =>
            {
                NameBlock.Text = AppGlobal.CurrentUser.Name;
                contentGrid.Visibility = Visibility.Visible;

                (logFrm.Parent as RadWindow).Close();

            };
            Tools.OpenWindow("登录", logFrm, null);
            
        }

        private void onExit()
        {
            //AppGlobal.CurrentUser = new Lib.Organization.Officer();
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
            mc.ListBox lb = sender as mc.ListBox;
            mc.ListBoxItem item = lb.SelectedItem as mc.ListBoxItem;
            if (item != null && item.Tag != null)
            {
                string tag = item.Tag.ToString();
                //如果不存在tab创建一个，否则直接跳转
                var existTab = tabGroup.Items.SingleOrDefault(c => (c as RadDocumentPane).Tag.ToString() == tag) as RadDocumentPane;
                if (existTab == null)
                {
                    RadDocumentPane tab = cretateTab(tag);
                    tab.Tag = tag;
                    tabGroup.Items.Add(tab);
                    
                }
                else
                {
                    tabGroup.SelectedItem = existTab;
                }
            }
        }

        private void menu_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            var bar = (sender as RadPanelBar);
            var baritem = bar.SelectedItem as RadPanelBarItem;
            foreach (var item in baritem.Items)
            {
                if (item is mc.ListBox)
                {
                    (item as mc.ListBox).SelectedIndex = -1;
                    break;
                }
            }
        }

        //创建一个tab页
        RadDocumentPane cretateTab(string key)
        {
            RadDocumentPane tab = new RadDocumentPane();
            tab.Header = tab.Title = Dict.GetStr(key);

            Type ttt = Type.GetType("PoliceSMS.Views." + key);
            var obj = ttt.GetConstructor(Type.EmptyTypes).Invoke(null);
            tab.Content = obj;
            return tab;

        }

    }
}
