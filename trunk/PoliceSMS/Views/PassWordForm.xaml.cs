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
using PoliceSMS.Comm;
using System.ServiceModel;
using PoliceSMS.Lib.Organization;

namespace PoliceSMS.Views
{
    public partial class PassWordForm : UserControl
    {
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action CallBack { get; set; }

        public PassWordForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string oldtxtPass = this.oldtxtPass.Password.Trim();

            string txtPass = this.txtPass.Password.Trim();

            string checktxtPass = this.checktxtPass.Password.Trim();

            if (txtPass != checktxtPass)
            {
                Tools.ShowMessage("两次密码输入不一致！", string.Empty, false);
            }
            else
            {
                OfficerService.OfficerServiceClient ser1 = new OfficerService.OfficerServiceClient();
                //登录完成的事件
                ser1.LoginCompleted += (object sender1, OfficerService.LoginCompletedEventArgs e1) =>
                {
                    Officer LoginUser = JsonSerializerHelper.JsonToEntity<Officer>(e1.Result);
                    if (LoginUser != null)
                    {
                        ser1.SaveOrUpdateCompleted += (object sender2, OfficerService.SaveOrUpdateCompletedEventArgs e2) =>
                            {
                                CallBack();
                            };
                        LoginUser.Password = txtPass;

                        ser1.SaveOrUpdateAsync(JsonSerializerHelper.EntityToJson<Officer>(LoginUser));
                    }
                    else
                    {
                        Tools.ShowMessage("原始密码输入不正确！", string.Empty, false);
                    }
                };
                ser1.LoginAsync(AppGlobal.CurrentUser.Code, oldtxtPass);
            }
        }

        private void btnExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as RadWindow).Close();
        }
    }
}
