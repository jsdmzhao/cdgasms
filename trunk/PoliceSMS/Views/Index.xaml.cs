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
using System.Text;
using PoliceSMS.Comm;
using PoliceSMS.ViewModel;
using PoliceSMS.Lib.Report;
using Telerik.Windows.Controls;

namespace PoliceSMS.Views
{
    public partial class Index : Page
    {
        public Index()
        {
            InitializeComponent();

            page1.UnitType = 1;
            //page2.UnitType = 2;
            page3.UnitType = 3;

            page1.LoadReport();
            page3.LoadReport(false);
        }


    }
}
