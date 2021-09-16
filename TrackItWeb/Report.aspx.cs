using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace TrackItWeb
{
    public partial class Report : Page
    {
        public CUserInfo LoggedInUser;
        public String
         logMsgPageInfo = "",
         logMsgPageData = "",
         pageTitle = "Reports Page",
         userMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack) PageSetup();
        }


        #region Page Methods
        public void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                ddlProject.DataSource = (DataTable)Page.Session["UserProjects"];
                ddlProject.DataTextField = "code";
                ddlProject.DataValueField = "id";
                ddlProject.DataBind();
                ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                ddlProject.SelectedValue = "0";

                DataTable UserList = CWeb.UserListHierarchy(LoggedInUser);
                ddlPeople.DataSource = UserList.AsEnumerable()
                    .Select(row => new { UserName = row.Field<string>("empName"), UserId = row.Field<int>("UserId") })
                    .Distinct()
                    .OrderBy(row => row.UserName);
                ddlPeople.DataTextField = "UserName";
                ddlPeople.DataValueField = "UserId";
                ddlPeople.DataBind();
                ddlPeople.Items.Insert(0, new ListItem("Select", "0"));
                ddlPeople.SelectedValue = "0";

                ddlDepartment.DataSource = UserList.AsEnumerable()
                    .Where(r => !(String.IsNullOrEmpty(r.Field<string>("cchlvl1"))))
                    .Select(row => new { Department = row.Field<string>("cchlvl1") })
                    .Distinct()
                    .OrderBy(row => row.Department);
                ddlDepartment.DataTextField = "Department";
                ddlDepartment.DataValueField = "Department";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("Select", "NA"));
                ddlDepartment.SelectedValue = "NA";

                ddlStatus.Items.Insert(0, new ListItem("Select", "NA"));
                ddlStatus.Items.Insert(1, new ListItem("Active", "Active"));
                ddlStatus.Items.Insert(2, new ListItem("Inactive", "Inactive"));
                ddlStatus.SelectedValue = "NA";

                ddlType.DataSource = UserList.AsEnumerable()
                  .Where(r => !(String.IsNullOrEmpty(r.Field<string>("emp_type"))))
                  .Select(row => new { EmpType = row.Field<string>("emp_type") })
                  .Distinct()
                  .OrderBy(row => row.EmpType);
                ddlType.DataTextField = "EmpType";
                ddlType.DataValueField = "EmpType";
                ddlType.DataBind();
                ddlType.Items.Insert(0, new ListItem("Select", "NA"));
                ddlType.SelectedValue = "NA";

                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }
        #endregion


        #region User Event Handlers
        protected void cmdFilter_Click(object sender, EventArgs e)
        {
            String LogMessage = "";
            lblErrorMsgReport.Text = "";
            try
            {
                CLog.WebEvent("Report Filter Start", Page.Title, LoggedInUser);
                int
                    FilteredUser = CUtils.CInt(ddlPeople.SelectedValue, 0),
                    FilteredProject = CUtils.CInt(ddlProject.SelectedValue, 0);
                String
                    FilteredDepartment = ddlDepartment.SelectedValue.Trim() == "NA" ? "" : ddlDepartment.SelectedValue.Trim(),
                    FilteredEmpType = ddlType.SelectedValue.Trim() == "NA" ? "" : ddlType.SelectedValue.Trim(),
                    FilteredEmpState = ddlStatus.SelectedValue.Trim() == "NA" ? "" : ddlStatus.SelectedValue.Trim();

                DataTable dt = CWeb.UserReportEntries(LoggedInUser, FilteredUser, FilteredProject, txtsdate.Text, txtedate.Text, FilteredDepartment, FilteredEmpState, FilteredEmpType);
                ViewState["FilteredResult"] = dt;
                lstReports.DataSource = dt;
                lstReports.DataBind();
                LogMessage = String.Format("Login {0} Page {1} User {2} Project {3} Dept {4} From {5} To {6} ", LoggedInUser.LoginAlias, Page.Title, FilteredUser, FilteredProject, FilteredDepartment, txtsdate.Text, txtedate.Text);
                CLog.Information(String.Format("Report Generated {0}", LogMessage));
                if (dt == null)
                {

                    lblErrorMsgReport.Text = "No records for the selected filter";
                    lblErrorMsgReport.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                CLog.WebError("Error in Report Filter", Page.Title, LoggedInUser, ex);
            }
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            CLog.WebEvent("Clear Filter Start", Page.Title, LoggedInUser);
            txtedate.Text = "";
            txtsdate.Text = "";
            ddlProject.SelectedValue = "0";
            ddlPeople.SelectedValue = "0";
            ddlDepartment.SelectedValue = "NA";
            ddlType.SelectedValue = "NA";
            ddlStatus.SelectedValue = "NA";
            ViewState["FilteredResult"] = null;
            lstReports.DataBind();
            CLog.WebEvent("Clear Filter End", Page.Title, LoggedInUser);
            lblErrorMsgReport.Text = "";
        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                CLog.WebEvent("Export Start", Page.Title, LoggedInUser);
                DataTable dt = (DataTable)ViewState["FilteredResult"];
                // Remove unwanted columns from the datatable
                dt.Columns.Remove("EntryId");
                dt.Columns.Remove("ProjectId");
                dt.Columns.Remove("ProjectPeriod");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("username");
                dt.Columns.Remove("descr");
                dt.Columns.Remove("CategoryId");
                dt.Columns.Remove("CategoryDescr");
                dt.Columns.Remove("SubCategoryId");
                dt.Columns.Remove("SubCategoryDescr");
                dt.Columns.Remove("ActivityTypeId");
                dt.Columns.Remove("ActivityTypeDescr");
                dt.Columns.Remove("srt");

                //reorder columns in the datatable as per expected in the report
                dt.Columns["empname"].SetOrdinal(0);
                dt.Columns["cchlvl1"].SetOrdinal(1);
                dt.Columns["emp_type"].SetOrdinal(2);
                dt.Columns["empstate"].SetOrdinal(3);
                dt.Columns["EntryDate"].SetOrdinal(4);
                dt.Columns["code"].SetOrdinal(5);
                dt.Columns["CategoryCode"].SetOrdinal(6);
                dt.Columns["SubCategoryCode"].SetOrdinal(7);
                dt.Columns["ActivityTypeCode"].SetOrdinal(8);
                dt.Columns["Allocated"].SetOrdinal(9);

                //change the name of the columns
                dt.Columns["empname"].ColumnName = "User";
                dt.Columns["emp_type"].ColumnName = "Type";
                dt.Columns["empstate"].ColumnName = "State";
                dt.Columns["code"].ColumnName = "Project";
                dt.Columns["CategoryCode"].ColumnName = "Category";
                dt.Columns["SubCategoryCode"].ColumnName = "Sub Category";
                dt.Columns["ActivityTypeCode"].ColumnName = "Activity Type";
                dt.Columns["cchlvl1"].ColumnName = "Department";

                //Call Export to Excel function passing the datatable                
                StringBuilder sb = new StringBuilder();
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                        string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=Entries.csv");
                Response.Charset = "";
                Response.Write(sb.ToString());
                Response.End();
                CLog.WebEvent("Export Completed", Page.Title, LoggedInUser);
            }
            catch (Exception ex)
            {
                if (ex.Message != "Thread was being aborted.")
                    CLog.WebError("Export Error", Page.Title, LoggedInUser, ex);
            }
        }   
        #endregion
    }
}