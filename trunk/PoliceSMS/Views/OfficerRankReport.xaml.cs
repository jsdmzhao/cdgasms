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

            rDataPager1.PageSize = AppGlobal.PageSize;

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

        IList<StationReportResult> list;
        private void LoadReport()
        {
            if (dateStart.SelectedDate == null || dateEnd.SelectedDate == null)
            {
                Tools.ShowMessage("时间不能为空!", "", false);
                return;
            }

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
                    list = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                    
                    rDataPager1.Source = list;

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
            if (list == null || list.Count == 0)
                return;
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

            gvtmp.ItemsSource = list;
            export.ExportWithHeader(gvtmp, html);
        }

        private void rDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
        }


        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text;
            pageSizeUpdate(sender, text);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = (sender as TextBox).Text;
            pageSizeUpdate(sender, text);
        }

        private void pageSizeUpdate(object sender, string text)
        {
            int size = 0;

            if (int.TryParse(text, out size) && size > 0)
            {
                if (size > 30)
                    size = 30;

                rDataPager1.PageSize = size;
                (sender as TextBox).Text = size.ToString();

                try
                {
                    string cookie = CookiesUtils.GetCookie("PageSize");
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        var list = cookie.Split(',').ToList();
                        bool isExist = false;
                        for (int i = 0; i < list.Count; i++)
                        {
                            string items = list[i];
                            var item = items.Split(':');
                            if (item[0] == AppGlobal.CurrentUser.Id.ToString())
                            {
                                list[i] = string.Format("{0}:{1}", AppGlobal.CurrentUser.Id.ToString(), size.ToString());
                                isExist = true;
                                cookie = string.Join(",", list);
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            list.Add(string.Format("{0}:{1}", AppGlobal.CurrentUser.Id.ToString(), size.ToString()));
                            cookie = string.Join(",", list);
                        }
                        CookiesUtils.SetCookie("PageSize", cookie, new TimeSpan(90, 0, 0, 0));
                    }
                    else
                    {
                        cookie = string.Format("{0}:{1}", AppGlobal.CurrentUser.Id.ToString(), size.ToString());
                        CookiesUtils.SetCookie("PageSize", cookie, new TimeSpan(90, 0, 0, 0));
                    }

                }
                catch
                {
                }
            }
        }

       

    }
}
