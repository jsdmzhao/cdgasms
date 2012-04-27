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

namespace PoliceSMS
{
    public class AppGlobal
    {
        private static PoliceSMS.Lib.Organization.Officer currentUser;
        public static PoliceSMS.Lib.Organization.Officer CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                if (currentUser != null && !string.IsNullOrEmpty(currentUser.Password))
                {
                    currentUser.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(currentUser.Password));
                }
            }
        }
    }
}
