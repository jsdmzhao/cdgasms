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

namespace PoliceSMS.Lib.SMS
{
    public class SMSRecord
    {
        public int Id { get; set; }

        public string PersonName { get; set; }

        public string PersonIdCard { get; set; }

        public string WorkContent { get; set; }

        public Organization.Organization Organization { get; set; }

        public Organization.Officer Officer { get; set; }

        
    }
}
