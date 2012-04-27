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

namespace PoliceSMS.Views
{
    public partial class SMSRecordForm : UserControl
    {
        private SMSRecord smsRecord;

        private IList<Sex> sexs;

        private IList<GradeType> gradeTypes;

        private IList<WorkType> workTypes;

        private IList<OfficerType> officerTypes;

        private IList<Officer> officers;

        public Action<SMSRecord> SaveCallBack { get; set; }

        public SMSRecordForm()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SMSRecordForm_Loaded);
        }

        void SMSRecordForm_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = smsRecord;

            LoadSexs();
            LoadGradeTypes();
            LoadWorkTypes();
            LoadOfficerTypes();
            LoadOfficers();
        }

        public SMSRecord SMSRecord
        {
            get { return smsRecord; }
            set { smsRecord = value; }
        }

        private void LoadSexs()
        {
            try
            {
                //SexService.SexServiceClient ser = new SexService.SexServiceClient(AppGlobal.CreateHttpBinding(), new EndpointAddress(new Uri(Application.Current.Host.Source, "../../BaseWcf/SexService.svc")));
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

                ser.GetListByHQLAsync("from GradeType");

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

                ser.GetListByHQLAsync("from WorkType");

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

                    cmbWorkOfficer.ItemsSource = officers;

                };

                ser.GetListByHQLAsync("from Officer as e where e.Organization.id ="+AppGlobal.CurrentOrganization.Id);

            }
            catch (Exception ex)
            {
                Tools.ShowMessage("读取民警发生错误", ex.Message, false);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckVerify())
            {
                SMSRecordService.SMSRecordServiceClient ser = new SMSRecordService.SMSRecordServiceClient();

                ser.SaveOrUpdateCompleted += (object sender1, SMSRecordService.SaveOrUpdateCompletedEventArgs e1) =>
                    {
                        int id = JsonSerializerHelper.JsonToEntity<int>(e1.Result);
                        if (id > 0)
                        {
                            smsRecord.Id = id;
                            if (SaveCallBack != null)
                                SaveCallBack(smsRecord);
                        }
                    };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(smsRecord);
                ser.SaveOrUpdateAsync(json);
                (this.Parent as RadWindow).Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as RadWindow).Close();
        }

        private bool CheckVerify()
        {
            return true;//temp!
        }
    }
}
