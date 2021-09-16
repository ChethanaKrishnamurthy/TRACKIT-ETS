using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;



namespace TrackItWeb
{

    public partial class _Default : Page
    {
        public CUserInfo LoggedInUser;
        public String
            logMsgPageInfo = "",
            logMsgPageData = "",
            pageTitle = "Entry Page",
            userMessage = "";
        private int TotalTimeWorkedMin1 = 0;
        private float TotalTimeWorkedHrs1 = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack) PageSetup();
        }



        #region Page Methods

        private void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable dtProjects = CWeb.UserListProjects(LoggedInUser);
                ViewState["ProjectData"] = dtProjects;

                DataTable dtCategory = CWeb.UserListCategory(LoggedInUser, "Category", false);
                ViewState["CategoryData"] = dtCategory;

                DataTable dtSubCategory = CWeb.UserListSubCategory(LoggedInUser, "SubCategory", false);
                ViewState["SubCategoryData"] = dtSubCategory;

                DataTable dtActivity = CWeb.UserListActivityType(LoggedInUser, "ActivityType", false);
                ViewState["ActivityTypeData"] = dtActivity;

                txtsdate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dtGetEntries = CWeb.UserGetEntries(LoggedInUser, LoggedInUser.UserID, txtsdate.Text);
                ViewState["GetEntriesData"] = dtGetEntries;

                // Setting up date for Project Code Info               
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
                txtTotalPer.Text = "100";
                txtImportEntries.Text = "";

                //Display SNOW Entries
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));

                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }

        private void PageEntriesSetup()
        {
            try
            {
                CLog.WebEvent("PageEntriesSetup Start", Page.Title, LoggedInUser);
                DataTable dtGetEntries = CWeb.UserGetEntries(LoggedInUser, LoggedInUser.UserID, txtsdate.Text);

                // Setting up date for Project Code Info               
                DataTable UserProjectList = (DataTable)ViewState["ProjectData"];
                tblProjects.DataSource = UserProjectList;
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
                txtTotalPer.Text = "100";
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
                CLog.WebEvent("PageEntriesSetup Completed", Page.Title, LoggedInUser);

            }
            catch (Exception ex)
            {
                CLog.WebError("PageEntriesSetup Error", Page.Title, LoggedInUser, ex);
            }

        }

        protected void lstViewSNOWEntries_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var dataRow = e.Item.DataItem as DataRowView;

                String TotTimeWorkedMin = dataRow["timeworked"].ToString();
                TotalTimeWorkedMin1 = int.Parse(TotTimeWorkedMin) + TotalTimeWorkedMin1;
                StrTotalTimeWorkedMin1 = TotalTimeWorkedMin1.ToString();

                String TotTimeWorkedHrs = dataRow["timeworkedhrs"].ToString();
                //TotalTimeWorkedHrs1 = int.Parse(TotTimeWorkedHrs) + TotalTimeWorkedHrs1;
                TotalTimeWorkedHrs1 = float.Parse(TotTimeWorkedHrs) + TotalTimeWorkedHrs1;
                StrTotalTimeWorkedHrs1 = TotalTimeWorkedHrs1.ToString();

                lblTotalTimeMin.Text = StrTotalTimeWorkedMin1;
                lblTotalTimeHrs.Text = StrTotalTimeWorkedHrs1;

            }
        }
        private void DisplaySNOWEntries(DateTime EntryDate)
        {
            DataTable EntryList = CWeb.UserViewSNOWEntries(LoggedInUser, LoggedInUser.UserID, EntryDate.ToString());

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
                lblSNOW.Text = "";
                lblTotalTimeHrs.Text = "";
                lblTotalTimeMin.Text = "";
            }
        }

        protected void OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
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

            DataTable dtCategory = (DataTable)ViewState["CategoryData"];
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

            DataTable dtSubCategory = (DataTable)ViewState["SubCategoryData"];
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

            DataTable dtActivity = (DataTable)ViewState["ActivityTypeData"];
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

        protected void txtsdate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                StrMessage.Text = "";
                txtImportEntries.Text = "";
                PageEntriesSetup();
            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Dropdown Date Change", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        protected void txtImportEntries_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Dropdown Date Change", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
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
                CLog.WebEvent("PageSave Start", Page.Title, LoggedInUser);
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
                }
               
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
                CLog.WebEvent("PageSave Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Saving", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        protected void cmdImportEntries_Click(object sender, EventArgs e)
        {
            try
            {
                CLog.WebEvent("Import Entries Start", Page.Title, LoggedInUser);
                DataTable dtGetEntries = CWeb.UserGetEntries(LoggedInUser, LoggedInUser.UserID, txtImportEntries.Text);

                // Setting up date for Project Code Info               
                DataTable UserProjectList = (DataTable)ViewState["ProjectData"];
                tblProjects.DataSource = UserProjectList;
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
                txtTotalPer.Text = "100";
                StrMessage.Text = "";
                DisplaySNOWEntries(DateTime.Parse(txtsdate.Text));
                CLog.WebEvent("Import Entries End", Page.Title, LoggedInUser);

            }
            catch (Exception ex)
            {
                WebLog.Log(LoggedInUser.LoginAlias, Page.Title, "Error in Import Entries", "InnerException:" + ex.InnerException + " Stack Trace: " + ex.StackTrace);
            }
        }

        protected void firstTime_Load(object sender, EventArgs e)
        {
            if (dataFirstTime.Value == "yes")
            {
                dataFirstTime.Value = "no";
            }
        }

        protected string StrTotalTimeWorkedMin1
        {
            get { return ViewState["StrTotalTimeWorkedMin1"] as string; }
            set { ViewState["StrTotalTimeWorkedMin1"] = value; }
        }

        protected string StrTotalTimeWorkedHrs1
        {
            get { return ViewState["StrTotalTimeWorkedHrs1"] as string; }
            set { ViewState["StrTotalTimeWorkedHrs1"] = value; }
        }
    }
}
#endregion