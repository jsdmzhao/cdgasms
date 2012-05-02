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
using PoliceSMS.Comm;
using System.Collections;
using PoliceSMS.Lib.SMS;
using PoliceSMS.ViewModel;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;

namespace PoliceSMS.Views
{
    public partial class GradeTypeList : Page
    {
        GradeTypeService.GradeTypeServiceClient ser = new GradeTypeService.GradeTypeServiceClient();
        public GradeTypeList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GradeTypeList_Loaded);
            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        void GradeTypeList_Loaded(object sender, RoutedEventArgs e)
        {
            ser.GetListByHQLCompleted += new EventHandler<GradeTypeService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<GradeTypeService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);

            getData();
        }

        void ser_GetListByHQLCompleted(object sender, GradeTypeService.GetListByHQLCompletedEventArgs e)
        {
            try
            {
                int totalCount = 0;
                IList<GradeType> list = JsonSerializerHelper.JsonToEntities<GradeType>(e.Result, out totalCount);
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

        void ser_DeleteByIdCompleted(object sender, GradeTypeService.DeleteByIdCompletedEventArgs e)
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
            ser.GetListByHQLAsync("from GradeType");
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            getData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GradeTypeForm frm = new GradeTypeForm(getData);

            Tools.OpenWindow("评分内容-新增", frm, null);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            GradeType obj = gv.SelectedItem as GradeType;
            if (obj != null)
            {
                GradeTypeForm frm = new GradeTypeForm(obj,getData);

                Tools.OpenWindow("评分内容-编辑", frm, null);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            GradeType obj = gv.SelectedItem as GradeType;

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

        public void OnCellDoubleClick(object sender, RadRoutedEventArgs e)
        {
            GradeType obj = gv.SelectedItem as GradeType;

            if (obj != null)
            {
                GradeTypeForm form = new GradeTypeForm(obj, getData);
                Tools.OpenWindow("评分内容-编辑", form, null);
            }
        }
    }
}
