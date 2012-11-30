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

namespace PoliceSMS.ViewModel
{
    /// <summary>
    /// 用以sql，因为sql的datetime.min 和 c# 不同
    /// </summary>
    public static class ExtDateTime
    {
        public static DateTime SqlMinValue(this DateTime sqlDateTime)
        {
            return new DateTime(1900, 01, 01, 00, 00, 00);
        }
        public static DateTime SqlMaxValue(this DateTime sqlDateTime)
        {
            return new DateTime(2079, 06, 06, 23, 59, 00);
        }


    }
}
