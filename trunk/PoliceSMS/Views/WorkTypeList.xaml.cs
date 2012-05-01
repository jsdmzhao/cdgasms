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

namespace PoliceSMS.Views
{
    public partial class WorkTypeList : Page
    {
        WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient();
        public WorkTypeList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WorkTypeList_Loaded);
        }

        void WorkTypeList_Loaded(object sender, RoutedEventArgs e)
        {
            ser.GetListByHQLCompleted += new EventHandler<WorkTypeService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<WorkTypeService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);

            getData();
        }


        void ser_GetListByHQLCompleted(object sender, WorkTypeService.GetListByHQLCompletedEventArgs e)
        {
            try
            {
                int totalCount = 0;
                IList<WorkType> list = JsonSerializerHelper.JsonToEntities<WorkType>(e.Result, out totalCount);
                foreach (var item in list)
                {
                    item.Description = UsedState.CreateAry()[Convert.ToInt32(item.IsUsed)].Name;
                }

                gv.ItemsSource = list;

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
        void ser_DeleteByIdCompleted(object sender, WorkTypeService.DeleteByIdCompletedEventArgs e)
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
            ser.GetListByHQLAsync("from WorkType");
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            getData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            WorkTypeForm frm = new WorkTypeForm(getData);

            Tools.OpenWindow("办事内容-新增", frm, null);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            WorkType obj = gv.SelectedItem as WorkType;
            if (obj != null)
            {
                WorkTypeForm frm = new WorkTypeForm(obj, getData);

                Tools.OpenWindow("办事内容-编辑", frm, null);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WorkType obj = gv.SelectedItem as WorkType;

            if (obj != null)
            {

                Tools.OpenConfirm(string.Format("是否要删除\"{0}\"", obj.Name),
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

    }
}
