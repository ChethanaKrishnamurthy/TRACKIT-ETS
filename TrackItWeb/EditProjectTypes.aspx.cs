using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class EditCategory : System.Web.UI.Page
    {
        public CUserInfo LoggedInUser;
        public String
           logMsgPageInfo = "",
           logMsgPageData = "",
           pageTitle = "Edit Category",
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
            ViewState["AdminCategory"] = null;
            DataTable dtCategory = CWeb.UserListCategory(LoggedInUser, "Category", true);
            ViewState["AdminCategory"] = dtCategory;
        }
        protected void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable dtCategory = (DataTable)ViewState["AdminCategory"];
                lstCategory.DataSource = dtCategory.AsEnumerable().Select(row => new
                {
                    Id = row.Field<int>("Id"),
                    project_code = row.Field<string>("code"),
                    description = row.Field<string>("descr")                   
                });
                lstCategory.DataBind();                
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        } 
        #endregion


        #region User Event Handlers

        protected void lstCategory_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lstCategory.EditIndex = e.NewEditIndex;
            PageSetup();
        }
        protected void lstCategory_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lstCategory.EditIndex = -1;
            PageSetup();
        }
        protected void lstCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataTable dtCategory = (DataTable)ViewState["AdminCategory"];
            if (lstCategory.EditIndex == (e.Item as ListViewDataItem).DataItemIndex)
            {
                DropDownList ddList = (DropDownList)e.Item.FindControl("ddlGridProjectOrg");
                ddList.DataSource = dtCategory.AsEnumerable().Select(row => new { projectorg = row.Field<string>("project_org") }).Distinct();
                ddList.DataTextField = "projectorg";
                ddList.DataValueField = "projectorg";
                ddList.DataBind();

                ddList.Items.FindByValue(((Label)e.Item.FindControl("lblGridProjectOrg")).Text).Selected = true;
            }
        }
        protected void lstCategory_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                CLog.WebEvent("Update Project Code Start", Page.Title, LoggedInUser);
                int Id = Convert.ToInt32(((Label)lstCategory.Items[e.ItemIndex].FindControl("lblId")).Text);
                String projectCode = Convert.ToString(((TextBox)lstCategory.Items[e.ItemIndex].FindControl("txtProjectCode")).Text);
                String SubProduct = Convert.ToString(((DropDownList)lstCategory.Items[e.ItemIndex].FindControl("ddlGridProjectOrg")).Text);
                String Description = Convert.ToString(((TextBox)lstCategory.Items[e.ItemIndex].FindControl("txtProjectDescription")).Text);             

                //update record for Category by calling the Procedure
                CWeb.UserAdminSaveProjects(LoggedInUser, Id, projectCode, Description, SubProduct);                
                RefreshProjects();
                lstCategory.EditIndex = -1;
                PageSetup();
                CLog.WebEvent("Updated Project Code", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("Update Project Code Error", Page.Title, LoggedInUser, ex);
            }
        }
        protected void cmdAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                CLog.WebEvent("Add Project Code Start", Page.Title, LoggedInUser);
                if (!String.IsNullOrEmpty(txtCategory.Text) && !String.IsNullOrEmpty(txtCategoryDescription.Text))
                {
                   // CWeb.UserAdminSaveProjects(LoggedInUser, 0, txtCategory.Text, txtCategoryDescription.Text);
                    ErrorMsg.Text = "Record was added successfully";
                    CLog.WebEvent("Added Category", Page.Title, LoggedInUser);
                    RefreshProjects();
                    PageSetup();
                    CLog.WebEvent("Added Category", Page.Title, LoggedInUser);
                }
                else
                {
                    ErrorMsg.Text = "Please Complete all the fields";                    
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", " $(\".temp\").click();", true);
            }
            catch (Exception ex)
            {
                CLog.WebError("Adding CAtegory Error", Page.Title, LoggedInUser, ex);
            }
        }      
        #endregion
    }
}