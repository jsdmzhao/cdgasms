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
using PoliceSMS.Lib.Core;

namespace PoliceSMS.Views
{
    public partial class OfficerRankReport : Page
    {

        public int UnitType { get; set; }
        public IList<Organization> stations;

        bool isOfficerSelected = false;
        bool autoReport = false;
        

        public OfficerRankReport()
        {
            InitializeComponent();

            rDataPager1.PageSize = AppGlobal.PageSize;
            DateTime preMonth = DateTime.Now.AddMonths(-1);
            DateTime beginTime = new DateTime(preMonth.Year, preMonth.Month, 1);
            DateTime endTime = new DateTime(preMonth.Year, preMonth.Month, DateTime.DaysInMonth(preMonth.Year, preMonth.Month));

            dateEnd.SelectedDate = endTime;
            dateStart.SelectedDate = beginTime;

            this.Loaded += new RoutedEventHandler(OfficerRankReport_Loaded);
        }

        void OfficerRankReport_Loaded(object sender, RoutedEventArgs e)
        {
            isOfficerSelected = false;
            autoReport = false;
            int orgId = 0;
            
            if (this.NavigationContext != null)
            {
                DateTime? start = null;
                DateTime? end = null;
                if (this.NavigationContext.QueryString.Keys.Contains("OrgId"))
                    int.TryParse(this.NavigationContext.QueryString["OrgId"], out orgId);
                if (this.NavigationContext.QueryString.Keys.Contains("Start"))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(this.NavigationContext.QueryString["Start"], out tmp))
                        start = tmp;
                }
                if (this.NavigationContext.QueryString.Keys.Contains("End"))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(this.NavigationContext.QueryString["End"], out tmp))
                        end = tmp;
                }
                dateStart.SelectedDate = start;
                dateEnd.SelectedDate = end;
            }

            

            if (orgId == 0)
            {
                LoadSortTypes();
                Action action = new Action(
                    () =>
                    {
                        LoadStation(new Action(() =>
                        {
                            autoReport = true;
                        }
                        ));
                    }
                );
                LoadOfficerTypes(action);
            }
            else
            {
                this.dateStart.IsEnabled = false;
                this.dateEnd.IsEnabled = false;
                this.cmbStation.IsEnabled = false;
                LoadSortTypes();
                Action action = new Action(
                    () =>
                    {
                        LoadStation(new Action(() =>
                        {
                            var o = stations.SingleOrDefault(c => c.Id == orgId);
                            if (o != null)
                                cmbStation.SelectedItem = o;
                            LoadReport(new Action(() => autoReport = true));

                        }
                        ));
                    }
                );
                LoadOfficerTypes(action);
            }

        }


        void gv_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (this.NavigationService != null)
            {
                if (isOfficerSelected)
                {
                    if (e.AddedItems != null && e.AddedItems.Count > 0)
                    {
                        StationReportResult obj = e.AddedItems[0] as StationReportResult;
                        if (obj != null)
                        {
                            OfficerType type = lbType.SelectedItem as OfficerType;

                            string uri = string.Format("/Views/SMSRecordListNew.xaml?OfficerId={0}&Start={1}&End={2}&OfficerTypeId={3}",
                                obj.UnitId, dateStart.SelectedDate, dateEnd.SelectedDate, type == null ? "" : type.Id.ToString());
                            this.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
                        }
                    }
                }
            }
        }


        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }


        private void LoadStation(Action action = null)
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

                    if (action != null)
                    {
                        action();
                    }

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
        private void LoadReport(Action action = null)
        {
            if (cmbStation.SelectedItem == null)
            {
                Tools.ShowMessage("请选择单位!", "", false);
                return;
            }

            btnExport.IsEnabled = false;
            Tools.ShowMask(true,"正在查找数据,请稍等...");

            ReportService.ReportWcfClient ser = new ReportService.ReportWcfClient();
            
            Organization selOrg =(Organization) cmbStation.SelectedItem;
            
            ser.LoadOfficerByOrderReportResultCompleted+= (object sender, ReportService.LoadOfficerByOrderReportResultCompletedEventArgs e) =>
                {
                    int total = 0;
                    list = JsonSerializerHelper.JsonToEntities<StationReportResult>(e.Result, out total);
                    
                    rDataPager1.Source = list;
                    gv.CurrentItem = null;
                    isOfficerSelected = true;
                    if(action!=null)
                    {
                        action();
                    }

                    Tools.ShowMask(false);
                    btnExport.IsEnabled = true;
                    if (list == null || list.Count == 0)
                    {
                        Tools.ShowMessage("没有找到相对应的数据！","",true);
                    }

                };

            DateTime beginTime1 = new DateTime().SqlMinValue();
            DateTime endTime1 = new DateTime().SqlMaxValue();
            if(dateStart.SelectedDate!=null)
                beginTime1 = dateStart.SelectedDate.Value;

            if (dateEnd.SelectedDate != null)
            {
                var tmp = dateEnd.SelectedDate.Value;
                endTime1 = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
            }

            int unitId = selOrg == null ? 0 : selOrg.Id;
            //////////警种
            int officerType = 0;
            if (lbType.SelectedItem != null)
            {
                var obj = lbType.SelectedItem as OfficerType;
                if (obj != null)
                    officerType = obj.Id;
            }
            //////////排序条件
            int orderIndex = 0;
            if (lbSort.SelectedItem != null)
            {
                var obj = lbSort.SelectedItem as SortType;
                if (obj != null)
                    orderIndex = obj.Id;
            }

            ser.LoadOfficerByOrderReportResultAsync(unitId, beginTime1, endTime1, string.Format("{0}%", tbOfficerName.Text), officerType, orderIndex);

            isOfficerSelected = false;
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

        private void LoadOfficerTypes(Action action = null)
        {
            try
            {
                //WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../SMSWcf/WorkTypeService.svc")));
                OfficerTypeService.OfficerTypeServiceClient ser = new OfficerTypeService.OfficerTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, OfficerTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var officerTypes = JsonSerializerHelper.JsonToEntities<OfficerType>(e.Result, out total);

                    officerTypes.Insert(0, new OfficerType { Id = 0, Name = "全部" });
                    lbType.ItemsSource = officerTypes;
                    lbType.SelectedIndex = 0;

                    if (action != null)
                        action();
                };

                ser.GetListByHQLAsync("from OfficerType");

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取警种发生错误", ex.Message, false);
            }
        }

        private void LoadSortTypes()
        {
            SortType[] objs = new SortType[] { new SortType {Id=1, Name = "差评" }, new SortType {Id=2, Name = "满意率" }, 
                new SortType {Id=3, Name = "受理总量" } };
            lbSort.ItemsSource = objs;
            lbSort.SelectedIndex = 0;
        }


        private void lbType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (autoReport)
                 LoadReport();
        }

        private void lbSort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(autoReport)
                LoadReport();
        }

       

    }

}
