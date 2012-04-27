using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace PoliceSMS.Web.Comm
{
    public class SessionFactory
    {
        public static ISessionFactory HbmSessionFactory
        {
            get
            {
                return HttpContext.Current.Cache["HbmFactory"] as ISessionFactory;
            }
        }

        public static ISession Session
        {
            get
            {
                ISessionFactory factory = HttpContext.Current.Cache["HbmFactory"] as ISessionFactory;
                return factory.OpenSession();
            }
        }
    }
}