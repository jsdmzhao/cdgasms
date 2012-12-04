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

    public class NegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                if (value is int)
                {
                    return ((int)value) < 0 ? 0 : value;
                }
                if (value is double)
                {
                    return ((double)value) < 0 ? 0 : value;
                }
                if (value is float)
                {
                    return ((float)value) < 0 ? 0 : value;
                }
                return 0;
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class StationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string v = value as string;
                if (v.StartsWith("巡警"))
                    return "巡警大队";
                if (v.StartsWith("局青羊区分局"))
                    return "分局";
                if (v.StartsWith("办公室"))
                    return "办公室";
                if (v.StartsWith("法制科"))
                    return "法制科";
                if (v.StartsWith("国内安全保卫大队"))
                    return "国保大队";
                if (v.StartsWith("纪检组、监察室"))
                    return "纪检组";
                if (v.StartsWith("禁毒大队"))
                    return "禁毒大队";
                if (v.StartsWith("经济犯罪侦查大队"))
                    return "经侦大队";
                if (v.StartsWith("信息通信科"))
                    return "信通科";
                if (v.StartsWith("刑警大队"))
                    return "刑警大队";
                if (v.StartsWith("政治处"))
                    return "政治处";
                if (v.StartsWith("治安大队"))
                    return "治安大队";
                if (v.StartsWith("治安防范人口管理科"))
                    return "防范科";
                if (v.StartsWith("治安科"))
                    return "治安科";
                if (v.StartsWith("装备财务科"))
                    return "装财科";
                if (v.StartsWith("公共信息网络安全监察大队"))
                    return "网监大队";
                
                if (v.Length > 2)
                    return v.Substring(0, 2);
                return value;
            }
            else
            {
                return value;
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
