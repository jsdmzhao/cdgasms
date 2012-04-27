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
using System.Windows.Data;
using System.Globalization;

namespace PoliceSMS
{
    public class SexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知性别";
            }
            else
            {
                return ((bool)value) ? "男" : "女";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class SexImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "../Themes/Images/user-silhouette.png";
            }
            else
            {
                return ((bool)value) ? "../Themes/Images/Male.png" : "../Themes/Images/female.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class SexBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (parameter as string).Equals("male") ? value : !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (parameter as string).Equals("male") ? value : !(bool)value;
        }
    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible ? true : false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
