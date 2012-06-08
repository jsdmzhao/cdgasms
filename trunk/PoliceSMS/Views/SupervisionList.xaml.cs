﻿using System;
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
using PoliceSMS.Lib.SMS;
using PoliceSMS.Comm;
using Telerik.Windows;
using PoliceSMS.ViewModel;
using Telerik.Windows.Controls.GridView;
using PoliceSMS.Lib.Organization;
using System.Text;

namespace PoliceSMS.Views
{
    public partial class SupervisionList : Page
    {
        SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();
        private const int PageSize = 19;
        private QueryCondition queryCondition = null;

        public SupervisionList()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SupervisionList_Loaded);

            ser.GetListByHQLCompleted += new EventHandler<SMSRecordService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<SMSRecordService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);
            ser.GetListByHQLWithPagingCompleted += new EventHandler<SMSRecordService.GetListByHQLWithPagingCompletedEventArgs>(ser_GetListByHQLWithPagingCompleted);

            LoadOfficers();
            LoadStation();
            LoadGradeType();

            dateStart.SelectedDate = DateTime.Now.AddDays(-7);
            dateEnd.SelectedDate = DateTime.Now;

            getData();
            this.gv.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        
        }

        void SupervisionList_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void LoadGradeType()
        {
            try
            {
                GradeTypeService.GradeTypeServiceClient ser = new GradeTypeService.GradeTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, GradeTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var list = JsonSerializerHelper.JsonToEntities<GradeType>(e.Result, out total);

                    cboxGradeType.ItemsSource = list;

                };

                ser.GetListByHQLAsync("from GradeType where Id > 5 and IsUsed = " + true);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取评分类别发生错误", ex.Message, false);
            }
        }

        private void LoadOfficers()
        {
            try
            {
                if (cboxStation.SelectedItem != null)
                {
                    OfficerService.OfficerServiceClient ser = new OfficerService.OfficerServiceClient();
                    ser.GetListByHQLCompleted += (object sender, OfficerService.GetListByHQLCompletedEventArgs e) =>
                    {
                        int total = 0;
                        var list = JsonSerializerHelper.JsonToEntities<Officer>(e.Result, out total);
                        var removeList = list.Where(c => c.Name == "吴涛" || c.Name == "贾红兵").ToList();
                        for (int i = 0; i < removeList.Count; i++)
                            list.Remove(removeList[i]);
                        cboxOper.ItemsSource = list;

                    };

                    ser.GetListByHQLAsync(string.Format("from Officer as e where e.Organization.id = {0} order by e.Name", (cboxStation.SelectedItem as Organization).Id));
                }

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取民警发生错误", ex.Message, false);
            }
        }

        private void LoadStation()
        {
            try
            {
                OrganizationService.OrganizationServiceClient ser = new OrganizationService.OrganizationServiceClient();
                ser.GetListByHQLCompleted += (object sender, OrganizationService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var list = JsonSerializerHelper.JsonToEntities<Organization>(e.Result, out total);

                    cboxStation.ItemsSource = list;

                };

                //这里没有考虑权限
                ser.GetListByHQLAsync("from Organization where Name like '%青羊%' order by OrderIndex ");

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取单位发生错误", ex.Message, false);
            }
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

            DateTime tmp = dateStart.SelectedDate.Value;
            DateTime start = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0);
            tmp = dateEnd.SelectedDate.Value;
            DateTime end = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);

            hql.Append(string.Format(" from SMSRecord as r where r.GradeType.IsSupervise = {0} and  r.WorkDate between '{1}' and '{2}' ",
                 true,start, end));

            if (cboxOper.SelectedItem != null)
            {
                Officer off = cboxOper.SelectedItem as Officer;
                if (off != null)
                    hql.Append(" and r.WorkOfficer.Id=" + off.Id);
            }

          

            if (cboxStation.SelectedItem != null)
            {
                Organization org = cboxStation.SelectedItem as Organization;
                if (org != null)
                    hql.Append(" and r.Organization.Id =" + org.Id);
            }

            if (cboxGradeType.SelectedItem != null)
            {
                GradeType grade = cboxGradeType.SelectedItem as GradeType;
                if (grade != null)
                    hql.Append(" and r.GradeType.Id =" + grade.Id);
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
            SupervisionForm frm = new SupervisionForm();
            rDataPager1.PageIndex = 0;
            frm.SaveCallBack = getData;

            Tools.OpenWindow("督查情况-新增", frm, null, 400, 260);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SMSRecord obj = gv.SelectedItem as SMSRecord;
            if (obj != null)
            {
                SupervisionForm frm = new SupervisionForm(obj);
                frm.SaveCallBack = getData;
                Tools.OpenWindow("督查情况-编辑", frm, null, 400, 260);
            }
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
                SupervisionForm form = new SupervisionForm(obj);

                form.IsEnabled = false;

                //允许政治处修改数据
                if ((AppGlobal.CurrentUser.Organization).Name.Contains("政治处") || (AppGlobal.CurrentUser.Organization).Name.Contains("成都市公安局青羊区分局"))
                    form.IsEnabled = true;

                Tools.OpenWindow("督查情况", form, null, 400, 260);
            }
        }

        private void cboxStation_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadOfficers();
        }
    }
}
