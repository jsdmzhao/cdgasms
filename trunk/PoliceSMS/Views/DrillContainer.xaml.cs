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
        public int UnitTypeId { get; set; }
        public bool ShowTooltip { get; set; }

        public DrillContainer()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(DrillContainer_Loaded);
        }

        private bool isInit = false;

        void DrillContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInit)
            {
                isInit = true;
                string uri = string.Format("/Views/StationRankReport.xaml?UnitTypeId={0}&ShowTooltip={1}", UnitTypeId, ShowTooltip);
                mainFrame.Source = new Uri(uri, UriKind.RelativeOrAbsolute);
            }
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
