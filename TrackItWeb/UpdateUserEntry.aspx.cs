using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class UpdateUserEntry : Page
    {
        public CUserInfo LoggedInUser;
        public CUserInfo usr;
        public String
          logMsgPageInfo = "",
          logMsgPageData = "",
          pageTitle = "Update User Entry",
          userMessage = "";

        private int TotalTimeWorkedMin = 0;
        private int TotalTimeWorkedHrs = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
             
            if (!IsPostBack)                
                PagesetUp(null, null, null, null, null, true);
                lblSNOW.Text = "";
        }


        #region Page Methods
        protected void PagesetUp(String Name, String Alias, String ManagerName, String EmployeeType, String Status, bool RefreshControl)
        {
            try
            {              

                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable dt = CWeb.UserAdminReportUsers(LoggedInUser, Name, Alias, ManagerName, EmployeeType, Status);
                if (RefreshControl)
                {
                    lstUser.DataSource = dt.AsEnumerable().Select(x => new { empname = x.Field<string>("empname"), UserName = x.Field<string>("username") }).OrderBy(x => x.empname);
                    lstUser.DataTextField = "empname";
                    lstUser.DataValueField = "username";
                    lstUser.DataBind();
                    lstUser.Items.Insert(0, new ListItem("Select", null));
                }

                DataTable dtProjects = CWeb.UserListProjects(LoggedInUser);
                ViewState["ProjectData"] = dtProjects;

                DataTable dtCategory = CWeb.UserListCategory(LoggedInUser, "Category", false);
                ViewState["CategoryData"] = dtCategory;

                DataTable dtSubCategory = CWeb.UserListSubCategory(LoggedInUser, "SubCategory", false);
                ViewState["SubCategoryData"] = dtSubCategory;

                DataTable dtActivity = CWeb.UserListActivityType(LoggedInUser, "ActivityType", false);
                ViewState["ActivityTypeData"] = dtActivity;

                txtsdate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dtGetEntries = CWeb.AdminUserGetEntries(LoggedInUser, LoggedInUser.UserID, txtsdate.Text);
                ViewState["GetEntriesData"] = dtGetEntries;

                // Setting up date for Project Code Info                
                tblProjects.DataSource = dtProjects;
                tblProjects.DataBind();

                cmdSave.Visible = false;
                txtTotalPer.Visible = false;
                lblTotalPer.Visible = false;
                txtTotalPer.Text = "100";
                
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }
        
        private void PageEntriesSetup(CUserInfo SelectedUser, DateTime EntryDate)
        {
            try
            {
                CLog.WebEvent("PageEntriesSetup Start", Page.Title, LoggedInUser);

                //Display SNOW Entries
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
                DataTable dtGetEntries = CWeb.AdminUserGetEntries(SelectedUser, SelectedUser.UserID, txtsdate.Text);

                // Setting up date for Project Code Info
                DataTable dtProjects = CWeb.UserListProjects(LoggedInUser);
                ViewState["ProjectData"] = dtProjects;
                
                tblProjects.DataSource = dtProjects;
                tblProjects.DataBind();


                ListView1.DataSource = dtGetEntries.AsEnumerable().Select
                    (row => new
                    {
                        UserId = row.Field<int?>("UserId"),
                        SNo = row.Field<int?>("SNo"),
                        Project = row.Field<int?>("ProjectId"),
                        Category = row.Field<int?>("CategoryId"),
                        SubCategory = row.Field<int?>("SubCategoryId"),
                        ActivityType = row.Field<int?>("ActivityTypeId"),
                        Comment = row.Field<string>("Comment"),
                        AllocatedPer = row.Field<int?>("ProjectPer")
                    }
                     );
                ListView1.DataBind();

                cmdSave.Visible = true;
                txtTotalPer.Visible = true;
                lblTotalPer.Visible = true;
                txtTotalPer.Text = "100";
                CLog.WebEvent("PageEntriesSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageEntriesSetup Error", Page.Title, LoggedInUser, ex);
            }
        }

        protected void OnItemDataBound(object sender, ListViewItemEventArgs e)
        {

            CUserInfo Selectedusr = new CUserInfo(lstUser.SelectedValue);
            Selectedusr.GetUserInfo(lstUser.SelectedValue, Session.SessionID);
            // Project Codes 

            DataTable dtProjects = (DataTable)ViewState["ProjectData"];

            DropDownList ddlPjt = (e.Item.FindControl("lstProject1") as DropDownList);
            ddlPjt.DataSource = dtProjects;
            ddlPjt.DataTextField = "code";
            ddlPjt.DataValueField = "Id";
            ddlPjt.DataBind();

            for (int d = 0; d < ddlPjt.Items.Count; d++)
            {
                ddlPjt.Items[d].Attributes.Add("title", dtProjects.Rows[d]["descr"].ToString());
            }
            Label lblPjt = (e.Item.FindControl("lblPjt1") as Label);
            if (lblPjt.Text == "")
                lblPjt.Text = "1";
            ddlPjt.Items.FindByValue(lblPjt.Text).Selected = true;

            //Category

            DataTable dtCategory = CWeb.UserListCategory(Selectedusr, "Category", false);            

            DropDownList ddlCat = (e.Item.FindControl("lstProject2") as DropDownList);
            ddlCat.DataSource = dtCategory;
            ddlCat.DataTextField = "code";
            ddlCat.DataValueField = "Id";
            ddlCat.DataBind();
            for (int x = 0; x < ddlCat.Items.Count; x++)
            {
                ddlCat.Items[x].Attributes.Add("title", dtCategory.Rows[x]["descr"].ToString());
            }
            Label lblCat = (e.Item.FindControl("lblPjt2") as Label);
            if (lblCat.Text == "")
                lblCat.Text = "1";
            //ddlCat.Items.FindByValue(lblCat.Text).Selected = true;
            try
            {
                ddlCat.Items.FindByValue(lblCat.Text).Selected = true;
            }
            catch
            {
                ddlCat.Items.FindByValue("1").Selected = true;
            }

            //Sub-Category

            DataTable dtSubCategory = CWeb.UserListSubCategory(Selectedusr, "SubCategory", false);

            DropDownList ddlSubCat = (e.Item.FindControl("lstProject3") as DropDownList);
            ddlSubCat.DataSource = dtSubCategory;
            ddlSubCat.DataTextField = "code";
            ddlSubCat.DataValueField = "Id";
            ddlSubCat.DataBind();
            for (int y = 0; y < ddlSubCat.Items.Count; y++)
            {
                ddlSubCat.Items[y].Attributes.Add("title", dtSubCategory.Rows[y]["descr"].ToString());
            }
            Label lblSubCat = (e.Item.FindControl("lblPjt3") as Label);
            if (lblSubCat.Text == "")
                lblSubCat.Text = "1";
            //ddlSubCat.Items.FindByValue(lblSubCat.Text).Selected = true;
            try
            {
                ddlSubCat.Items.FindByValue(lblSubCat.Text).Selected = true;
            }
            catch
            {
                ddlSubCat.Items.FindByValue("2").Selected = true;
            }


            //Activity Type       

            DataTable dtActivity = CWeb.UserListActivityType(Selectedusr, "ActivityType", false);

            DropDownList ddlActType = (e.Item.FindControl("lstProject4") as DropDownList);
            ddlActType.DataSource = dtActivity;
            ddlActType.DataTextField = "code";
            ddlActType.DataValueField = "Id";
            ddlActType.DataBind();
            for (int z = 0; z < ddlActType.Items.Count; z++)
            {
                ddlActType.Items[z].Attributes.Add("title", dtActivity.Rows[z]["descr"].ToString());
            }
            Label lblActType = (e.Item.FindControl("lblPjt4") as Label);
            if (lblActType.Text == "")
                lblActType.Text = "1";
            ddlActType.Items.FindByValue(lblActType.Text).Selected = true;

        }

        protected void lstViewSNOWEntries_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var dataRow = e.Item.DataItem as DataRowView;

                String TotTimeWorkedMin = dataRow["timeworked"].ToString();
                TotalTimeWorkedMin = int.Parse(TotTimeWorkedMin) + TotalTimeWorkedMin;
                StrTotalTimeWorkedMin = TotalTimeWorkedMin.ToString();

                String TotTimeWorkedHrs = dataRow["timeworkedhrs"].ToString();
                TotalTimeWorkedHrs = int.Parse(TotTimeWorkedHrs) + TotalTimeWorkedHrs;
                StrTotalTimeWorkedHrs = TotalTimeWorkedHrs.ToString();

                lblTotalTimeMin.Text = StrTotalTimeWorkedMin;
                lblTotalTimeHrs.Text = StrTotalTimeWorkedHrs;


            }
        }
        #endregion

        #region User Event Handlers

        protected void txtsdate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                StrMessage.Text = "";
                CUserInfo usr = new CUserInfo(lstUser.SelectedValue);
                usr.GetUserInfo(lstUser.SelectedValue, Session.SessionID);
                lblSNOW.Text = "Service Now Entries";
                PageEntriesSetup(usr, DateTime.Parse(txtsdate.Text));
            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Dropdown Date Change", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        private void DisplaySNOWEntries(DateTime EntryDate)
        {
            CUserInfo usr = new CUserInfo(lstUser.SelectedValue);
            usr.GetUserInfo(lstUser.SelectedValue, Session.SessionID);
            DataTable EntryList = CWeb.UserViewSNOWEntries(usr, usr.UserID, EntryDate.ToString());


            if (EntryList != null)
            {
                lblSNOW.Text = "Service Now Entries";
                lblSNOWRecords.Text = "";
                lstViewSNOWEntries.DataSource = EntryList;
                lstViewSNOWEntries.DataBind();
            }
            else
            {
                lstViewSNOWEntries.DataSource = null;
                lstViewSNOWEntries.DataBind();
                //lblSNOWRecords.Text = "No SNOW Records available";
                lblSNOW.Text = "";
                lblTotalTimeHrs.Text = "";
                lblTotalTimeMin.Text = "";
            }
        }
        #endregion

        #region User Event Handlers       

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            int Save = 0;
            int Allocatedper = 0;
            try
            {
                foreach (ListViewItem row in ListView1.Items)
                {
                    switch (row.ItemType)
                    {
                        case ListViewItemType.DataItem:
                            string ProjectPer = ((TextBox)row.FindControl("txtAllocatedPer")).Text;
                            Allocatedper = int.Parse(ProjectPer) + Allocatedper;
                            break;
                    }
                }
                if (Allocatedper != 100)
                    Save = 1;

                if (Save == 0)
                {
                    CWeb.EntrySave(LoggedInUser, txtsdate, ListView1, Page.Title);
                    StrMessage.Text = "Entries saved successfully";
                    StrMessage.ForeColor = System.Drawing.Color.Green;
                    StrMessage.Font.Bold = true;
                }
                else
                {
                    StrMessage.Text = "Allocated percentage should be 100 % ";
                    StrMessage.ForeColor = System.Drawing.Color.Red;
                    txtTotalPer.Text = "";
                }
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
                CLog.WebEvent("PageSave Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Saving", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        protected void lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                StrMessage.Text = "";
                CUserInfo usr = new CUserInfo(lstUser.SelectedValue);
                usr.GetUserInfo(lstUser.SelectedValue, Session.SessionID);
                lblSNOW.Text = "Service Now Entries";
                PageEntriesSetup(usr, DateTime.Parse(txtsdate.Text));

                if(lstUser.SelectedIndex == 0)
                {
                    ListView1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CLog.Error(String.Format("Error in Dropdown Date Change: Page {0} : User {1} : Error {2}", Page.Title, LoggedInUser.LoginAlias, ex.Message));
                CLog.Exception(Page.Title, ex);
            }
        }

        protected void firstTime_Load(object sender, EventArgs e)
        {
            if (dataFirstTime.Value == "yes")
            {
                dataFirstTime.Value = "no";                
            }
        }

        protected string StrTotalTimeWorkedMin
        {
            get { return ViewState["StrTotalTimeWorkedMin"] as string; }
            set { ViewState["StrTotalTimeWorkedMin"] = value; }
        }


        protected string StrTotalTimeWorkedHrs
        {
            get { return ViewState["StrTotalTimeWorkedHrs"] as string; }
            set { ViewState["StrTotalTimeWorkedHrs"] = value; }
        }

        #endregion
    }
}
