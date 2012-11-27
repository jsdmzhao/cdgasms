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
using PoliceSMS.Lib.Query;
using Telerik.Windows.Controls.GridView;
using PoliceSMS.ViewModel;
using PoliceSMS.Comm;
using PoliceSMS.Lib.SMS;
using Telerik.Windows;
using PoliceSMS.Lib.Organization;
using System.Text;
using System.Windows.Browser;

namespace PoliceSMS.Views
{
    public partial class SMSRecordListNew : Page
    {
        SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();
        private const int PageSize = 19;
        private QueryCondition queryCondition = null;
        
        public SMSRecordListNew()
        {
            InitializeComponent();

            ser.GetListByHQLCompleted += new EventHandler<SMSRecordService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<SMSRecordService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);
            ser.GetListByHQLWithPagingCompleted += new EventHandler<SMSRecordService.GetListByHQLWithPagingCompletedEventArgs>(ser_GetListByHQLWithPagingCompleted);
            ser.ExportCompleted += new EventHandler<SMSRecordService.ExportCompletedEventArgs>(ser_ExportCompleted);

           
            dateStart.SelectedDate = DateTime.Now.AddDays(-7);
            dateEnd.SelectedDate = DateTime.Now;

            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);

            this.Loaded += new RoutedEventHandler(SMSRecordList_Loaded);
   
        }

        void SMSRecordList_Loaded(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = AppGlobal.HasPermission() ? Visibility.Visible : Visibility.Collapsed;
            btnDelete.Visibility = AppGlobal.HasPermission() ? Visibility.Visible : Visibility.Collapsed;
            int officerId = 0;

            DateTime? start = null;
            DateTime? end = null;
            if (this.NavigationContext != null)
            {
                if (this.NavigationContext.QueryString.Keys.Contains("OfficerId"))
                    int.TryParse(this.NavigationContext.QueryString["OfficerId"], out officerId);
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
                mainGrid.RowDefinitions[0].Height = new GridLength(0);
                mainGrid.RowDefinitions[1].Height = new GridLength(0);
                mainGrid.RowDefinitions[2].Height = new GridLength(0);
            }

            if (start != null)
                dateStart.SelectedDate = start;
            if (end != null)
                dateEnd.SelectedDate = end;


            getData(officerId);
        }

        void ser_GetListByHQLCompleted(object sender, SMSRecordService.GetListByHQLCompletedEventArgs e)
        {
            try
            {
                int totalCount = 0;
                IList<SMSRecord> list = JsonSerializerHelper.JsonToEntities<SMSRecord>(e.Result, out totalCount);
                gv.ItemsSource = list;
            }
            catch (Exception ex)
            {
                Tools.ShowMessage(ex.Message, "", false);
            }
            finally
            {
                //buyRoot.IsBusy = false;
            }
        }

        void ser_DeleteByIdCompleted(object sender, SMSRecordService.DeleteByIdCompletedEventArgs e)
        {
            if (JsonSerializerHelper.JsonToEntity<bool>(e.Result))
            {
                Tools.ShowMessage("删除成功!", "", true);
                getData();
            }
        }

        void ser_GetListByHQLWithPagingCompleted(object sender, SMSRecordService.GetListByHQLWithPagingCompletedEventArgs e)
        {
            try
            {
                int total = 0;
                IList<SMSRecord> list = JsonSerializerHelper.JsonToEntities<SMSRecord>(e.Result, out total);

                foreach (var item in list)
                {
                    if (item.IsResponse)
                        item.CheckImage = new Uri(@"/Images/check.png", UriKind.Relative);
                }

                gv.ItemsSource = list;

                rDataPager1.ItemCount = total;
                Tools.ShowMask(false);
                if (list == null || list.Count == 0)
                {
                    Tools.ShowMessage("没有找到相对应的数据！","",true);
                }
            }
            catch (Exception ex)
            {
                Tools.ShowMessage(ex.Message, "", false);//这里会出错，应该是分页控件的问题
            }
            finally
            {
                //buyRoot.IsBusy = false;
            }
          
        }

        void getData()
        {
            getData(0);
        }

        void getData(int offcerId)
        {
            Tools.ShowMask(true, "正在查找数据,请稍等...");
            BuildHql(offcerId);

            queryCondition.FirstResult = rDataPager1.PageIndex * PageSize; ;

            QueryPaging(queryCondition);
        }

        private void BuildHql(int offcerId = 0)
        {
            string hqlStr = generateBaseHql(offcerId);
            
            queryCondition = new QueryCondition();

            queryCondition.HQL = "select r " + hqlStr + " order by r.WorkDate desc";

            queryCondition.TotalHQL = "select count(r) " + hqlStr;

            queryCondition.MaxResults = PageSize;

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            initialQuery();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SMSRecordForm frm = new SMSRecordForm();
            rDataPager1.PageIndex = 0;
            frm.SaveCallBack = getData;

            Tools.OpenWindow("群众办事登记-新增", frm, null, 600, 400);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SMSRecord obj = gv.SelectedItem as SMSRecord;

            if (obj != null)
            {

                Tools.OpenConfirm(string.Format("是否要删除?"),
                    new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>
                        (
                        (c, args) =>
                        {
                            if (args.DialogResult == true)
                            {
                                ser.DeleteByIdAsync(obj.Id);
                            }
                        }
                        ));
            }
        }
          
        private void rDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            Tools.ShowMask(true);
            if (queryCondition != null)
            {
                queryCondition.FirstResult = e.NewPageIndex * PageSize;
                //buyRoot.IsBusy = true;
                QueryPaging(queryCondition);
            }
        }

        private void QueryPaging(QueryCondition qc)
        {
            if (qc != null)
            {
                string condition = Newtonsoft.Json.JsonConvert.SerializeObject(qc);

                //buyRoot.IsBusy = true;
                ser.GetListByHQLWithPagingAsync(condition);
            }
        }



        public void OnCellDoubleClick(object sender, RadRoutedEventArgs e)
        {
            edit();
        }

        private void edit()
        {
            SMSRecord obj = gv.SelectedItem as SMSRecord;

            if (obj != null)
            {
                if (obj.GradeType.IsSupervise)
                {
                    SupervisionForm form = new SupervisionForm(obj);
                    form.SaveCallBack = getData;

                    Tools.OpenWindow("督查情况", form, null, 400, 320);
                }
                else
                {
                    SMSRecordForm form = new SMSRecordForm(obj);
                    form.SaveCallBack = getData;

                    Tools.OpenWindow("群众办事登记", form, null, 600, 400);
                }
            }
        }

        private void conditionType_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (conditionType != null)
            {
                if (conditionType.Text == "时间")
                {
                    datePanel.Visibility = Visibility.Visible;
                    tb.Visibility = Visibility.Collapsed;
                }
                else
                {
                    datePanel.Visibility = Visibility.Collapsed;
                    tb.Visibility = Visibility.Visible;
                }
            }
        }


        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string bastHql = generateBaseHql();
            string hql = "select r " + bastHql + " order by r.WorkDate desc";
            ser.ExportAsync(hql);
        }

        

        void ser_ExportCompleted(object sender, SMSRecordService.ExportCompletedEventArgs e)
        {
            try
            {
                if (e.Result.StartsWith("错误"))
                    throw new Exception(e.Result);
                string url = e.Result;
                if (!string.IsNullOrEmpty(url))
                    HtmlPage.Window.Eval("window.open('" + url + "','', 'top=" + (Application.Current.Host.Content.ActualHeight - 100) / 2 + ",left=" + (Application.Current.Host.Content.ActualWidth - 100) / 2
                          + ",fullscreen=0, width=100, height=100,  toolbar=0,   location=0,   directories=0,   status=0,   menubar=0,   scrollbars=0,   resizable=0')");
            }
            catch (Exception ex)
            {
                Tools.ShowMessage(ex.Message, "", true);
            }
            finally
            {
                //buyRoot.IsBusy = false;
            }
        }

        private string generateBaseHql(int offcerId = 0)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append(string.Format(" from SMSRecord as r where 1=1 "));

            if (offcerId != 0)
            {
                //跳转到个人录入记录时使用
                hql.Append(string.Format(" and r.WorkOfficer.Id = {0} ", offcerId));
            }
            else
            {
                if (conditionType.Text == "时间")
                {
                    if (dateStart.SelectedDate != null)
                    {
                        DateTime tmp = dateStart.SelectedDate.Value;
                        DateTime start = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0);
                        hql.Append(string.Format(" and r.WorkDate >= '{0}' ", start));
                    }
                    if (dateEnd.SelectedDate != null)
                    {
                        DateTime tmp = dateEnd.SelectedDate.Value;
                        DateTime end = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
                        hql.Append(string.Format(" and r.WorkDate <= '{0}' ", end));
                    }
                }
                if (conditionType.Text == "电话")
                {
                    if (!string.IsNullOrEmpty(tb.Text.Trim()))
                        hql.Append(string.Format(" and r.PersonMobile like '%{0}%' ", tb.Text.Trim()));
                }
                if (conditionType.Text == "受理人")
                {
                    if (!string.IsNullOrEmpty(tb.Text.Trim()))
                        hql.Append(string.Format(" and r.WorkOfficer.Name like '%{0}%' ", tb.Text.Trim()));
                }
                if (conditionType.Text == "办案人")
                {
                    if (!string.IsNullOrEmpty(tb.Text.Trim()))
                        hql.Append(string.Format(" and r.PersonName like '%{0}%' ", tb.Text.Trim()));
                }
                if (conditionType.Text == "值班领导")
                {
                    if (!string.IsNullOrEmpty(tb.Text.Trim()))
                        hql.Append(string.Format(" and r.Leader.Name like '%{0}%' ", tb.Text.Trim()));
                }
            }
            string hqlStr = hql.ToString();
            return hqlStr;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            edit();
        }

        private void condition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                initialQuery();
            }
        }

        /// <summary>
        /// 初始化查询
        /// </summary>
        private void initialQuery()
        {
            rDataPager1.PageIndex = 0;
            getData();
        }

    }
}
