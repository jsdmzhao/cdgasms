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

namespace PoliceSMS.Views
{
    public partial class OfficerRankReport : Page
    {

        public int UnitType { get; set; }
        public IList<Organization> stations;

        public OfficerRankReport()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(StationRankReport_Loaded);
        }

        void StationRankReport_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime endTime = DateTime.Now;
            DateTime beginTime = new DateTime(endTime.Year, endTime.Month, 1);

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

                };

                //这里没有考虑权限
                ser.GetListByHQLAsync("from Organization where Name like '%青羊%' and SMSUnitType>0");

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
                };

            DateTime beginTime1 = dateStart.SelectedDate.Value;
            DateTime endTime1 = dateEnd.SelectedDate.Value;

            TimeSpan span = endTime1 - beginTime1;

            
            DateTime endTime2 = beginTime1.AddDays(-1);

            DateTime beginTime2 = endTime1.Add(-span);

            int unitId = selOrg == null ? 0 : selOrg.Id;

            ser.LoadOfficerReportResultAsync(unitId, beginTime1, endTime1, beginTime2, endTime2);

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
