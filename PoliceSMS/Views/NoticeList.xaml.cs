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
using PoliceSMS.Lib.SMS;
using PoliceSMS.ViewModel;
using Telerik.Windows;
using Telerik.Windows.Controls.GridView;
using System.Windows.Browser;
using System.Text;
using PoliceSMS.Lib.Query;

namespace PoliceSMS.Views
{
    public partial class NoticeList : Page
    {
        NoticeService.NoticeServiceClient ser = new NoticeService.NoticeServiceClient();
        private const int PageSize = 19;
        private QueryCondition queryCondition = null;

        public NoticeList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NoticeList_Loaded);
            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void NoticeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppGlobal.HasPermission())
                btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = Visibility.Visible;
            else
                btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = Visibility.Collapsed;
            ser.GetListByHQLWithPagingCompleted+=new EventHandler<NoticeService.GetListByHQLWithPagingCompletedEventArgs>(ser_GetListByHQLWithPagingCompleted); 
            
            ser.DeleteByIdCompleted += new EventHandler<NoticeService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);

            getData();
        }

        void ser_GetListByHQLWithPagingCompleted(object sender, NoticeService.GetListByHQLWithPagingCompletedEventArgs e)
        {
            try
            {
                int total = 0;
                IList<Notice> list = JsonSerializerHelper.JsonToEntities<Notice>(e.Result, out total);
                
                gv.ItemsSource = list;

                rDataPager1.ItemCount = total;
                Tools.ShowMask(false);

                // 增加
                if (list == null || list.Count == 0)
                {
                    Tools.ShowMessage("没有找到相对应的数据！", "", true);
                }

                // 结束


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


        void ser_DeleteByIdCompleted(object sender, NoticeService.DeleteByIdCompletedEventArgs e)
        {
            if (JsonSerializerHelper.JsonToEntity<bool>(e.Result))
            {
                Tools.ShowMessage("删除成功!", "", true);
                getData();
            }
        }

        void getData()
        {
            Tools.ShowMask(true);
            BuildHql();

            queryCondition.FirstResult = rDataPager1.PageIndex * PageSize; ;

            QueryPaging(queryCondition);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            getData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NoticeForm frm = new NoticeForm(getData);

            Tools.OpenWindow("系统通告-新增", frm, null);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Notice obj = gv.SelectedItem as Notice;
            if (obj != null)
            {
                NoticeForm frm = new NoticeForm(obj, getData);

                Tools.OpenWindow("系统通告-编辑", frm, null);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Notice obj = gv.SelectedItem as Notice;

            if (obj != null)
            {

                Tools.OpenConfirm(string.Format("是否要删除\"{0}\"", obj.Title),
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
        public void OnCellDoubleClick(object sender, RadRoutedEventArgs e)
        {
            Notice obj = gv.SelectedItem as Notice;
            if (obj != null)
            {
                NoticeForm frm = new NoticeForm(obj, getData);

                Tools.OpenWindow("系统通告-编辑", frm, null);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btn = sender as HyperlinkButton;
            if (btn != null)
            {
                var url = btn.Tag.ToString();
                if (!url.Trim().StartsWith("http://"))
                    url = "http://" + url;
                HtmlPage.Window.Eval(string.Format("window.open('{0}')",url));
            }
        }

        private void BuildHql()
        {
            StringBuilder hql = new StringBuilder();
            hql.Append(string.Format(" from Notice as r "));

           

            queryCondition = new QueryCondition();

            string hqlStr = hql.ToString();
            queryCondition.HQL = "select r " + hqlStr + " order by Id desc";

            queryCondition.TotalHQL = "select count(r) " + hqlStr;

            queryCondition.MaxResults = PageSize;

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
    }
}
