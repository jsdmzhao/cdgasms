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
    public partial class StationRankReport : Page
    {
        public int UnitType { get; set; }

        public StationRankReport()
        {
            InitializeComponent();
            DateTime endTime = DateTime.Now;
            DateTime beginTime = new DateTime(endTime.Year, endTime.Month, 1);

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;
        }


        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            Tools.ShowMask(true);
            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();

            ser.LoadStationReportResultCompleted += (object sender, ReportService.LoadStationReportResultCompletedEventArgs e) =>
                {
                    int total = 0;
                    IList<StationReportResult> result = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                    gv.ItemsSource = result;

                    gv.Items.Refresh();
                    Tools.ShowMask(false);
                };

            DateTime beginTime1 = dateStart.SelectedDate.Value;
            DateTime endTime1 = dateEnd.SelectedDate.Value.AddDays(1);

            TimeSpan span = endTime1 - beginTime1;


            DateTime endTime2 = beginTime1.AddDays(-1);

            DateTime beginTime2 = endTime1.Add(-span);

            ser.LoadStationReportResultAsync(UnitType, beginTime1, endTime1, beginTime2, endTime2);

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
