using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class AddRemoveAdmin : Page
    {
        public CUserInfo LoggedInUser;
        public String
          logMsgPageInfo = "",
          logMsgPageData = "",
          pageTitle = "Add/Remove Admin",
          userMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            if (!LoggedInUser.IsAdministrator) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack) PageSetup();
        }



        #region Page Methods
        protected void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable UserList = CWeb.UserAdminListUsers(LoggedInUser);
                if (UserList.Select("IsAdmin = 1").Count() > 0)
                {
                    lstAdminUsers.DataSource = UserList.Select("IsAdmin = 1").CopyToDataTable();
                    lstAdminUsers.DataBind();
                }

                DataTable NonAdmins = UserList.Select("IsAdmin = 0").CopyToDataTable();

                ddlNonAdminUsers.DataSource = NonAdmins.AsEnumerable().Select(row => new { empName = row.Field<string>("empName"), empAlias = row.Field<string>("username") }).Distinct().OrderBy(row => row.empName);
                ddlNonAdminUsers.DataTextField = "empName";
                ddlNonAdminUsers.DataValueField = "empAlias";
                ddlNonAdminUsers.DataBind();
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }
        #endregion

        #region User Event Handlers
        protected void cmdAddAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                CWeb.UserAdminUpdateAdminStatus(ddlNonAdminUsers.SelectedValue, true);
                CLog.WebEvent("Added Admin", Page.Title, LoggedInUser);
                Response.Redirect("~/AddRemoveAdmin.aspx");
            }
            catch (Exception ex)
            {
                CLog.WebError("Add Admin Error", Page.Title, LoggedInUser, ex);
            }

        }

        protected void DeleteAdmin(object sender, ListViewDeleteEventArgs e)
        {
            CLog.WebEvent("Deleting Admin Start", Page.Title, LoggedInUser);
            ListViewItem item = this.lstAdminUsers.Items[e.ItemIndex];
            String username = (item.FindControl("lblUsername") as Label).Text;
            try
            {
                CWeb.UserAdminUpdateAdminStatus(username, false);               
                if (LoggedInUser.LoginAlias == username)
                {
                    LoggedInUser.IsAdministrator = false;
                    Session["UserData"] = LoggedInUser;
                    Response.Redirect("~/Default.aspx");
                }
                Response.Redirect("~/AddRemoveAdmin.aspx");
                CLog.WebEvent("Deleted Admin", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("Delete Admin Error", Page.Title, LoggedInUser, ex);
            }
        } 
        #endregion
    }
}