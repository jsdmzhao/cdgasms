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
using PoliceSMS.Lib.Organization;

namespace PoliceSMS.ViewModel
{
    public class TreeViewItemModel
    {
        public int Id { get; set; }

        public int SMSUnitType { get; set; }

        public string Name { get; set; }

        public IList<Organization> Childs { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsActive { get; set; }
    }
}
