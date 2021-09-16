using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;
using System.IO;


namespace TrackItWeb
{
    public partial class AddRemoveRoles : Page
    {
        public CUserInfo LoggedInUser;
        public String
            logMsgPageInfo = "",
            logMsgPageData = "",
            pageTitle = "User List Page",
            userMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            if (!LoggedInUser.IsAdministrator) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack) PageSetup(null, null, null, null, null, true);      
        }


        #region Page Methods
        protected void PageSetup(String Name, String Department, String ManagerName, String EmployeeType, String Status, bool RefreshControl)
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DataTable dt = CWeb.UserAdminReportUsers(LoggedInUser, Name, Department, ManagerName, EmployeeType, Status);
                ViewState["FilteredResult"] = dt;
                if (RefreshControl)
                {
                   
                    ddlName.Items.Insert(0, new ListItem("Select", null));
                    ddlName.DataSource = dt.AsEnumerable().Select(x => new { empname = x.Field<string>("empname") }).OrderBy(x => x.empname);
                    ddlName.DataTextField = "empname";
                    ddlName.DataValueField = "empname";
                    ddlName.DataBind();
                    ddlName.Items.Insert(0, new ListItem("Select", null));

                    ddlDepartment.DataSource = dt.AsEnumerable().Where(r => !(String.IsNullOrEmpty(r.Field<string>("department")))).Select(row => new { department = row.Field<string>("department") }).Distinct().OrderBy(x => x.department); ;
                    ddlDepartment.DataTextField = "department";
                    ddlDepartment.DataValueField = "department";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new ListItem("Select", null));


                    ddlManagerName.DataSource = dt.AsEnumerable().Select(row => new { mgr = row.Field<string>("mgr") }).Distinct().OrderBy(x => x.mgr); ;
                    ddlManagerName.DataTextField = "mgr";
                    ddlManagerName.DataValueField = "mgr";
                    ddlManagerName.DataBind();
                    ddlManagerName.Items.Insert(0, new ListItem("Select", null));


                    ddlEmployeeType.DataSource = dt.AsEnumerable().Select(row => new { EmployeeType = row.Field<string>("emp_type") }).Distinct().OrderBy(x => x.EmployeeType);
                    ddlEmployeeType.DataTextField = "EmployeeType";
                    ddlEmployeeType.DataValueField = "EmployeeType";
                    ddlEmployeeType.DataBind();
                    ddlEmployeeType.Items.Insert(0, new ListItem("Select", null));


                    ddlStatus.DataSource = dt.AsEnumerable().Select(row => new { Status = row.Field<string>("empstate") }).Distinct().OrderBy(x => x.Status);
                    ddlStatus.DataTextField = "Status";
                    ddlStatus.DataValueField = "Status";
                    ddlStatus.DataBind();
                    ddlStatus.Items.Insert(0, new ListItem("Select", null));

                    ddlName.SelectedIndex = 0;                  
                    ddlDepartment.SelectedIndex = 0;
                    ddlManagerName.SelectedIndex = 0;
                    ddlEmployeeType.SelectedIndex = 0;
                    ddlStatus.SelectedIndex = 0;                    
                }
                if (dt != null)
                {
                    lstAllUsers.DataSource = dt.AsEnumerable()
                            .Select(r => new
                            {
                                empname = r.Field<string>("empname"),
                                emp_id = r.Field<string>("emp_id"),
                                department = r.Field<string>("department"),
                                mgr = r.Field<string>("mgr"),
                                emp_type = r.Field<string>("emp_type"),
                                empstate = r.Field<String>("empstate")                                
                            })
                            .OrderBy(r => r.empname);
                    lblErrorMsg.Text = "";

                    CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);
                }
                else
                {
                    lblErrorMsg.Text = "No matching records for this selection";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    ViewState["FilteredResult"] = null;
                }
                lstAllUsers.DataBind();
                
            }
            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }

        }
        #endregion

        #region User Event Handlers
        protected void ddlFilterChanged(object sender, EventArgs e)
        {
            
            PageSetup(ddlName.SelectedValue == "Select" ? null : ddlName.SelectedValue                        
                        , ddlDepartment.SelectedValue == "Select" ? null : ddlDepartment.SelectedValue
                        , ddlManagerName.SelectedValue == "Select" ? null : ddlManagerName.SelectedValue
                        , ddlEmployeeType.SelectedValue == "Select" ? null : ddlEmployeeType.SelectedValue
                        , ddlStatus.SelectedValue == "Select" ? null : ddlStatus.SelectedValue                       
                        , false);
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            PageSetup(null, null, null, null, null, true);
            ViewState["FilteredResult"] = null;
        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                CLog.WebEvent("Export Start", Page.Title, LoggedInUser);
                DataTable dt = (DataTable)ViewState["FilteredResult"];
                // Remove unwanted columns from the datatable
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("UserName");
                dt.Columns.Remove("level");
                dt.Columns.Remove("srt");
                dt.Columns.Remove("cchlvl1");
                dt.Columns.Remove("cchlvl4");
                dt.Columns.Remove("cchlvl5");
                dt.Columns.Remove("UserId1");
                dt.Columns.Remove("rowid");
                dt.Columns.Remove("IsAdmin");

                //reorder columns in the datatable as per expected in the report
                dt.Columns["empname"].SetOrdinal(0);
                dt.Columns["emp_id"].SetOrdinal(1);
                dt.Columns["department"].SetOrdinal(2);
                dt.Columns["mgr"].SetOrdinal(3);
                dt.Columns["emp_type"].SetOrdinal(4);
                dt.Columns["empstate"].SetOrdinal(5);                

                //Call Export to Excel function passing the datatable
                StringWriter writer = CWeb.ExportToExcel(dt);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=User List.xls");
                Response.Charset = "";
                Response.Write(writer.ToString());
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