using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;
using System.Text;
using System.Collections.Generic;

namespace TrackItWeb
{
    public partial class UpdateDepartmentAssociations : Page
    {
        public CUserInfo LoggedInUser;
        public String
          logMsgPageInfo = "",
          logMsgPageData = "",
          pageTitle = "Update Department Asscoiations",
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

                DataTable adminDepartmentList = CWeb.UserAdminListDepartments(LoggedInUser);

                ddlDepartment.DataSource = adminDepartmentList.AsEnumerable() .Select(row => new
                {
                    Department = row.Field<string>("cchlvl1"),
                    Id = row.Field<int>("Id")
                }) .Distinct()
                   .OrderBy(row => row.Department);
                ddlDepartment.DataTextField = "Department";
                ddlDepartment.DataValueField = "Id";
                ddlDepartment.DataBind();

                ddlProjectTypes.Items.Add("Category");
                ddlProjectTypes.Items.Add("SubCategory");
                ddlProjectTypes.Items.Add("ActivityType");

                StrSaveMsg.Text = "";
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }

        
        #endregion

        #region User Event Handlers

        protected void btnAllocate_Click(object sender, EventArgs e)
        {            

            DataTable dtDepProjectType = CWeb.UserAdminListDepartmentProjectTypes(LoggedInUser, int.Parse(ddlDepartment.SelectedValue), ddlProjectTypes.SelectedValue);
            ViewState["DepProjectTypeData"] = dtDepProjectType;

            DataTable dtDepProjectTypeIsAssc = null;
            DataTable dtDepProjectTypeIsNotAssc = null;

            var asscRows = dtDepProjectType.Select("IsAssociated = 0");
            var notAsscRows1 = dtDepProjectType.Select("IsAssociated = 1");

            if (asscRows.Any())           
                dtDepProjectTypeIsNotAssc = asscRows.CopyToDataTable();
            if (notAsscRows1.Any())
                dtDepProjectTypeIsAssc = notAsscRows1.CopyToDataTable();

            if (dtDepProjectTypeIsNotAssc != null)
            {
                lstDepPjtTypes.DataSource = dtDepProjectTypeIsNotAssc.AsEnumerable().Select(row => new
                {
                    PjtTypeId = row.Field<int>("ProjectTypeId"),
                    code = row.Field<string>("code")
                }).Distinct();
                lstDepPjtTypes.DataTextField = "code";
                lstDepPjtTypes.DataValueField = "PjtTypeId";
                lstDepPjtTypes.DataBind();
            }

            if (dtDepProjectTypeIsAssc != null)
            {
                lstCategoryWithAsscociation.DataSource = dtDepProjectTypeIsAssc.AsEnumerable().Select(row => new
                {
                    PjtTypeId = row.Field<int>("ProjectTypeId"),
                    code = row.Field<string>("code")
                }).Distinct();
                lstCategoryWithAsscociation.DataTextField = "code";
                lstCategoryWithAsscociation.DataValueField = "PjtTypeId";
                lstCategoryWithAsscociation.DataBind();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                var SB = new StringBuilder();
                foreach (ListItem lst in lstCategoryWithAsscociation.Items)
                    SB.Append(lst.Text + ",");
                var allowedIds = SB.ToString().Substring(0, (SB.Length - 1));

                CWeb.AdminDeptAssociationsSave(LoggedInUser, int.Parse(ddlDepartment.SelectedValue), ddlProjectTypes.SelectedValue, allowedIds);
                StrSaveMsg.Text = "Associated Successfully";
                allowedIds = "";
            }
            catch(Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Saving", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCategoryWithAsscociation.Items.Clear();
            lstDepPjtTypes.Items.Clear();
            StrSaveMsg.Text = "";
        }

        protected void ddlProjectTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCategoryWithAsscociation.Items.Clear();
            lstDepPjtTypes.Items.Clear();
            StrSaveMsg.Text = "";
        }


        protected void LeftClick(object sender, EventArgs e)
        {
            //List will hold items to be removed.
            List<ListItem> removedItems = new List<ListItem>();

            //Loop and transfer the Items to Destination ListBox.
            foreach (ListItem item in lstCategoryWithAsscociation.Items)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                    lstDepPjtTypes.Items.Add(item);
                    removedItems.Add(item);
                }
            }

            //Loop and remove the Items from the Source ListBox.
            foreach (ListItem item in removedItems)
            {
                lstCategoryWithAsscociation.Items.Remove(item);
            }
            StrSaveMsg.Text = "";
        }

        protected void RightClick(object sender, EventArgs e)
        {
            //List will hold items to be removed.
            List<ListItem> removedItems = new List<ListItem>();

            //Loop and transfer the Items to Destination ListBox.
            foreach (ListItem item in lstDepPjtTypes.Items)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                    lstCategoryWithAsscociation.Items.Add(item);
                    removedItems.Add(item);
                }
            }

            //Loop and remove the Items from the Source ListBox.
            foreach (ListItem item in removedItems)
            {
                lstDepPjtTypes.Items.Remove(item);
            }
            StrSaveMsg.Text = "";
        }
        #endregion
    }
}