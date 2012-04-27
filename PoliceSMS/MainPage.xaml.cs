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
            
        }

        private void onExit()
        {

           

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
