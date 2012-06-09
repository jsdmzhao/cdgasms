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
using PoliceSMS.Lib.Report;
using PoliceSMS.Comm;
using PoliceSMS.ViewModel;
using Telerik.Windows.Controls;
using PoliceSMS.Lib.Organization;
using System.Reflection;
using System.Text;

namespace PoliceSMS.Views
{
    public partial class OfficerRankReport : Page
    {

        public int UnitType { get; set; }
        public IList<Organization> stations;

        public OfficerRankReport()
        {
            InitializeComponent();

            DateTime preMonth = DateTime.Now.AddMonths(-1);
            DateTime beginTime = new DateTime(preMonth.Year, preMonth.Month, 1);
            DateTime endTime = new DateTime(preMonth.Year, preMonth.Month, DateTime.DaysInMonth(preMonth.Year, preMonth.Month));

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;

            LoadStation();
        }


        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
           
            LoadReport();
        }


        private void LoadStation()
        {
            try
            {
                OrganizationService.OrganizationServiceClient ser = new OrganizationService.OrganizationServiceClient();
                ser.GetListByHQLCompleted += (object sender, OrganizationService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    stations = JsonSerializerHelper.JsonToEntities<Organization>(e.Result, out total);

                    Organization orgNull = new Organization { Name = "全部", Id = 0 };

                    stations.Insert(0, orgNull);

                    
                    cmbStation.ItemsSource = stations;
                    if (cmbStation.Items.Count > 0)
                        cmbStation.SelectedIndex = 0;

                };

                //这里没有考虑权限
                ser.GetListByHQLAsync("from Organization where Name like '%青羊%' and SMSUnitType>0 order by OrderIndex ");

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取单位发生错误", ex.Message, false);
            }
        }

        private void LoadReport()
        {
            if (cmbStation.SelectedItem == null)
            {
                Tools.ShowMessage("请选择单位!", "", false);
                return;
            }

            btnExport.IsEnabled = false;
            Tools.ShowMask(true);

            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();
            
            Organization selOrg =(Organization) cmbStation.SelectedItem;
            
            ser.LoadOfficerReportResultCompleted+= (object sender, ReportService.LoadOfficerReportResultCompletedEventArgs e) =>
                {
                    int total = 0;
                    IList<StationReportResult> result = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                    gv.ItemsSource = result;

                    gv.Items.Refresh();
                    Tools.ShowMask(false);
                    btnExport.IsEnabled = true;
                };

            DateTime beginTime1 = DateTime.Now.AddDays(-1);
            DateTime endTime1 = DateTime.Now;
            if(dateStart.SelectedDate!=null)
                beginTime1 = dateStart.SelectedDate.Value;
            
            if(dateEnd.SelectedDate!=null)
                endTime1 = dateEnd.SelectedDate.Value;

            TimeSpan span = endTime1 - beginTime1;
            endTime1 = endTime1.AddDays(1);

            
            DateTime endTime2 = beginTime1.AddDays(-1);

            DateTime beginTime2 = endTime2.Add(-span);

            int unitId = selOrg == null ? 0 : selOrg.Id;

            ser.LoadOfficerReportResultAsync(unitId, beginTime1, endTime1, beginTime2, endTime2, string.Format("{0}%", tbOfficerName.Text));

        }


        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportingModel export = new ExportingModel();
            export.SelectedExportFormat = "Excel";

            Assembly assembly = GetType().Assembly;
            string html = string.Empty;
            using (System.IO.Stream htmlStream = assembly.GetManifestResourceStream("PoliceSMS.HeaderTemplate.OfficerHeader.htm"))
            {
                byte[] bsHtml = new byte[htmlStream.Length];
                htmlStream.Read(bsHtml, 0, (int)htmlStream.Length);
                html = Encoding.UTF8.GetString(bsHtml, 0, bsHtml.Length);
            }

            export.ExportWithHeader(gv, html);
        }

    }
}
