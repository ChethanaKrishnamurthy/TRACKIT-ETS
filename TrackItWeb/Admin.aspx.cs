using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class Admin : Page
    {
        public CUserInfo LoggedInUser;
        public String
            logMsgPageInfo = "",
            logMsgPageData = "",
            pageTitle = "Admin Page",
            userMessage = "";

        #region Page Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            CLog.Information(String.Format("Page {0} Load Start for user {1}", Page.Title, LoggedInUser.LoginAlias));           
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            if (!LoggedInUser.IsAdministrator) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
        } 
        #endregion
    }
}