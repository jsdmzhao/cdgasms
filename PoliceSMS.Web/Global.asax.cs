using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NHibernate.Cfg;

namespace PoliceSMS.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var hbmConfiguration = new Configuration();

            var factory = hbmConfiguration.Configure().BuildSessionFactory();
            HttpContext.Current.Cache["HbmFactory"] = factory;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}