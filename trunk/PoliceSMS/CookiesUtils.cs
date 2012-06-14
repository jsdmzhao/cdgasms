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
using System.Text;
using System.Windows.Browser;
using System.Linq;

namespace PoliceSMS
{
    public class CookiesUtils
    {
        public static void SetCookie(String key, String value)
        {
            SetCookie(key, value, null, null, null, false);
        }
        public static void SetCookie(String key, String value, TimeSpan expires)
        {
            SetCookie(key, value, expires, null, null, false);
        }
        public static void SetCookie(String key, String value, TimeSpan? expires,
            String path, String domain, bool secure)
        {
            StringBuilder cookie = new StringBuilder();
            cookie.Append(String.Concat(key, "=", value));
            if (expires.HasValue)
            {
                DateTime expire = DateTime.UtcNow + expires.Value;
                cookie.Append(String.Concat(";expires=", expire.ToString("R")));
            }
            if (!String.IsNullOrEmpty(path))
            {
                cookie.Append(String.Concat(";path=", path));
            }
            if (!String.IsNullOrEmpty(domain))
            {
                cookie.Append(String.Concat(";domain=", domain));
            }
            if (secure)
            {
                cookie.Append(";secure");
            }
            HtmlPage.Document.SetProperty("cookie", cookie.ToString());
        }
        public static string GetCookie(String key)
        {
            String[] cookies = HtmlPage.Document.Cookies.Split(';');
            String result = (from c in cookies
                             let keyValues = c.Split('=')
                             where keyValues.Length == 2 && keyValues[0].Trim() == key.Trim()
                             select keyValues[1]).FirstOrDefault();
            return result;
        }
        public static void DeleteCookie(String key)
        {
            DateTime expir = DateTime.UtcNow - TimeSpan.FromDays(1);
            string cookie = String.Format("{0}=;expires={1}",
                key, expir.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }
        public static bool Exists(String key, String value)
        {
            return HtmlPage.Document.Cookies.Contains(String.Format("{0}={1}", key, value));
        }
    }
}
