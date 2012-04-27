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
using System.Collections.Generic;

namespace PoliceSMS.Comm
{
    public class SessionManager
    {
        private static Dictionary<string, object> session = new Dictionary<string, object>();

        public static Dictionary<string, object> Session
        {
            get { return SessionManager.session; }
            set { SessionManager.session = value; }
        }
    }
}
