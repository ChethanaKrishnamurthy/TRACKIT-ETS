#region References
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
#endregion References


namespace SysComponents
{

    public partial class DB
    {
        #region Methods
        public static void LogEvent(String Message, String Category, String App, String Method)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.LogEvent");
                    db.SetParameter("@EventMessage", Message, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@EventReference", Category, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@AppReference", App, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@ModuleReference", Method, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            finally
            {
                rs.Close();
            }
        }
        public static CData UserGetInfo(string LoginID)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserGetDetails");
                    db.SetParameter("@LoggedInUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
        public static CData UserGetInfo(int UserId)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserGetDetails");
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserListProjects(string LoginID)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserListProjects");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);                  
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
        public static CData UserListProjectTypes(string LoginID, string CodeType, Boolean IsAdmin)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserListProjectTypes");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@ProjectType", CodeType, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@AdminPage", IsAdmin, SqlDbType.Bit, ParameterDirection.Input, false);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }


        public static CData UserAdminListDepartmentProjectTypes(string LoginID, int DepId, string ProjectType)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminListDepartmentProjectTypes");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@DeptId", DepId, SqlDbType.Int, ParameterDirection.Input, false);
                    db.SetParameter("@ProjectType", ProjectType, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData AdminUserGetEntries(string LoginID, int UserId, String ProjectPeriod)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserGetEntries");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    if (CUtils.IsDate(ProjectPeriod)) db.SetParameter("@ProjectPeriod", Convert.ToDateTime(ProjectPeriod), SqlDbType.Date, ParameterDirection.Input, false);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        
        public static CData UserGetEntries(string LoginID, int UserId, string ProjectPeriod)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserGetEntries");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    if (CUtils.IsDate(ProjectPeriod)) db.SetParameter("@ProjectPeriod", Convert.ToDateTime(ProjectPeriod), SqlDbType.Date, ParameterDirection.Input, false);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserSaveEntries(string LoginID, String JsonData)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserSaveEntries");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@EntryData", JsonData, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData AdminDeptAssociationsSave(string LoginID, int DeptId, String ProjectType, String AllowedIds)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminSaveDepartmentProjectTypes");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@DeptId", DeptId, SqlDbType.Int, ParameterDirection.Input, true);
                    db.SetParameter("@ProjectType", ProjectType, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@AllowedIds", AllowedIds, SqlDbType.VarChar, ParameterDirection.Input, true);                    
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserListHierarchy(int UserId, bool IncludeHierarchy, bool IsAdmin, String FromDate)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserListHierarchy");
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, true);
                    db.SetParameter("@IncludeHierarchy", IncludeHierarchy, SqlDbType.Bit, ParameterDirection.Input, false);
                    db.SetParameter("@IsAdmin", IsAdmin, SqlDbType.Bit, ParameterDirection.Input, false);
                    if (!String.IsNullOrEmpty(FromDate)) db.SetParameter("@FromDate", Convert.ToDateTime(FromDate), SqlDbType.Date, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData ManagerUserListHierarchy(int UserId, bool IncludeHierarchy, bool IsAdmin, String FromDate)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.ManagerListHierarchy");
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, true);
                    db.SetParameter("@IncludeHierarchy", IncludeHierarchy, SqlDbType.Bit, ParameterDirection.Input, false);
                    db.SetParameter("@IsAdmin", IsAdmin, SqlDbType.Bit, ParameterDirection.Input, false);
                    if (!String.IsNullOrEmpty(FromDate)) db.SetParameter("@FromDate", Convert.ToDateTime(FromDate), SqlDbType.Date, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserAdminListDepartments(string LoggedInuser)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminListDepartments");
                    db.SetParameter("@LoginUser", LoggedInuser, SqlDbType.VarChar, ParameterDirection.Input, true);  
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserReportEntries(String LoginId, int ReportUserId, int ProjectId, String StartDate, String EndDate, String Department, String EmpState, String EmpType)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {

                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserReportEntries");
                    db.SetParameter("@LoginUser", LoginId, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (ReportUserId != 0) db.SetParameter("@ReportUserId ", ReportUserId, SqlDbType.Int, ParameterDirection.Input, true);
                    if (ProjectId != 0) db.SetParameter("@ReportProjectId ", ProjectId, SqlDbType.Int, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(Department)) db.SetParameter("@ReportDepartment", Department, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(EmpType)) db.SetParameter("@ReportEmpType", EmpType, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(EmpState)) db.SetParameter("@ReportEmpState", EmpState, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(StartDate)) db.SetParameter("@ReportStartDate", Convert.ToDateTime(StartDate), SqlDbType.Date, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(EndDate)) db.SetParameter("@ReportEndDate", Convert.ToDateTime(EndDate), SqlDbType.Date, ParameterDirection.Input, true);

                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
        public static CData UserViewEntries(string LoginID, int UserId, String ProjectPeriod)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserViewEntries");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@ReportUserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    if (!String.IsNullOrEmpty(ProjectPeriod)) db.SetParameter("@ProjectPeriod", Convert.ToDateTime(ProjectPeriod), SqlDbType.Date, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserViewSNOWEntries(string LoginID, int UserId, String ProjectPeriod)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserSnowGetEntries");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    if (!String.IsNullOrEmpty(ProjectPeriod)) db.SetParameter("@ProjectPeriod", Convert.ToDateTime(ProjectPeriod), SqlDbType.Date, ParameterDirection.Input, true);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        public static CData UserAdminReportUsers(string LoginID, String Name, String Department, String ManagerName, String EmployeeType, String Status)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminReportUsers");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    if (!String.IsNullOrEmpty(Name))  db.SetParameter("@Name", Name, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(Department)) db.SetParameter("@Department", Department, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(ManagerName)) db.SetParameter("@ManagerName", ManagerName, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(EmployeeType)) db.SetParameter("@EmployeeType", EmployeeType, SqlDbType.VarChar, ParameterDirection.Input, true);
                    if (!String.IsNullOrEmpty(Status)) db.SetParameter("@Active", Status, SqlDbType.VarChar, ParameterDirection.Input, true);                   
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
                

        public static CData AuthorizedUserUpdate(string LoginID, bool @IsAuthUser)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.AuthorizedUserUpdate");
                    db.SetParameter("@UserAlias", LoginID, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@IsAuthUser", @IsAuthUser, SqlDbType.Bit, ParameterDirection.Input, false);
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }

        //End CK
        public static CData UserAdminUpdate(string LoginID, bool IsAdmin)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminUpdate");
                    db.SetParameter("@UserAlias", LoginID, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@IsAdmin", IsAdmin, SqlDbType.Bit, ParameterDirection.Input, false);
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
        public static CData UserAdminListProjects(string LoginID, int UserId)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminListProjects");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    db.ExecuteQuery(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }
        public static CData UserAdminSaveProjects(string LoginID, int UserId,int ProjectId, String ProjectCode, String ProjectDescription, String SubProduct)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            CData rs = new CData();
            try
            {
                using (CDBCommand db = new CDBCommand())
                {
                    db.SetProcedure("dbo.UserAdminSaveProjects");
                    db.SetParameter("@LoginUser", LoginID, SqlDbType.VarChar, ParameterDirection.Input, false);
                    db.SetParameter("@UserId", UserId, SqlDbType.Int, ParameterDirection.Input, false);
                    db.SetParameter("@ProjectId ", ProjectId, SqlDbType.Int, ParameterDirection.Input, true);
                    db.SetParameter("@ProjectCode", ProjectCode, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@Description", ProjectDescription, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.SetParameter("@SubProduct", SubProduct, SqlDbType.VarChar, ParameterDirection.Input, true);
                    db.ExecuteCommand(rs);
                    CLog.Error(Module, !rs.Success, string.Format("DB Error @{0}", Module), false);
                }
            }
            catch (SqlException e) { CLog.Exception(Module, e); }
            catch (Exception e) { CLog.Exception(Module, e); }
            return rs;
        }


        #endregion
    }
}
