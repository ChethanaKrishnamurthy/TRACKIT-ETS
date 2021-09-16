using System;
using System.Web;
using System.Web.Optimization;

namespace TrackItWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {            
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SysComponents.CApplication.Start(Server.MachineName);
        }
        void Application_End(object sender, EventArgs e)
        {
            SysComponents.CApplication.End();
        }

        void Application_Error(object sender, EventArgs e)
        {
            
        }
        void Session_Start(object sender, EventArgs e)
        {
            
            String LoggedInUser = User.Identity.Name;                    
            CWeb.UserLogin(Session, LoggedInUser, Request);
        }

        void Session_End(object sender, EventArgs e)
        {
            CWeb.UserLogout(Session);
        }
    }
}