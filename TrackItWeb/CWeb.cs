using SysComponents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace TrackItWeb
{
    public class CWeb
    {
        public static void UserLogin(System.Web.SessionState.HttpSessionState Session, string LoginID, HttpRequest Request)
        {
            LoginID = LoginID.Replace("RMS\\", "");
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CLog.Information("Session Start", string.Format("ID:{0}:User:{1}:From:{2} : UrlReferrer--{3} : QuerystringCount --{4} : UrlHostName--{5} : Browser -- {6}  ", Session.SessionID, LoginID, Request.Url, (Request.UrlReferrer == null) ? "No Data" : Request.UrlReferrer.ToString(), Request.QueryString.Count.ToString(), Request.UserHostName, Request.Browser.Browser));
            CUserInfo usr = new CUserInfo(LoginID);
            usr.GetUserInfo(LoginID, Session.SessionID);
            if (!usr.IsValidUser) CApplication.RaiseException(usr.ValidationMessage, Module);
            Session["UserData"] = usr;
            Session["UserProjects"] = UserListProjects(usr);
        }
        public static void UserLogout(System.Web.SessionState.HttpSessionState Session)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            string LoginID = ((CUserInfo)Session["UserData"]).LoginAlias;
            CLog.Information("Session Close", String.Format("ID:{0}:User:{1}", Session.SessionID, LoginID));
            DB.LogEvent(string.Format("User {0} Session {1}", LoginID, Session.SessionID), "User Logout", CApplication.WebServerName, Module);
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
        }
        public static void NotAuthorizedRedirect(CUserInfo loggedInUser, string PageTitle, HttpResponse response)
        {
            CLog.Information("Not Authorized", String.Format("User:{0} Name:{1} Page:{2}", loggedInUser.LoginAlias, loggedInUser.FullName, PageTitle));
            response.Redirect("NotAuthenticated.html");
        }
        public static DataTable UserListProjects(CUserInfo loggedInUser)
        {
            CData results = DB.UserListProjects(loggedInUser.LoginAlias);
            return results.sqlData;
        }

        public static DataTable UserListCategory(CUserInfo loggedInUser, string CodeType, bool IsAdmin)
        {
            CData results = DB.UserListProjectTypes(loggedInUser.LoginAlias, CodeType, IsAdmin);
            return results.sqlData;
        }

        public static DataTable UserListSubCategory(CUserInfo loggedInUser, string CodeType, bool IsAdmin)
        {
            CData results = DB.UserListProjectTypes(loggedInUser.LoginAlias, CodeType, IsAdmin);
            return results.sqlData;
        }

        public static DataTable UserListActivityType(CUserInfo loggedInUser, string CodeType, bool IsAdmin)
        {
            CData results = DB.UserListProjectTypes(loggedInUser.LoginAlias, CodeType, IsAdmin);
            return results.sqlData;
        }

        public static DataTable UserAdminListDepartmentProjectTypes(CUserInfo loggedInUser, int DeptId, string ProjectType)
        {
            CData results = DB.UserAdminListDepartmentProjectTypes(loggedInUser.LoginAlias, DeptId, ProjectType);
            return results.sqlData;
        }

        public static DataTable AdminUserGetEntries (CUserInfo loggedInUser, int UserId, String ProjectPeriod)
        {
            CData results = DB.AdminUserGetEntries(loggedInUser.LoginAlias, UserId, ProjectPeriod);           
            return results.sqlData;
        }
        
        public static DataTable UserGetEntries(CUserInfo loggedInUser, int UserId, String ProjectPeriod)
        {
            CData results = DB.UserGetEntries(loggedInUser.LoginAlias, UserId, ProjectPeriod);
            return results.sqlData;
        }

        /*public static bool EntrySave(CUserInfo userInfo, TextBox ProjectPeriod, ListView UserEntryList, String PageTitle)
        {
            DateTime date = Convert.ToDateTime(ProjectPeriod.Text.ToString());
            List<String> UpdatedIdList = new List<String>(UpdatedIds.Split(',').Distinct());

            WebLog.Log(userInfo.LoginAlias, PageTitle, "After Save Before DB Transaction", "UpdatedIds are " + UpdatedIds);
            if (UpdatedIdList.Count == 0)
            {
                return true;
            }

            String JSONDataToInsert = String.Empty;
            JSONDataToInsert += "[";

            foreach (ListViewItem item in UserEntryList.Items)
            {
                String cmt = ((TextBox)item.FindControl("txtComment")).Text;
                cmt = cmt.Replace("\\", " ");
                cmt = cmt.Replace(":", " ");
                cmt = cmt.Replace("\"", "\\\"");

                //Form the JSON String that Needs to be Send to the Stored procedure                
                JSONDataToInsert += "{" +
                        " \"UserId\": \"" + ((Label)item.FindControl("lblEntryUserId")).Text + "\" ," +
                        " \"ProjectPeriod\" :\"" + date.ToString("MM/dd/yyyy") + "\"," +
                        " \"ProjectId\" :\"" + ((DropDownList)item.FindControl("lstProject1")).SelectedValue + "\" ," +                        
                        " \"CategoryId\" :\"" + ((DropDownList)item.FindControl("lstProject2")).SelectedValue + "\" ," +
                        " \"SubCategoryId\" :\"" + ((DropDownList)item.FindControl("lstProject3")).SelectedValue + "\" ," +
                        " \"ActivityTypeId\" :\"" + ((DropDownList)item.FindControl("lstProject4")).SelectedValue + "\" ," +
                        " \"Comment\" :\"" + cmt + "\" ," +
                        //" \"Comment\" :\"" + ((TextBox)item.FindControl("txtComment")).Text + "\" ," +  
                        " \"ProjectPer\" :\"" + ((TextBox)item.FindControl("txtAllocatedPer")).Text + "\" " +
                       "},";               
            }
            JSONDataToInsert = JSONDataToInsert.TrimEnd(',');
            JSONDataToInsert += "]";            
            SysComponents.DB.UserSaveEntries(userInfo.LoginAlias, JSONDataToInsert);
            WebLog.Log(userInfo.LoginAlias, PageTitle, "After Save", "Records Saved are " + JSONDataToInsert);           
            return true;
        }*/

        public class UserEntryItem
        {
            public int UserId;
            public string ProjectPeriod;
            public int ProjectId;
            public int CategoryId;
            public int SubCategoryId;
            public int ActivityTypeId;
            public string Comment;
            public int ProjectPer;


            public UserEntryItem(string entryPeriod, int userId, int projectId, int categoryId, int subCategoryId, int activityTypeId, int projectPer, string comment)
            {
                ProjectPeriod = entryPeriod;
                UserId = userId;
                ProjectId = projectId;
                CategoryId = categoryId;
                SubCategoryId = subCategoryId;
                ActivityTypeId = activityTypeId;
                ProjectPer = projectPer;
                Comment = comment;
            }

        }

        public static bool EntrySave(CUserInfo userInfo, TextBox ProjectPeriod, ListView UserEntryList, String PageTitle)
        {
            string
                entryPeriod = Convert.ToDateTime(ProjectPeriod.Text.ToString()).ToString("MM/dd/yyyy")
                , jsonForSaving = "";
            List<UserEntryItem> sData = new List<UserEntryItem>();
            foreach (ListViewItem item in UserEntryList.Items)
            {
                int UserId = CUtils.CInt(((Label)item.FindControl("lblEntryUserId")).Text, 0)
                    , ProjectId = CUtils.CInt(((DropDownList)item.FindControl("lstProject1")).Text, 0)
                    , CategoryId = CUtils.CInt(((DropDownList)item.FindControl("lstProject2")).Text, 0)
                    , SubCategoryId = CUtils.CInt(((DropDownList)item.FindControl("lstProject3")).Text, 0)
                    , ActivityTypeId = CUtils.CInt(((DropDownList)item.FindControl("lstProject4")).Text, 0)
                    , ProjectPer = CUtils.CInt(((TextBox)item.FindControl("txtAllocatedPer")).Text, 0);
                string Comment = ((TextBox)item.FindControl("txtComment")).Text;
                sData.Add(new UserEntryItem(entryPeriod, UserId, ProjectId, CategoryId, SubCategoryId, ActivityTypeId, ProjectPer, Comment));
            }
            jsonForSaving = JsonConvert.SerializeObject(sData, Formatting.Indented);
            SysComponents.DB.UserSaveEntries(userInfo.LoginAlias, jsonForSaving);
            WebLog.Log(userInfo.LoginAlias, PageTitle, "After Save", "Records Saved are " + jsonForSaving);
            return true;
        }

        public static bool AdminDeptAssociationsSave(CUserInfo userInfo, int DeptId, String ProjectType , String AllowedIDs)
        {
            SysComponents.DB.AdminDeptAssociationsSave(userInfo.LoginAlias, DeptId, ProjectType, AllowedIDs);
            return true;
        }
       
        public static DataTable UserListHierarchy(CUserInfo loggedInUser)
        {
            CData results = DB.UserListHierarchy(loggedInUser.UserID, true, loggedInUser.IsAdministrator,null );
            return results.sqlData;
        }

        public static DataTable ManagerUserListHierarchy(CUserInfo loggedInUser)
        {
            CData results = DB.ManagerUserListHierarchy(loggedInUser.UserID, true, loggedInUser.IsAdministrator, null);
            return results.sqlData;
        }

        public static DataTable UserAdminListDepartments(CUserInfo loggedInUser)
        {
            CData results = DB.UserAdminListDepartments(loggedInUser.LoginAlias);
            return results.sqlData;
        }
        public static DataTable UserReportEntries(CUserInfo loggedInUser, int ReportUserId, int ProjectId, String StartDate, String EndDate, String Department, String EmpState, String EmpType)
        {
            CData results = DB.UserReportEntries(loggedInUser.LoginAlias, ReportUserId, ProjectId, StartDate, EndDate, Department, EmpState, EmpType);
            return results.sqlData;
        }
        public static StringWriter ExportToExcel(DataTable dataTable)
        {
            GridView gridView = new GridView();
            gridView.DataSource = dataTable;
            gridView.AutoGenerateColumns = true;
            gridView.DataBind();

            StringWriter writer = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);
            htmlWriter.Write(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Report</title>");
            htmlWriter.Write(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            htmlWriter.Write("<DIV  style='font-size:12px;'>");                    
            htmlWriter.WriteLine();
            gridView.RenderControl(htmlWriter);
            htmlWriter.Close();
            return writer;
        }
        public static DataTable UserViewEntries(CUserInfo loggedInUser, int ReportUserId, String ReportPeriod)
        {
            CData results = DB.UserViewEntries(loggedInUser.LoginAlias, ReportUserId, ReportPeriod);
            return results.sqlData;
        }

        public static DataTable UserViewSNOWEntries(CUserInfo loggedInUser, int ReportUserId, String ReportPeriod)
        {
            CData results = DB.UserViewSNOWEntries(loggedInUser.LoginAlias, ReportUserId, ReportPeriod);
            return results.sqlData;
        }

        public static DataTable UserViewEntriesByTime(CUserInfo loggedInUser, int ReportUserId, String ReportPeriod)
        {
            CData results = DB.UserViewEntries(loggedInUser.LoginAlias, ReportUserId, ReportPeriod);
            return results.sqlData;
        }
        
        public static DataTable UserAdminReportUsers(CUserInfo loggedInUser, String Name, String Department, String ManagerName, String EmployeeType, String Status)
        {
            CData results = DB.UserAdminReportUsers(loggedInUser.LoginAlias, Name, Department, ManagerName, EmployeeType, Status);
            return results.sqlData;
        }
        public static DataTable UserAdminListUsers(CUserInfo loggedInUser)
        {
            CData results = DB.UserAdminReportUsers(loggedInUser.LoginAlias, "", "", "","","");
            return results.sqlData;
        }
       
        public static void UserAdminUpdateAdminStatus(string UserAlias, bool IsAdmin)
        {
            DB.UserAdminUpdate(UserAlias, IsAdmin);
        }

        public static void AuthorizedUserUpdateStatus(string UserAlias, bool @IsAuthUser)
        {
            DB.AuthorizedUserUpdate(UserAlias, IsAuthUser);
        }
        public static DataTable UserAdminListProjects(CUserInfo loggedInUser)
        {
            CData results = DB.UserAdminListProjects(loggedInUser.LoginAlias, loggedInUser.UserID);
            return results.sqlData;
        }
        public static void UserAdminSaveProjects(CUserInfo loggedInUser,int ProjectId, String ProjectCode, String ProjectDescription, string SubProduct)
        {
            DB.UserAdminSaveProjects(loggedInUser.LoginAlias, loggedInUser.UserID,ProjectId, ProjectCode, ProjectDescription, SubProduct);
        }
    }
}