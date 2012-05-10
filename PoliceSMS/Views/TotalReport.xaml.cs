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
using PoliceSMS.Comm;
using PoliceSMS.Lib.Report;
using PoliceSMS.ViewModel;
using System.Reflection;
using System.Text;

namespace PoliceSMS.Views
{
    public partial class TotalReport : Page
    {
        public TotalReport()
        {
            InitializeComponent();

            dateStart.SelectedDate = DateTime.Now.AddMonths(-12);
            dateEnd.SelectedDate = DateTime.Now;
            dateStart.Culture.DateTimeFormat.ShortDatePattern = "yyyy-MM";
            dateEnd.Culture.DateTimeFormat.ShortDatePattern = "yyyy-MM";

            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }

        public void LoadReport()
        {
            Tools.ShowMask(true);
            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();

            ser.LoadTotalReportResultCompleted += (object sender, ReportService.LoadTotalReportResultCompletedEventArgs e) =>
            {
                int total = 0;
                IList<StationReportResult> result = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                gv.ItemsSource = result;

                gv.Items.Refresh();
                Tools.ShowMask(false);
            };

            DateTime beginTime = dateStart.SelectedDate.Value;
            DateTime endTime = dateEnd.SelectedDate.Value;

            int begin = beginTime.Year * 100 + beginTime.Month;
            int end = endTime.Year * 100 + endTime.Month;

            ser.LoadTotalReportResultAsync(begin, end);

        }


        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportingModel export = new ExportingModel();

            export.SelectedExportFormat = "Excel";

            Assembly assembly = GetType().Assembly;
            string html = string.Empty;
            using (System.IO.Stream htmlStream = assembly.GetManifestResourceStream("PoliceSMS.HeaderTemplate.TotalHeader.htm"))
            {
                byte[] bsHtml = new byte[htmlStream.Length];
                htmlStream.Read(bsHtml, 0, (int)htmlStream.Length);
                html = Encoding.UTF8.GetString(bsHtml, 0, bsHtml.Length);
            }

            export.ExportWithHeader(gv, html);
        }

    }
}
