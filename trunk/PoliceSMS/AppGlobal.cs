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

namespace PoliceSMS
{
    public static class AppGlobal
    {
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

        public static Organization CurrentOrganization { get; set; }

        public static BasicHttpBinding CreateHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            return binding;
        }
    }
}
