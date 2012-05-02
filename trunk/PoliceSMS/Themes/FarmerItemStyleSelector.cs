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
using Telerik.Windows.Controls;
using PoliceSMS.Lib.Report;

namespace PoliceSMS.Themes
{
    public class FarmerItemStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var farmer = item as StationReportResult;
            if (farmer.StationRate<70)
                return this.RedStyle;

            return this.NormalStyle;
        }

        public Style NormalStyle { get; set; }
        public Style GreenStyle { get; set; }

        public Style OrangeStyle { get; set; }
        public Style RedStyle { get; set; }
    }
}
