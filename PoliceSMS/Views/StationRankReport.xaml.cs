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
        private int unitType = -1;
        public int UnitType
        {
            get
            {
                return unitType;
            }
            set
            {
                unitType = value;
                LoadStation();
            }
        }

        public StationRankReport()
        {
            InitializeComponent();
            
            DateTime preMonth = DateTime.Now.AddMonths(-1);
            DateTime beginTime = new DateTime(preMonth.Year, preMonth.Month, 1);
            DateTime endTime = new DateTime(preMonth.Year, preMonth.Month, DateTime.DaysInMonth(preMonth.Year, preMonth.Month));

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;

            LoadStation();
        }

        private void LoadStation()
        {
            if (unitType == -1)
                return;
            if (unitType == 1)
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
                            //去掉‘青羊区分局’
                            if (station.Name.StartsWith("青羊区分局"))
                                station.Name = station.Name.Replace("青羊区分局", "");
                        }


                        lb.ItemsSource = stations;

                    };

                    //这里没有考虑权限
                    ser.GetListByHQLAsync(string.Format("from Organization where Name like '%青羊%' and SMSUnitType={0} order by OrderIndex ", unitType));

                }
                catch (Exception ex)
                {
                    Tools.ShowMessage("读取单位发生错误", ex.Message, false);
                }
            }
            if (unitType == 3)
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


                        lb.ItemsSource = stations;

                    };

                    //这里没有考虑权限
                    ser.GetListByHQLAsync(string.Format("select distinct  o from SMSRecord r inner join r.Organization as o where o.Name like '%青羊%' and o.SMSUnitType={0} order by o.OrderIndex ", unitType));

                }
                catch (Exception ex)
                {
                    Tools.ShowMessage("读取单位发生错误", ex.Message, false);
                }
            }
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
            Tools.ShowMask(true, "正在查找数据,请稍等...");
        }

        bool showTooltip = true;
        public void LoadReport(bool showTooltip = true)
        {
            this.showTooltip = showTooltip;
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
                    if (result == null || result.Count == 0 && showTooltip == true)
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

        private void lb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                Organization org = e.AddedItems[0] as Organization;
                if (org != null)
                {
                    AppGlobal.CurrentDialogPage = new OfficerRankReport(org, dateStart.SelectedDate, dateEnd.SelectedDate);

                    Tools.OpenCustomWindow(string.Format("{0}个人排名表", org.Name), AppGlobal.CurrentDialogPage, new Action(() =>
                        {
                            lb.SelectedIndex = -1;
                            //必须置空，否则RadBusyIndicator不会正常显示
                            AppGlobal.CurrentDialogPage = null;
                        }));
                }
            }
        }

    }
}
