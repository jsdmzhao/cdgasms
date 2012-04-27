using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PoliceSMS.Comm;

namespace PoliceSMS.Views
{
    public partial class GradeTypeList : Page
    {
        public GradeTypeList()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GradeTypeForm frm = new GradeTypeForm();

            Tools.OpenWindow("评分内容-新增", frm, null);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            GradeTypeForm frm = new GradeTypeForm();

            Tools.OpenWindow("评分内容-编辑", frm, null);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
