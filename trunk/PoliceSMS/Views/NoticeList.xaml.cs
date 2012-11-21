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

namespace PoliceSMS.Views
{
    public partial class NoticeList : Page
    {
        NoticeService.NoticeServiceClient ser = new NoticeService.NoticeServiceClient();
        public NoticeList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NoticeList_Loaded);
            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        void NoticeList_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppGlobal.HasPermission())
                btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = Visibility.Visible;
            else
                btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = Visibility.Collapsed;
            ser.GetListByHQLCompleted += new EventHandler<NoticeService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<NoticeService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);

            getData();
        }


        void ser_GetListByHQLCompleted(object sender, NoticeService.GetListByHQLCompletedEventArgs e)
        {
            try
            {
                int totalCount = 0;
                IList<Notice> list = JsonSerializerHelper.JsonToEntities<Notice>(e.Result, out totalCount);
                foreach (var item in list)
                {
                    item.Description = UsedState.CreateAry()[Convert.ToInt32(item.IsUsed)].Name;
                }

                gv.ItemsSource = list;
                if (list == null || list.Count == 0)
                {
                    Tools.ShowMessage("没有系统通告!","",true);
                }

            }
            catch (Exception ex)
            {
                Tools.ShowMessage(ex.Message, "", false);
            }
            finally
            {
                Tools.ShowMask(false);
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
            ser.GetListByHQLAsync("from Notice");
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
    }
}
