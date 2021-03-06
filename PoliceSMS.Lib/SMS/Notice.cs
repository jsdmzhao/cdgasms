﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PoliceSMS.Lib.Core;

namespace PoliceSMS.Lib.SMS
{
    public class Notice : Item
    {
        public virtual string Title { get; set; }

        public virtual string Url { get; set; }
    }
}
