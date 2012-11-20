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
using Telerik.Windows.Controls;
using PoliceSMS.Lib.SMS;
using PoliceSMS.ViewModel;
using PoliceSMS.Comm;

namespace PoliceSMS.Views
{
    public partial class NoticeForm : UserControl
    {
        Notice obj = new Notice();

        Action refreshAction;

        public NoticeForm(Action refreshAction)
        {
            InitializeComponent();
            this.refreshAction = refreshAction;
            this.Loaded += new RoutedEventHandler(NoticeForm_Loaded);
        }

        public NoticeForm(Notice editObj, Action refreshAction)
            : this(refreshAction)
        {
            //复制可能被改变的属性，避免退出修改的刷新
            obj.Id = editObj.Id;
            obj.Title = editObj.Title;
            obj.Url = editObj.Url;

            this.IsEnabled = AppGlobal.HasPermission();
        }

        void NoticeForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = obj;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(obj.Title))
            {
                Tools.ShowMessage("请输入标题!", "", false);
                return;
            }
            if (string.IsNullOrEmpty(obj.Url))
            {
                Tools.ShowMessage("请输入地址!", "", false);
                return;
            }
            Tools.ShowMask(true, "正在保存数据");
            NoticeService.NoticeServiceClient ser = new NoticeService.NoticeServiceClient();
            ser.SaveOrUpdateCompleted += new EventHandler<NoticeService.SaveOrUpdateCompletedEventArgs>(ser_SaveOrUpdateCompleted);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            ser.SaveOrUpdateAsync(json);
        }

        void ser_SaveOrUpdateCompleted(object sender, NoticeService.SaveOrUpdateCompletedEventArgs e)
        {
            Tools.ShowMask(false);
            (this.Parent as RadWindow).Close();
            if (refreshAction != null)
                refreshAction();
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as RadWindow).Close();
        }
    }
}
