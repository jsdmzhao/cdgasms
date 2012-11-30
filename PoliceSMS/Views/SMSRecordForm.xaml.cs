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
using PoliceSMS.Lib.Base;
using PoliceSMS.Comm;
using System.ServiceModel;
using PoliceSMS.Lib.SMS;
using PoliceSMS.Lib.Organization;
using Telerik.Windows.Controls;
using System.Text.RegularExpressions;

namespace PoliceSMS.Views
{
    public partial class SMSRecordForm : UserControl
    {
        private SMSRecord smsRecord = null;

        private IList<Sex> sexs;

        private IList<GradeType> gradeTypes;

        private IList<WorkType> workTypes;

        private IList<OfficerType> officerTypes;

        private IList<Officer> officers;

        public Action SaveCallBack { get; set; }

        public SMSRecordForm()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SMSRecordForm_Loaded);
            smsRecord = new SMSRecord();
            DataContext = smsRecord;
            
        }

        bool isEdit = false;
        SMSRecord editObj;
        public SMSRecordForm(SMSRecord editObj)
        {
            InitializeComponent();
            isEdit = true;
            this.editObj = editObj;
            smsRecord = editObj;
            Loaded += new RoutedEventHandler(SMSRecordForm_Loaded);
            SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();
            ser.GetByIdCompleted +=
            (object sender, SMSRecordService.GetByIdCompletedEventArgs e) =>
            {
                SMSRecord obj = JsonSerializerHelper.JsonToEntity<SMSRecord>(e.Result);
                this.smsRecord = obj;
                DataContext = smsRecord;
                chkIsResponse.IsChecked = smsRecord.IsResponse ;
                
            };
            this.IsEnabled = AppGlobal.HasPermission();
            //查询一次，避免在form更改了数据点击取消后原list界面数据更改（界面双向绑定，但没有提交到数据库）
            ser.GetByIdAsync(editObj.Id);
        }

        void SMSRecordForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSexs();
            LoadGradeTypes();
            LoadWorkTypes();
            LoadOfficerTypes();
            LoadOfficers();

            this.cmbWorkOrg.Text = (AppGlobal.CurrentUser.Organization).Name;


            bool hasPsermission = AppGlobal.HasPermission();
            this.cmbGradeType.IsReadOnly = !hasPsermission;
            this.cmbGradeType.IsEnabled = hasPsermission;


        }

        private void LoadSexs()
        {
            try
            {
                SexService.SexServiceClient ser = new SexService.SexServiceClient();
                ser.GetListByHQLCompleted += (object sender, SexService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    sexs = JsonSerializerHelper.JsonToEntities<Sex>(e.Result, out total);

                    cmbSex.ItemsSource = sexs;

                };

                ser.GetListByHQLAsync("from Sex");

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取性别发生错误", ex.Message, false);
            }
        }

        private void LoadGradeTypes()
        {
            try
            {
                //GradeTypeService.GradeTypeServiceClient ser = new GradeTypeService.GradeTypeServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../SMSWcf/GradeTypeService.svcc")));
                GradeTypeService.GradeTypeServiceClient ser = new GradeTypeService.GradeTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, GradeTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    gradeTypes = JsonSerializerHelper.JsonToEntities<GradeType>(e.Result, out total);

                    cmbGradeType.ItemsSource = gradeTypes;

                };
                ser.GetListByHQLAsync("from GradeType where IsSupervise = " + false + " and IsUsed = " + true);

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
                //WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../SMSWcf/WorkTypeService.svc")));
                WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, WorkTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    workTypes = JsonSerializerHelper.JsonToEntities<WorkType>(e.Result, out total);

                    cmbWorkType.ItemsSource = workTypes;

                };

                ser.GetListByHQLAsync("from WorkType where IsUsed = " + true);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取办事类别发生错误", ex.Message, false);
            }
        }

        private void LoadOfficerTypes()
        {
            try
            {
                //WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../SMSWcf/WorkTypeService.svc")));
                OfficerTypeService.OfficerTypeServiceClient ser = new OfficerTypeService.OfficerTypeServiceClient();
                ser.GetListByHQLCompleted += (object sender, OfficerTypeService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    officerTypes = JsonSerializerHelper.JsonToEntities<OfficerType>(e.Result, out total);

                    cmbOfficerType.ItemsSource = officerTypes;

                };

                ser.GetListByHQLAsync("from OfficerType");

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取警种发生错误", ex.Message, false);
            }
        }

        private void LoadOfficers()
        {
            try
            {
                //WorkTypeService.WorkTypeServiceClient ser = new WorkTypeService.WorkTypeServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../SMSWcf/WorkTypeService.svc")));
                OfficerService.OfficerServiceClient ser = new OfficerService.OfficerServiceClient();
                ser.GetListByHQLCompleted += (object sender, OfficerService.GetListByHQLCompletedEventArgs e) =>
                {
                    int total = 0;
                    officers = JsonSerializerHelper.JsonToEntities<Officer>(e.Result, out total);
                    var removeList = officers.Where(c => c.Name == "吴涛" || c.Name == "贾红兵").ToList();
                    for (int i = 0; i < removeList.Count; i++)
                        officers.Remove(removeList[i]);
                    cmbWorkOfficer.ItemsSource = cmbLeader.ItemsSource = officers;

                };

                int orgId = isEdit ? smsRecord.Organization.Id : AppGlobal.CurrentUser.Organization.Id;
                ser.GetListByHQLAsync(string.Format("from Officer as e where e.Organization.id = {0} order by e.Name", orgId));

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取受理人发生错误", ex.Message, false);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckVerify())
            {
                //是否为防范科
                bool flag = AppGlobal.CurrentUser.Organization.Name.Contains("防范");
                if (string.IsNullOrEmpty(smsRecord.PersonName))
                {
                    Tools.ShowMessage("请输入姓名!", "", false);
                    return;
                }
                string s = @"^(1)\d{10}$";
                //if (!string.IsNullOrEmpty(smsRecord.PersonMobile) && !Regex.IsMatch(smsRecord.PersonMobile, s))
                if (!string.IsNullOrEmpty(smsRecord.PersonMobile))
                {
                    //只能为数字
                    foreach (var c in smsRecord.PersonMobile)
                    {
                        if (!char.IsDigit(c))
                        {
                            Tools.ShowMessage("电话号码格式错误!", "", false);
                            return;
                        }
                    }

                    //不是手机号改为已发送
                    if (!Regex.IsMatch(smsRecord.PersonMobile, s))
                        smsRecord.IsSend = true;
                    //如果将座机改为手机 将记录修改为 未发送
                    if (isEdit)
                    {
                        if (Regex.IsMatch(smsRecord.PersonMobile, s) && smsRecord.PersonMobile != editObj.PersonMobile)
                            smsRecord.IsSend = false;
                    }

                }
                if (!flag && cmbWorkType.SelectedItem == null)
                {
                    Tools.ShowMessage("请输入办事类别!", "", false);
                    return;
                }
                if (!flag && string.IsNullOrEmpty(smsRecord.WorkNo))
                {
                    Tools.ShowMessage("请输入流水号!", "", false);
                    return;
                }
                if (!flag && cmbLeader.SelectedItem == null)
                {
                    Tools.ShowMessage("请输入值班领导!", "", false);
                    return;
                }
                if (cmbWorkOfficer.SelectedItem == null)
                {
                    Tools.ShowMessage("请输入受理人!", "", false);
                    return;
                }

                if (cmbOfficerType.SelectedItem == null)
                {
                    Tools.ShowMessage("请输入警种!", "", false);
                    return;
                }
                SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();

                ser.SaveOrUpdateCompleted += (object sender1, SMSRecordService.SaveOrUpdateCompletedEventArgs e1) =>
                    {
                        int id = JsonSerializerHelper.JsonToEntity<int>(e1.Result);
                        if (id > 0)
                        {
                            smsRecord.Id = id;
                            if (SaveCallBack != null)
                                SaveCallBack();
                        }
                    };
                if (smsRecord.Id == 0)
                {
                    smsRecord.LoginOfficer = AppGlobal.CurrentUser;
                    smsRecord.Organization = AppGlobal.CurrentUser.Organization;
                    smsRecord.GradeType = new GradeType() { Id = 3 };

                    if (string.IsNullOrEmpty(smsRecord.PersonMobile))
                        smsRecord.PersonMobile = ""; //数据库不能为空
                    if (smsRecord.WorkType == null)
                        smsRecord.WorkType = new WorkType { Id = 4 }; //数据库不能为空
                    if (smsRecord.Leader == null)
                        smsRecord.Leader = smsRecord.WorkOfficer; //数据库不能为空
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(smsRecord);
                ser.SaveOrUpdateAsync(json);
                (this.Parent as RadWindow).Close();
            }
            else
            {
                Tools.ShowMessage("没有权限", "", false);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as RadWindow).Close();
        }

        private bool CheckVerify()
        {
            return true;
        }
    }
}
