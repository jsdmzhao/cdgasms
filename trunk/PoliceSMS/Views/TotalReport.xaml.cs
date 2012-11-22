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
using PoliceSMS.Lib.Organization;

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
            Tools.ShowMask(true, "正在查找数据,请稍等...");
        }

        private string getFromatDate(string date)
        {
            int yearMonth = int.Parse(date);
            int year = yearMonth / 100;
            int month = yearMonth % 100;
            return string.Format("{0}年{1}月", year, month);
        }

        public void LoadReport()
        {
            if (dateStart.SelectedDate == null || dateEnd.SelectedDate == null)
            {
                Tools.ShowMessage("时间不能为空!", "", false);
                return;
            }

            btnExport.IsEnabled = false;
            Tools.ShowMask(true);
            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();

            ser.LoadTotalReportResultCompleted += (object sender, ReportService.LoadTotalReportResultCompletedEventArgs e) =>
            {
                int total = 0;
                IList<StationReportResult> result = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                gv.ItemsSource = result;
                gv.Items.Refresh();

                rc.ItemsSource = result.OrderBy(c => int.Parse(c.UnitName)).Select(c => new ChartModel{ X = getFromatDate(c.UnitName), IntY = c.TotalCount });

                Tools.ShowMask(false);
                btnExport.IsEnabled = true;
                if (result == null || result.Count == 0)
                {
                    Tools.ShowMessage("没有找到相对应的数据！","",true);
                }
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

        private void list_Checked(object sender, RoutedEventArgs e)
        {
            if (rc != null)
                rc.Visibility = Visibility.Collapsed;
            if (sv != null)
                sv.Visibility = Visibility.Visible;
        }

        private void chart_Checked(object sender, RoutedEventArgs e)
        {
            if (rc != null)
                rc.Visibility = Visibility.Visible;
            if (sv != null)
                sv.Visibility = Visibility.Collapsed;
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
