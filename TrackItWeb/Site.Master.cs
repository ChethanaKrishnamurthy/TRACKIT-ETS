using System;
using System.Web.UI;
using SysComponents;

namespace TrackItWeb
{
    public partial class SiteMaster : MasterPage
    {
        public CUserInfo LoggedInUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            lblLoggedInUser.Text = LoggedInUser.FullName;
            lblLoggedInUser.ToolTip = String.Format("{0,-15}: {1}\n{2,-15}: {3}\n{4,-15}: {5}\n{6,-15}: {7}\n", "Username", LoggedInUser.LoginAlias, "Manager", LoggedInUser.ManagerFullName, "Department", LoggedInUser.cchlvl1, "Last login", LoggedInUser.LastLoginTime.ToString("dddd MMM dd yyyy hh:mm tt"));
            TrackItLink.Visible = LoggedInUser.IsValidUser;
            ReportsLink.Visible = LoggedInUser.IsValidUser;
            AdminLink.Visible = LoggedInUser.IsAdministrator;
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            if (!LoggedInUser.IsManager) ManagerUpdateUserEntry.Visible = false;
        }
        public CUserInfo CurrentUser
        {
            get { return LoggedInUser; }
        }
    }
}