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

namespace PoliceSMS.Views
{
    public partial class StationRankReport : Page
    {
        public StationRankReport()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void allSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void noneSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportingModel export = new ExportingModel();
            Header header = new Header();
            HeaderCell[] cells = new HeaderCell[6];
            cells[0] = new HeaderCell { ColSpan = 5, Name = "" };
            cells[1] = new HeaderCell { ColSpan = 2, Name = "非常满意" };
            cells[2] = new HeaderCell { ColSpan = 2, Name = "满意" };
            cells[3] = new HeaderCell { ColSpan = 2, Name = "一般" };
            cells[4] = new HeaderCell { ColSpan = 2, Name = "业务不熟" };
            cells[5] = new HeaderCell { ColSpan = 2, Name = "态度欠佳" };
            header.cells = cells;

            export.SelectedExportFormat = exportType.SelectedItem as string;
            export.ExportWithHeader(gv, header);
        }
    }
}
