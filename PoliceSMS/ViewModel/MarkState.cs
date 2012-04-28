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
    public class MarkState
    {
        private static readonly MarkState[] ary = new MarkState[] { 
            new MarkState { IsMark = false, Name = "未评价" }, 
            new MarkState { IsMark = true, Name = "已评价" } };

        private MarkState() { }
        public static MarkState[] CreateAry()
        {
            return ary;
        }

        public bool IsMark { get; set; }

        public string Name { get; set; }
    }
}
