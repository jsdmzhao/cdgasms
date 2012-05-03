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
using System.Reflection;

namespace PoliceSMS.Views
{
    public partial class StationRankReport : Page
    {
        public int UnitType { get; set; }

        public StationRankReport()
        {
            InitializeComponent();
            DateTime currMonth = DateTime.Now;
            DateTime preMonth = currMonth.AddMonths(-1);
            DateTime beginTime = new DateTime(preMonth.Year, preMonth.Month, 1);
            DateTime endTime = new DateTime(currMonth.Year, currMonth.Month, 1);

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;
            
        }


        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }

        public void LoadReport()
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
            DateTime endTime1 = dateEnd.SelectedDate.Value;

            TimeSpan span = endTime1 - beginTime1;
            endTime1 = endTime1.AddDays(1);

            DateTime endTime2 = beginTime1.AddDays(-1);

            DateTime beginTime2 = endTime2.Add(-span);

            ser.LoadStationReportResultAsync(UnitType, beginTime1, endTime1, beginTime2, endTime2);

        }


        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportingModel export = new ExportingModel();

            export.SelectedExportFormat = "Excel";

            Assembly assembly = GetType().Assembly;
            string html = string.Empty;
            using (System.IO.Stream htmlStream = assembly.GetManifestResourceStream("PoliceSMS.HeaderTemplate.StationHeader.htm"))
            {
                byte[] bsHtml = new byte[htmlStream.Length];
                htmlStream.Read(bsHtml, 0, (int)htmlStream.Length);
                html = Encoding.UTF8.GetString(bsHtml, 0, bsHtml.Length);
            }

            export.ExportWithHeader(gv, html);
        }

    }
}
