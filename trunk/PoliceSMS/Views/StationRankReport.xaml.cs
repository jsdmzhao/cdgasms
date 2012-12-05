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
        private static bool isInit = false;
        private static DateTime? m_start;
        private static DateTime? m_end;
        private static string m_showType;

        public StationRankReport()
        {
            InitializeComponent();

            if (!isInit)
            {
                DateTime preMonth = DateTime.Now.AddMonths(-1);
                m_start = new DateTime(preMonth.Year, preMonth.Month, 1);
                m_end = new DateTime(preMonth.Year, preMonth.Month, DateTime.DaysInMonth(preMonth.Year, preMonth.Month));
                isInit = true;
            }
            this.Loaded += new RoutedEventHandler(StationRankReport_Loaded);
        }

        void StationRankReport_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStation();

            dateStart.SelectedDate = m_start;
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
                        IsExpanded = false,
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
            if (tree.SelectedItem is TreeViewItemModel)
            {
                TreeViewItemModel org = tree.SelectedItem as TreeViewItemModel;
                if (org != null)
                {
                    if (org.Id == 0)
                        LoadReport(org.SMSUnitType);
                }
            }
        }

        public void LoadReport(int unitType)
        {
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

           
            DateTime beginTime1 = new DateTime().SqlMinValue();
            DateTime endTime1 = new DateTime().SqlMaxValue();
            if (dateStart.SelectedDate != null)
                beginTime1 = dateStart.SelectedDate.Value;

            if (dateEnd.SelectedDate != null)
            {
                var tmp = dateEnd.SelectedDate.Value;
                endTime1 = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
            }

            //DateTime endTime2 = beginTime1.AddSeconds(-1);
            //DateTime beginTime2 = endTime2 - (endTime1 - beginTime1);

            Tools.ShowMask(true, "正在查找数据,请稍等...");
            ser.LoadStationReportResultAsync(unitType, beginTime1, endTime1, beginTime1, endTime1);

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
                object o = e.AddedItems[0];
                if (o is TreeViewItemModel)
                {
                    TreeViewItemModel org = o as TreeViewItemModel;
                    LoadReport(org.SMSUnitType);
                }
                else if (o is Organization)
                {
                    Organization org = o as Organization;

                    drill(org.Id);
                }

            }

        }

        

        private void gv_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                object o = e.AddedItems[0];
                if (o is StationReportResult)
                {
                    StationReportResult item = o as StationReportResult;
                    
                    drill(item.UnitId);
                }
               
            }

        }
       

        private void ChartArea_ItemClick(object sender, ChartItemClickEventArgs e)
        {
            if (e.DataPoint !=null && e.DataPoint.DataItem!=null)
            {
                object o = e.DataPoint.DataItem;
                if (o is StationReportResult)
                {
                    StationReportResult item = o as StationReportResult;

                    drill(item.UnitId);
                }

            }
        }

        private void drill(int orgId)
        {
            m_start = dateStart.SelectedDate;
            m_end = dateEnd.SelectedDate;
            if (listShowType.IsChecked == true)
                m_showType = listShowType.Name;
            if (chartShowType.IsChecked == true)
                m_showType = chartShowType.Name;

            string uri = string.Format("/Views/OfficerRankReport.xaml?OrgId={0}&Start={1}&End={2}", orgId, dateStart.SelectedDate, dateEnd.SelectedDate);
            this.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }
        private void All_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tree.SelectedItem is TreeViewItemModel)
                {
                    TreeViewItemModel org = tree.SelectedItem as TreeViewItemModel;
                    if (org != null)
                    {
                        if (org.Id == 0)
                            LoadReport(org.SMSUnitType);
                    }
                }
            }            
        }
    }
}
