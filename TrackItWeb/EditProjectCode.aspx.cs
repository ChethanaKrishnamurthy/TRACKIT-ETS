using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class EditProjectCode : System.Web.UI.Page
    {
        public CUserInfo LoggedInUser;
        public String
           logMsgPageInfo = "",
           logMsgPageData = "",
           pageTitle = "Edit Project Code",
           userMessage = "";

    
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            if (!LoggedInUser.IsAdministrator) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack)
            {
                RefreshProjects();
                PageSetup();
            }
        }
        #region Page Methods
        protected void RefreshProjects()
        {
            ViewState["AdminProjects"] = null;
            DataTable dtProjects = CWeb.UserAdminListProjects(LoggedInUser);
            ViewState["AdminProjects"] = dtProjects;
        }
        protected void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable dtProjects = (DataTable)ViewState["AdminProjects"];
                lstProjectCode.DataSource = dtProjects.AsEnumerable().Select(row => new
                {
                    Id = row.Field<int>("Id"),
                    project_code = row.Field<string>("project_code"),
                    description = row.Field<string>("description"),
                    project_org = row.Field<string>("project_org"),                   
                    start_date = row.Field<DateTime>("start_date").ToString("MM/dd/yyyy"),
                    end_date = row.Field<DateTime>("end_date").ToString("MM/dd/yyyy"),
                    Sf_Opportunity_Name = row.Field<string>("sf_opportunity_name"),
                    Sf_Project_Code = row.Field<string>("sf_project_code")
                });
                lstProjectCode.DataBind();
                ddlProjectOrg.DataSource = dtProjects.AsEnumerable().Select(row => new { projectorg = row.Field<string>("project_org") }).Distinct(); ;
                ddlProjectOrg.DataTextField = "projectorg";
                ddlProjectOrg.DataValueField = "projectorg";
                ddlProjectOrg.DataBind();
                ddlProjectOrg.Items.Insert(0, new ListItem("Select", "Select"));
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        } 
        #endregion


        #region User Event Handlers

        protected void lstProjectCode_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lstProjectCode.EditIndex = e.NewEditIndex;
            PageSetup();
        }
        protected void lstProjectCode_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lstProjectCode.EditIndex = -1;
            PageSetup();
        }
        protected void lstProjectCode_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataTable dtProjects = (DataTable)ViewState["AdminProjects"];
            if (lstProjectCode.EditIndex == (e.Item as ListViewDataItem).DataItemIndex)
            {
                DropDownList ddList = (DropDownList)e.Item.FindControl("ddlGridProjectOrg");
                ddList.DataSource = dtProjects.AsEnumerable().Select(row => new { projectorg = row.Field<string>("project_org") }).Distinct();
                ddList.DataTextField = "projectorg";
                ddList.DataValueField = "projectorg";
                ddList.DataBind();

                ddList.Items.FindByValue(((Label)e.Item.FindControl("lblGridProjectOrg")).Text).Selected = true;
            }
        }
        protected void lstProjectCode_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                CLog.WebEvent("Update Project Code Start", Page.Title, LoggedInUser);
                int Id = Convert.ToInt32(((Label)lstProjectCode.Items[e.ItemIndex].FindControl("lblId")).Text);
                String projectCode = Convert.ToString(((TextBox)lstProjectCode.Items[e.ItemIndex].FindControl("txtProjectCode")).Text);
                String SubProduct = Convert.ToString(((DropDownList)lstProjectCode.Items[e.ItemIndex].FindControl("ddlGridProjectOrg")).Text);
                String Description = Convert.ToString(((TextBox)lstProjectCode.Items[e.ItemIndex].FindControl("txtProjectDescription")).Text);             

                //update record for Project Code by calling the Procedure
                CWeb.UserAdminSaveProjects(LoggedInUser, Id, projectCode, Description, SubProduct);                
                RefreshProjects();
                lstProjectCode.EditIndex = -1;
                PageSetup();
                CLog.WebEvent("Updated Project Code", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("Update Project Code Error", Page.Title, LoggedInUser, ex);
            }
        }
        protected void cmdAddProjectCode_Click(object sender, EventArgs e)
        {
            try
            {
                CLog.WebEvent("Add Project Code Start", Page.Title, LoggedInUser);
                if (!String.IsNullOrEmpty(txtProjectCode.Text) && !String.IsNullOrEmpty(txtProjectDescription.Text) &&                    
                    (ddlProjectOrg.Text) != "Select")
                {

                    CWeb.UserAdminSaveProjects(LoggedInUser, 0, txtProjectCode.Text, txtProjectDescription.Text, ddlProjectOrg.SelectedValue);
                    ErrorMsg.Text = "Record was added successfully";
                    CLog.WebEvent("Added Project Code", Page.Title, LoggedInUser);
                    RefreshProjects();
                    PageSetup();
                    CLog.WebEvent("Added Project Code", Page.Title, LoggedInUser);
                }
                else
                {
                    ErrorMsg.Text = "Please Complete all the fields";                    
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", " $(\".temp\").click();", true);
            }
            catch (Exception ex)
            {
                CLog.WebError("Adding Project Code Error", Page.Title, LoggedInUser, ex);
            }
        }      
        #endregion
    }
}