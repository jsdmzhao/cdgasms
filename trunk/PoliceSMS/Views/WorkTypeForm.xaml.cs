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
    public partial class WorkTypeForm : UserControl
    {
        WorkType obj = new WorkType();

        Action refreshAction;

        public WorkTypeForm(Action refreshAction)
        {
            InitializeComponent();
            this.refreshAction = refreshAction;
            this.Loaded += new RoutedEventHandler(WorkTypeFormForm_Loaded);
        }

        public WorkTypeForm(WorkType editObj, Action refreshAction)
            : this(refreshAction)
        {
            //复制可能被改变的属性，避免退出修改的刷新
            obj.Id = editObj.Id;
            obj.Name = editObj.Name;
            obj.IsUsed = editObj.IsUsed;

            this.IsEnabled = AppGlobal.HasPermission();
        }

        void WorkTypeFormForm_Loaded(object sender, RoutedEventArgs e)
        {
            comboUsed.ItemsSource = UsedState.CreateAry();
            this.DataContext = obj;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Tools.ShowMask(true, "正在保存数据");
            WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient();
            ser.SaveOrUpdateCompleted += new EventHandler<WorkTypeService.SaveOrUpdateCompletedEventArgs>(ser_SaveOrUpdateCompleted);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            ser.SaveOrUpdateAsync(json);
        }

        void ser_SaveOrUpdateCompleted(object sender, WorkTypeService.SaveOrUpdateCompletedEventArgs e)
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
