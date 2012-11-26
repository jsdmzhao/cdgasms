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
using System.Windows.Navigation;

namespace PoliceSMS.Views
{
    public partial class DrillContainer : UserControl
    {
     
        public DrillContainer()
        {
            InitializeComponent();
            this.mainFrame.Source = new Uri("/Views/StationRankReport.xaml",UriKind.RelativeOrAbsolute);
        }


        private void mainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            btnBack.IsEnabled = mainFrame.CanGoBack;
            btnNxt.IsEnabled = mainFrame.CanGoForward;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoBack();
        }

        private void btnNxt_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoForward();
        }

    }
}
