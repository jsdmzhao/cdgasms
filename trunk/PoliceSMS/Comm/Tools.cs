using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using System.IO;
using System.Text;

namespace PoliceSMS.Comm
{
    public static class Tools
    {
        public struct RegularExp
        {
            public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
            public const string Color = "^#[a-fA-F0-9]{6}";
            public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
            public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            public const string Float = @"^(-?\d+)(\.\d+)?$";
            public const string ImageFormat = @"\.(?i:jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$";
            public const string Integer = @"^-?\d+$";
            public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            public const string Letter = "^[A-Za-z]+$";
            public const string LowerLetter = "^[a-z]+$";
            public const string MinusFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
            public const string MinusInteger = "^-[0-9]*[1-9][0-9]*$";
            public const string Mobile = "^0{0,1}13[0-9]{9}$";
            public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
            public const string Numeric = "^[0-9]+$";
            public const string NumericOrLetter = "^[A-Za-z0-9]+$";
            public const string NumericOrLetterOrUnderline = @"^\w+$";
            public const string PlusFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
            public const string PlusInteger = "^[0-9]*[1-9][0-9]*$";
            public const string Telephone = @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?";
            public const string UnMinusFloat = @"^\d+(\.\d+)?$";
            public const string UnMinusInteger = @"\d+$";
            public const string UnPlusFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
            public const string UnPlusInteger = @"^((-\d+)|(0+))$";
            public const string UpperLetter = "^[A-Z]+$";
            public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        }
        public static bool IsChinese(string pendingStr)
        {
            if (Regex.IsMatch(pendingStr, RegularExp.Chinese))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsNumericOrLetter(string pendingStr)
        {
            if (Regex.IsMatch(pendingStr, RegularExp.NumericOrLetter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsNumeric(string pendingStr)
        {
            if (Regex.IsMatch(pendingStr, RegularExp.Numeric))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<T> CheckSelectItems<T>(object view)
        {
            if (view.GetType().GetProperty("SelectedItem").GetValue(view, null) == null)
            {
                DialogParameters parameters = new DialogParameters();
                //   parameters.Theme = ThemeManager.FromName("Office_Black");
                parameters.Header = "操作提示";
                parameters.Content = "请选中要操作的记录";
                parameters.OkButtonContent = "确定";
                RadWindow.Alert(parameters);
                return null;
            }
            else
            {
                List<T> list = new List<T>();
                foreach (object item in (IList<object>)view.GetType().GetProperty("SelectedItems").GetValue(view, null))
                {
                    list.Add((T)item);
                }
                return list;
            }
        }
        public static void ShowMask(bool isMask)
        {
            ShowMask(isMask, string.Empty);
        }
        public static void ShowMask(bool isMask,string message)
        {
            RadBusyIndicator busy = App.Current.RootVisual.FindChildByType<RadBusyIndicator>();
            busy.IsBusy = isMask;
            busy.BusyContent = "正在加载数据...";
            if (!string.IsNullOrEmpty(message))
            {
                busy.BusyContent = message;
            }
        }

        public static RadWindow OpenWindow(string header, object content)
        {

            return OpenWindow(header, content, null);
        }
        public static RadWindow OpenWindow(string header, object content, object icon)
        {

            return OpenWindow(header, content, icon,null);
        }

        public static RadWindow OpenWindow(string header, object content, object icon, Action callback)
        {

            return OpenWindow(header, content, icon,null,null, callback);
        }

        public static RadWindow OpenWindow(string header, object content, object icon, int? width, int? height)
        {
            return OpenWindow(header, content, icon, width, height, null);
        }
        public static RadWindow OpenWindow(string header, object content, object icon, int? width, int? height, Action callback)
        {
            try
            {
                RadWindow window = new RadWindow();
              //  StyleManager.SetTheme(window, ThemeManager.FromName("Office_Black"));
                window.Style = Application.Current.Resources["DefaultWindowStyle"] as Style;
                if (width != null && width.Value != 0)
                {
                    window.Width = width.Value;
                }
                if (height != null && height.Value != 0)
                {
                    window.Height = height.Value;
                }
                window.Header = header;
                window.Icon = icon;
                window.WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation.CenterScreen;
                window.Content = content;
                window.Opacity = 1;
                window.Closed += (o, e) =>
                {
                    Tools.PerformOutEffect(window).Begin();
                    if (callback != null)
                        callback();
                };
                window.ShowDialog();
                Tools.PerformInEffect(window).Begin();
                return window;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static void OpenConfirm(object content, EventHandler<WindowClosedEventArgs> closedCallblack)
        {
            DialogParameters p = new DialogParameters();
            p.Theme = ThemeManager.FromName("Office_Black");
            p.Header = "确认操作";
            p.Content = content;
            p.OkButtonContent = "确定";
            p.CancelButtonContent = "取消";
            p.Closed = closedCallblack;
            RadWindow.Confirm(p);
        }
        public static Storyboard PerformOutEffect(DependencyObject target)
        {
            DoubleAnimation animationForOpacity = new DoubleAnimation();
            Storyboard.SetTarget(animationForOpacity, target);
            Storyboard.SetTargetProperty(
                animationForOpacity, new PropertyPath(FrameworkElement.OpacityProperty));
            animationForOpacity.From = 1;
            animationForOpacity.To = 0;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForOpacity);

            return storyBoard;
        }
        public static Storyboard PerformInEffect(DependencyObject target)
        {
            DoubleAnimation animationForOpacity = new DoubleAnimation();
            Storyboard.SetTarget(animationForOpacity, target);
            Storyboard.SetTargetProperty(
                animationForOpacity, new PropertyPath(FrameworkElement.OpacityProperty));
            animationForOpacity.From = 0;
            animationForOpacity.To = 1;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForOpacity);
            return storyBoard;
        }
        public static string FormatSize(long size)
        {
            const long k = 1024;
            const long m = 1024 * 1024;
            const long g = 1024 * 1024 * 1024;
            string postfix = string.Empty;

            if (size < k)
            {
                postfix = "B";
                ////size = size;
            }
            else if (size < m)
            {
                postfix = "kB";
                size /= k;
            }
            else if (size < g)
            {
                postfix = "mB";
                size /= m;
            }
            else
            {
                postfix = "gB";
                size /= g;
            }
            return "(" + size + postfix + ")";
        }

        public static Uri ConstructAbsoluteUri(Uri url)
        {
            if (!url.IsAbsoluteUri)
            {
                System.Uri source = System.Windows.Application.Current.Host.Source;
                string server = source.AbsoluteUri.Remove(source.AbsoluteUri.Length - source.AbsolutePath.Length);
                int serverLen = server.Length;
                string relativePath = url.OriginalString;
                const string PathSeparator = "/";

                if (relativePath.StartsWith(PathSeparator, StringComparison.OrdinalIgnoreCase))
                {
                    //// ; nothing to do - just continue!
                }
                else if (relativePath.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Substring(1);
                }
                else if (relativePath.StartsWith("./", StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Remove(0, 1);
                    server = source.AbsoluteUri.Remove(source.AbsoluteUri.LastIndexOf(PathSeparator, StringComparison.OrdinalIgnoreCase));
                }
                else if (relativePath.StartsWith("../", StringComparison.OrdinalIgnoreCase))
                {
                    server = RemoveLastNode(source.AbsoluteUri, PathSeparator, serverLen);
                    while (relativePath.StartsWith("../", StringComparison.OrdinalIgnoreCase))
                    {
                        relativePath = relativePath.Remove(0, 3);
                        server = RemoveLastNode(server, PathSeparator, serverLen);
                    }
                    server += PathSeparator;
                }
                else
                {
                    server += PathSeparator;
                }
                url = new Uri(server + relativePath, UriKind.Absolute);
            }
            return url;
        }
        public static string RemoveLastNode(string path, string separator, int stopAt)
        {
            int i = path.LastIndexOf(separator, StringComparison.OrdinalIgnoreCase);

            if (i < stopAt)
            {
                i = stopAt;
            }

            if (i < path.Length)
            {
                if (i <= 0)
                {
                    path = string.Empty;
                }
                else if (i > 0)
                {
                    path = path.Remove(i);
                }
            }

            return path;
        }
        public static void ShowMessage(string message, string detail, bool success)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Theme = ThemeManager.FromName("Office_Black");

            TextBlock block = new TextBlock();
            block.MinWidth = 100;
            block.MaxWidth = 350;
            block.Text = message;
            block.TextWrapping = TextWrapping.Wrap;
            Image image = new Image();


            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Left;
            parameters.OkButtonContent = "确定";
            if (success)
            {
                block.Margin = new Thickness(10, 0, 0, 0);

                //   image.Source = new BitmapImage(new Uri("/Images/applications-internet.png", UriKind.Relative));
                panel.Children.Add(image);
                panel.Children.Add(block);
                
                parameters.Content = panel;
                //  parameters.Content = message;
                parameters.Header = "提示消息";

            }
            else
            {
                parameters.Header = "错误提示";


                block.Margin = new Thickness(10, 0, 0, 0);
                //      block.VerticalAlignment = VerticalAlignment.Center;
                //     image.Source = new BitmapImage(new Uri("/Images/dialog-warning.png", UriKind.Relative));
                image.VerticalAlignment = VerticalAlignment.Top;
                panel.Children.Add(image);
                panel.Children.Add(block);

                StackPanel panel1 = new StackPanel();
                panel1.Orientation = Orientation.Vertical;

                panel1.Children.Add(panel);
                if (!string.IsNullOrEmpty(detail))
                {
                    RadExpander expander = new RadExpander();
                    expander.Header = "  展开其详细跟踪信息，可以复制进行质询。";
                    expander.MinWidth = 200;
                    expander.MaxWidth = 400;
                    expander.Margin = new Thickness(0, 10, 0, 0);
                    TextBox block1 = new TextBox();
                    block1.MaxWidth = 396;
                    block1.MinWidth = 100;
                    block1.Text = detail;
                    block1.MaxHeight = 200;
                    block1.TextWrapping = TextWrapping.Wrap;
                    expander.Content = block1;
                    expander.IsExpanded = true;
                    expander.HorizontalAlignment = HorizontalAlignment.Left;
                    panel1.Children.Add(expander);

                }
                parameters.Content = panel1;
                //   parameters.Content = message;
            }
            RadWindow.Alert(parameters);
        }
        static public void SetBinding(FrameworkElement guiElement, DependencyProperty valueProperty, string path)
        {

            Binding binding = new Binding();
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            binding.NotifyOnValidationError = true;
            binding.ValidatesOnExceptions = true;
            guiElement.SetBinding(valueProperty, binding);
        }

        public static T FindFirstVisualChild<T>(DependencyObject obj, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T && child.GetValue(FrameworkElement.NameProperty).ToString() == childName)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindFirstVisualChild<T>(child, childName);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        public static void ExportToWord(string text)
        {
            string extension = "doc";

            string selectedItem = "Word";
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = extension;
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, selectedItem);
            
            dialog.FilterIndex = 1;
            
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = dialog.OpenFile())
                    {

                        byte[] bs = Encoding.UTF8.GetBytes(text);
                        stream.Write(bs, 0, bs.Length);
                        stream.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Tools.ShowMessage(ex.Message,"",false);
                    
                }
            }
        }
    }
}