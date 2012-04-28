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
    public class UsedState
    {
        private static readonly UsedState[] ary = new UsedState[] { 
            new UsedState { Used = false, Name = "未使用" }, 
            new UsedState { Used = true, Name = "正在使用" } };

        private UsedState() { }
        public static UsedState[] CreateAry()
        {
            return ary;
        }

        public bool Used { get; set; }

        public string Name { get; set; }
    }
}
