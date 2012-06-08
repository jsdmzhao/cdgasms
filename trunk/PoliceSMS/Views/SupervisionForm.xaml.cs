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
using PoliceSMS.Lib.SMS;
using PoliceSMS.Lib.Base;
using PoliceSMS.Lib.Organization;
using PoliceSMS.Comm;
using Telerik.Windows.Controls;

namespace PoliceSMS.Views
{
    public partial class SupervisionForm : UserControl
    {
        private SMSRecord smsRecord = null;

        private IList<GradeType> gradeTypes;

        private IList<Officer> officers;

        public Action SaveCallBack { get; set; }

        public SupervisionForm()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SupervisionForm_Loaded);
            smsRecord = new SMSRecord();
            DataContext = smsRecord;
            count.Value = 1;

        }

        bool isEdit = false;
        public SupervisionForm(SMSRecord editObj)
        {
            InitializeComponent();
            isEdit = true;
            smsRecord = editObj;
            count.Value = 1;
            count.IsReadOnly = true;
            Loaded += new RoutedEventHandler(SupervisionForm_Loaded);
            SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();
            ser.GetByIdCompleted +=
            (object sender, SMSRecordService.GetByIdCompletedEventArgs e) =>
            {
                SMSRecord obj = JsonSerializerHelper.JsonToEntity<SMSRecord>(e.Result);
                this.smsRecord = obj;
                DataContext = smsRecord;

            };
            ser.GetByIdAsync(editObj.Id);
        }

        void SupervisionForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGradeTypes();
            LoadStation();

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

                ser.GetListByHQLAsync("from GradeType where IsSupervise = "+ true+" and IsUsed = " + true);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取扣分原因发生错误", ex.Message, false);
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
                    cmbWorkOfficer.ItemsSource = officers;

                };

                int orgId = isEdit ? smsRecord.Organization.Id : (cboxStation.SelectedItem as Organization).Id;
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
                //新增和修改不同，这里分开写
                if (isEdit)
                    update();
                else
                    save();
            }
        }

        private void save()
        {
            if (cboxStation.SelectedItem == null)
            {
                Tools.ShowMessage("请输入受理单位!", "", false);
                return;
            }

            if (cmbWorkOfficer.SelectedItem == null)
            {
                Tools.ShowMessage("请输入受理人!", "", false);
                return;
            }

            if (cmbGradeType.SelectedItem == null)
            {
                Tools.ShowMessage("请输入扣分原因!", "", false);
                return;
            }

            if (count.Value == null || count.Value.Value == 0)
            {
                Tools.ShowMessage("请输入数量!", "", false);
                return;
            }


            SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();

            ser.SaveListCompleted += (object sender1, SMSRecordService.SaveListCompletedEventArgs e1) =>
            {
                string res = JsonSerializerHelper.JsonToEntity<string>(e1.Result);
                if (SaveCallBack != null)
                    SaveCallBack();

            };

            int cnt = Convert.ToInt32(count.Value.Value);

            smsRecord.IsSend = true;
            smsRecord.IsResponse = true;
            smsRecord.LoginOfficer = AppGlobal.CurrentUser;
            smsRecord.PersonMobile = ""; //数据库不能为空
            smsRecord.WorkType = new WorkType { Id = 4 }; //数据库不能为空
            smsRecord.Leader = AppGlobal.CurrentUser; //数据库不能为空


            string json = Newtonsoft.Json.JsonConvert.SerializeObject(smsRecord);
            ser.SaveListAsync(json, cnt);
            (this.Parent as RadWindow).Close();

        }

        private void update()
        {
            if (cboxStation.SelectedItem == null)
            {
                Tools.ShowMessage("请输入受理单位!", "", false);
                return;
            }

            if (cmbWorkOfficer.SelectedItem == null)
            {
                Tools.ShowMessage("请输入受理人!", "", false);
                return;
            }

            if (cmbGradeType.SelectedItem == null)
            {
                Tools.ShowMessage("请输入扣分原因!", "", false);
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
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(smsRecord);
            ser.SaveOrUpdateAsync(json);
            (this.Parent as RadWindow).Close();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as RadWindow).Close();
        }

        private bool CheckVerify()
        {
            return true;//temp!
        }

        private void cboxStation_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadOfficers();
        }
    }
}
