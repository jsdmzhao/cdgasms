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
            this.Loaded += new RoutedEventHandler(SMSRecordList_Loaded);

            ser.GetListByHQLCompleted += new EventHandler<SMSRecordService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<SMSRecordService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);
            ser.GetListByHQLWithPagingCompleted += new EventHandler<SMSRecordService.GetListByHQLWithPagingCompletedEventArgs>(ser_GetListByHQLWithPagingCompleted);

            
            dateStart.SelectedDate = DateTime.Now.AddDays(-7);
            dateEnd.SelectedDate = DateTime.Now;

            getData();
            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        
        }

        void SMSRecordList_Loaded(object sender, RoutedEventArgs e)
        {
           
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
            Tools.ShowMask(true);
            BuildHql();

            queryCondition.FirstResult = rDataPager1.PageIndex * PageSize; ;

            QueryPaging(queryCondition);
        }

        private void BuildHql()
        {
            StringBuilder hql = new StringBuilder();
            hql.Append(string.Format(" from SMSRecord as r where 1=1 "));

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
                    hql.Append(string.Format(" and r.PersonMobile like '{0}%' ", tb.Text.Trim()));
            }
            if (conditionType.Text == "受理人")
            {
                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                    hql.Append(string.Format(" and r.WorkOfficer.Name like '{0}%' ", tb.Text.Trim()));
            }
            if (conditionType.Text == "办案人")
            {
                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                    hql.Append(string.Format(" and r.PersonName like '{0}%' ", tb.Text.Trim()));
            }
            if (conditionType.Text == "值班领导")
            {
                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                    hql.Append(string.Format(" and r.Leader.Name like '{0}%' ", tb.Text.Trim()));
            }
            

            queryCondition = new QueryCondition();

            string hqlStr = hql.ToString();
            queryCondition.HQL = "select r " + hqlStr + " order by r.WorkDate desc";

            queryCondition.TotalHQL = "select count(r) " + hqlStr;

            queryCondition.MaxResults = PageSize;

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            rDataPager1.PageIndex = 0;
            getData();
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
            SMSRecord obj = gv.SelectedItem as SMSRecord;

            if (obj != null)
            {
                if (obj.GradeType.IsSupervise)
                {
                    SupervisionForm form = new SupervisionForm(obj);
                    form.SaveCallBack = getData;
                    form.IsEnabled = false;

                    //允许政治处修改数据
                    if ((AppGlobal.CurrentUser.Organization).Name.Contains("政治处") || (AppGlobal.CurrentUser.Organization).Name.Contains("成都市公安局青羊区分局"))
                        form.IsEnabled = true;

                    Tools.OpenWindow("督查情况", form, null, 400, 290);
                }
                else
                {
                    SMSRecordForm form = new SMSRecordForm(obj);
                    form.SaveCallBack = getData;
                    form.IsEnabled = false;

                    //允许政治处修改数据
                    if ((AppGlobal.CurrentUser.Organization).Name.Contains("政治处") || (AppGlobal.CurrentUser.Organization).Name.Contains("成都市公安局青羊区分局"))
                        form.IsEnabled = true;

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

    }
}
