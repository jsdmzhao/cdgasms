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
using PoliceSMS.Comm;
using PoliceSMS.Lib.SMS;
using PoliceSMS.Lib.Query;
using System.Text;
using PoliceSMS.Lib.Organization;
using PoliceSMS.ViewModel;

namespace PoliceSMS.Views
{
    public partial class SMSRecordList : Page
    {
        SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();
        private const int PageSize = 19;
        private QueryCondition queryCondition = null;

        public SMSRecordList()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(SMSRecordList_Loaded);
        }

        void SMSRecordList_Loaded(object sender, RoutedEventArgs e)
        {
            Tools.ShowMask(true);
            ser.GetListByHQLCompleted += new EventHandler<SMSRecordService.GetListByHQLCompletedEventArgs>(ser_GetListByHQLCompleted);
            ser.DeleteByIdCompleted += new EventHandler<SMSRecordService.DeleteByIdCompletedEventArgs>(ser_DeleteByIdCompleted);
            ser.GetListByHQLWithPagingCompleted += new EventHandler<SMSRecordService.GetListByHQLWithPagingCompletedEventArgs>(ser_GetListByHQLWithPagingCompleted);

            LoadWorkTypes();
            LoadOfficers();
            LoadStation();
            LoadGradeType();

            cboxMark.ItemsSource = MarkState.CreateAry();

            dateStart.SelectedDate = DateTime.Now.AddDays(-7);
            dateEnd.SelectedDate = DateTime.Now; ;
            getData();
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

                ser.GetListByHQLAsync("from GradeType where IsUsed = " + true);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取评分类别发生错误", ex.Message, false);
            }
        }

        private void LoadWorkTypes()
        {
            try
            {
               WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, WorkTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var list = JsonSerializerHelper.JsonToEntities<WorkType>(e.Result, out total);

                    cboxContent.ItemsSource = list;

                };

                ser.GetListByHQLAsync("from WorkType where IsUsed = " + true);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取办事类别发生错误", ex.Message, false);
            }
        }

        private void LoadOfficers()
        {
            try
            {
                OfficerService.OfficerServiceClient ser = new OfficerService.OfficerServiceClient();
                ser.GetListByHQLCompleted += (object sender, OfficerService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    var list = JsonSerializerHelper.JsonToEntities<Officer>(e.Result, out total);

                    cboxOper.ItemsSource = list;

                };

                ser.GetListByHQLAsync("from Officer as e where e.Organization.id =" + AppGlobal.CurrentUser.Organization.Id);

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
                ser.GetListByHQLAsync("from Organization where Name like '%青羊%'");

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

                gv.ItemsSource = list;

                rDataPager1.ItemCount = total;
                Tools.ShowMask(false);
            }
            catch (Exception ex)
            {
                Tools.ShowMessage(ex.Message, "", false);//这里会出错，应该是分页控件的问题
            }
          
        }

        void getData()
        {
            Tools.ShowMask(true);
            BuildHql();

            queryCondition.FirstResult = 0;

            QueryPaging(queryCondition);
        }

        private void BuildHql()
        {
            StringBuilder hql = new StringBuilder();

            DateTime tmp = dateStart.SelectedDate.Value;
            DateTime start = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0);
            tmp = dateEnd.SelectedDate.Value;
            DateTime end = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
            
            hql.Append(string.Format(" from SMSRecord as r where r.WorkDate between '{0}' and '{1}' ",
                 start,end));

            if (cboxOper.SelectedItem != null)
            {
                Officer off = cboxOper.SelectedItem as Officer;
                if(off != null)
                    hql.Append(" and r.WorkOfficer.Id=" + off.Id);
            }

            if (cboxMark.SelectedItem != null)
            {
                MarkState mark = cboxMark.SelectedItem as MarkState;
                if(mark != null)
                 hql.Append(" and r.IsResponse =" + mark.IsMark);
            }

            if (cboxContent.SelectedItem != null)
            {
                WorkType wt = cboxContent.SelectedItem as WorkType;
                if (wt != null)
                    hql.Append(" and r.WorkType.Id =" + wt.Id);
            }

            if (cboxStation.SelectedItem!=null)
            {
                Organization org = cboxStation.SelectedItem as Organization;
                if (org != null)
                    hql.Append(" and r.Organization.Id =" + org.Id);
            }

            if ( cboxGradeType.SelectedItem != null)
            {
                GradeType grade = cboxGradeType.SelectedItem as GradeType;
                if (grade != null)
                    hql.Append(" and r.GradeType.Id =" + grade.Id);
            }

            if (!string.IsNullOrEmpty(cboxName.Text))
            {
                hql.Append(" and r.PersonName like '%" + cboxName.Text + "%'");
            }

            queryCondition = new QueryCondition();

            string hqlStr = hql.ToString();
            queryCondition.HQL = "select r " + hqlStr;

            queryCondition.TotalHQL = "select count(r) " + hqlStr;

            queryCondition.MaxResults = PageSize;

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            getData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SMSRecordForm frm = new SMSRecordForm();
            frm.SaveCallBack = getData;

            Tools.OpenWindow("群众办事登记-新增", frm, null, 600, 375);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SMSRecord obj = gv.SelectedItem as SMSRecord;
            if (obj != null)
            {
                SMSRecordForm frm = new SMSRecordForm(obj);
                frm.SaveCallBack = getData;
                Tools.OpenWindow("群众办事登记-编辑", frm, null, 600, 375);
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

                QueryPaging(queryCondition);
            }
        }

        private void QueryPaging(QueryCondition qc)
        {
            if (qc != null)
            {
                string condition = Newtonsoft.Json.JsonConvert.SerializeObject(qc);

                ser.GetListByHQLWithPagingAsync(condition);
            }
        }


    }
}