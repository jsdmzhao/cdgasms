using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using PoliceSMS.Lib.Organization;
using System.ServiceModel;
using Telerik.Windows.Controls;

namespace PoliceSMS
{
    public static class AppGlobal
    {
        /// <summary>
        /// 用于弹出的Dialog
        /// 因为BusyIndicator包含在MainWindow中
        /// 弹出的新Window如果使用BusyIndicator会调用MainWindow中的BusyIndicator
        /// 此属性和Tools.OpenCustomWindow一起使用
        /// 在弹出的新窗口调用此Page中的BusyIndicator，需保证此Page中包含BusyIndicator
        /// 注意需要手动将此属性置空，否则出现BusyIndicator引用错误
        /// </summary>
        public static Page CurrentDialogPage { get; set; }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        public static void Clear()
        {
            //banks = null;
            //organizations = null;
            //controlTypes = null;
            //contactTypes = null;
            //receiveTypeCalls = null;
            //invTypes = null;

            //CurrentUser = null;
            //CurrentRights = null;
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        private static Officer currentUser;
        public static Officer CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                if (value != null)
                {
                    currentUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(currentUser.Password));
                }
            }
        }

        public static bool HasPermission()
        {
            if(currentUser == null)
                return false;
            if (currentUser.Code.ToLower() == "admin")
                return true;
            else
                return false;
        }


        public static BasicHttpBinding CreateHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            return binding;
        }

        private static int pageSize = 15;
        public static int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value > 0)
                    pageSize = value;
            }
        }
    }
}
