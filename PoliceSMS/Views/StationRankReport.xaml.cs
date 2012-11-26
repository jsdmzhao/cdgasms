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
using Telerik.Windows.Controls.Charting;
using PoliceSMS.Lib.Organization;
using Telerik.Windows.Controls.Animation;

namespace PoliceSMS.Views
{
    public partial class StationRankReport : Page
    {
        //silverlight没有keepalive属性
        //在页面跳转时保存条件
        private static DateTime? m_start;
        private static DateTime? m_end;
        private static string m_showType;

        public StationRankReport()
        {
            InitializeComponent();
            
            DateTime preMonth = DateTime.Now.AddMonths(-1);
            DateTime beginTime = new DateTime(preMonth.Year, preMonth.Month, 1);
            DateTime endTime = new DateTime(preMonth.Year, preMonth.Month, DateTime.DaysInMonth(preMonth.Year, preMonth.Month));

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;

            this.Loaded += new RoutedEventHandler(StationRankReport_Loaded);

            
        }

        void StationRankReport_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStation();

            if (m_start != null)
                dateStart.SelectedDate = m_start;
            if (m_end != null)
                dateEnd.SelectedDate = m_end;
            if (m_showType != null)
            {
                if (listShowType.Name == m_showType)
                    listShowType.IsChecked = true;
                if (chartShowType.Name == m_showType)
                    chartShowType.IsChecked = true;

            }
            else
            {
                listShowType.IsChecked = true;
            }

        }

        private void LoadStation()
        {

            try
            {
                OrganizationService.OrganizationServiceClient ser = new OrganizationService.OrganizationServiceClient();
                ser.GetListByHQLCompleted += (object sender, OrganizationService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var stations = JsonSerializerHelper.JsonToEntities<Organization>(e.Result, out total);
                    foreach (var station in stations)
                    {
                        if (station.Name.StartsWith("青羊区分局"))
                            station.Name = station.Name.Replace("青羊区分局", "");
                        if (station.Name.StartsWith("成都市公安局"))
                            station.Name = station.Name.Replace("成都市公安局", "");
                    }

                    List<TreeViewItemModel> list = new List<TreeViewItemModel>();
                    list.Add(new TreeViewItemModel
                    {
                        Id = 0,
                        SMSUnitType = 1,
                        Name = "派出所",
                        Childs = stations.Where(c => c.SMSUnitType == 1).OrderBy(c=>c.OrderIndex).ToList(),
                        IsExpanded = true,
                        IsActive = true
                    });
                    list.Add(new TreeViewItemModel
                    {
                        Id = 0,
                        SMSUnitType = 3,
                        Name = "科队",
                        Childs = stations.Where(c => c.SMSUnitType == 3).OrderBy(c => c.OrderIndex).ToList(),
                        IsExpanded = true,
                        IsActive = false
                    });

                    tree.ItemsSource = list;
                };

                //这里没有考虑权限
                ser.GetListByHQLAsync(string.Format(" select distinct  o from SMSRecord r inner join r.Organization as o where o.Name like '%青羊%' and o.SMSUnitType in ({0},{1})  ", 1,3));

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取单位发生错误", ex.Message, false);
            }

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem != null)
            {
                Organization org = tree.SelectedItem as Organization;
                if (org != null)
                {
                    if (org.Id == 0)
                        LoadReport(org.SMSUnitType);
                }
            }
        }

        public void LoadReport(int unitType)
        {
            if (dateStart.SelectedDate == null || dateEnd.SelectedDate == null)
            {
                Tools.ShowMessage("时间不能为空!", "", false);
                return;
            }

            btnExport.IsEnabled = false;
            Tools.ShowMask(true);
            
            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();

            ser.LoadStationReportResultCompleted += (object sender, ReportService.LoadStationReportResultCompletedEventArgs e) =>
                {
                    int total = 0;
                    IList<StationReportResult> result = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                    gv.ItemsSource = result;
                    gv.Items.Refresh();

                    rc.ItemsSource = result;

                    Tools.ShowMask(false);
                    btnExport.IsEnabled = true;
                    if (result == null || result.Count == 0)
                    {
                        Tools.ShowMessage("没有找到相对应的数据！", "", true);
                    }
                };

           
            DateTime beginTime1 = dateStart.SelectedDate.Value;
            DateTime endTime1 = dateEnd.SelectedDate.Value;

            TimeSpan span = endTime1 - beginTime1;
            endTime1 = endTime1.AddDays(1);

            DateTime endTime2 = beginTime1.AddDays(-1);

            DateTime beginTime2 = endTime2.Add(-span);

            Tools.ShowMask(true, "正在查找数据,请稍等...");
            ser.LoadStationReportResultAsync(unitType, beginTime1, endTime1, beginTime2, endTime2);

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

        private void tree_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is TreeViewItemModel)
                {
                    TreeViewItemModel org = e.AddedItems[0] as TreeViewItemModel;
                    LoadReport(org.SMSUnitType);
                }
                else if (e.AddedItems[0] is Organization)
                {
                    Organization org = e.AddedItems[0] as Organization;

                    m_start = dateStart.SelectedDate;
                    m_end = dateEnd.SelectedDate;
                    if (listShowType.IsChecked == true)
                        m_showType = listShowType.Name;
                    if (chartShowType.IsChecked == true)
                        m_showType = chartShowType.Name;

                    string uri = string.Format("/Views/OfficerRankReport.xaml?OrgId={0}&Start={1}&End={2}", org.Id, dateStart.SelectedDate, dateEnd.SelectedDate);
                    this.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
                }

            }
        }

    }
}
