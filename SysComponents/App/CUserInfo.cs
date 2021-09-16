#region References
using System;
using System.Reflection;
#endregion References

namespace SysComponents
{
    public class CUserInfo
    {
        # region Data
        public int UserID;
        public string UserRowID;
        public string FullName;
        public string WorkerType;
        public string UserStatus;
        public string ManagerLoginAlias;
        public string ManagerFullName;
        public string DepartmentCode;
        public string DepartmentName;
        public string GroupName;
        public string cchlvl1;
        public string cchlvl4;
        public string cchlvl5;
        public bool IsAdministrator;
        public bool IsGroupAdministrator;
        public bool IsDepartmentAdministrator;
        public bool IsValidUser;
        public bool IsManager;
        public string ValidationMessage;

        public int EntryRange;
        public int DirectReportees;
        public DateTime EntryStartDate;
        public DateTime EntryCurrentDate;
        public int EntryGracePeriod;
        public DateTime LastLoginTime;

        public string LoginAlias;
        public string LoginSessionID;
        public string WebAppTitle;
  
        # endregion Data

        # region Constructors

        public CUserInfo()
        {
            SetEmptyUser();
        }
        public CUserInfo(string LoginID)
        {
            SetEmptyUser();
            this.LoginAlias = LoginID;
         }
        # endregion Constructors

        # region Methods
        public void SetEmptyUser()
        {
            this.UserID =-1;
            this.LoginAlias= "(UNKNOWN)";
            this.LoginSessionID = "(UNKNOWN)";
            this.UserRowID = "(UNKNOWN)";
            this.FullName = "(UNKNOWN)";
            this.WorkerType = "(UNKNOWN)";
            this.UserStatus = "In Active";
            this.ManagerLoginAlias = "";
            this.ManagerFullName = "";
            this.DepartmentCode = "(UNKNOWN)"; 
            this.DepartmentName = "(UNKNOWN)";
            this.GroupName = "(UNKNOWN)";
            this.cchlvl1 = "(UNKNOWN)";
            this.cchlvl4 = "(UNKNOWN)";
            this.cchlvl5 = "(UNKNOWN)";
            this.IsAdministrator =false;
            this.IsGroupAdministrator = false; 
            this.IsDepartmentAdministrator = false; 
            this.IsValidUser = false;
            this.IsManager = false;
            this.ValidationMessage= "Not Authorized for TrackIt SW"; 
            this.EntryRange=0;
            this.DirectReportees = 0;
            this.EntryStartDate=new DateTime(2019,12,01);
            this.EntryCurrentDate = DateTime.Today;
            this.EntryGracePeriod = 7;
            this.LastLoginTime=new DateTime(2019, 12, 01);
            this.WebAppTitle = CApplication.sApplicationTitle;
    }
        public void  GetUserInfo(string LoginId, string SessionID)
        {
            bool UserValidated = false;
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            this.LoginAlias = LoginId;
            this.LoginSessionID = SessionID;
            DB.LogEvent(String.Format("User {0} Session {1}", LoginId, SessionID), "User Login", CApplication.WebServerName, Module);
            using (CData userData = DB.UserGetInfo(this.LoginAlias))
            {
                UserValidated = userData.Success && userData.HasRows;
                if (UserValidated)
                {
                    PopulateDBValues(userData);
                    UserValidated = this.IsValidUser;
                }
            }
        }
        public void GetUserInfo(int UserId, string SessionID)
        {
            bool UserValidated = false;
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            this.UserID = UserId;
            this.LoginSessionID = SessionID;
            using (CData userData = DB.UserGetInfo(UserID))
            {
                UserValidated = userData.Success && userData.HasRows;
                if (UserValidated)
                {
                    PopulateDBValues(userData);
                    UserValidated = this.IsValidUser;
                }
            }
        }

        private void PopulateDBValues(CData userInfo)
        {
            this.UserID = CUtils.CInt(userInfo.sqlData.Rows[0]["Id"], this.UserID);
            this.UserRowID = CUtils.CString(userInfo.sqlData.Rows[0]["rowid"], this.UserRowID);
            this.LoginAlias = CUtils.CString(userInfo.sqlData.Rows[0]["Alias"], this.LoginAlias);
            this.FullName = CUtils.CString(userInfo.sqlData.Rows[0]["Name"], this.FullName);
            this.UserStatus = CUtils.CString(userInfo.sqlData.Rows[0]["Status"], this.UserStatus);
            this.WorkerType = CUtils.CString(userInfo.sqlData.Rows[0]["EmployeeTyp"], this.WorkerType);
            this.ManagerLoginAlias = CUtils.CString(userInfo.sqlData.Rows[0]["MgrAlias"], this.ManagerLoginAlias);
            this.ManagerFullName = CUtils.CString(userInfo.sqlData.Rows[0]["MgrName"], this.ManagerFullName);
            this.DepartmentCode = CUtils.CString(userInfo.sqlData.Rows[0]["department_code"], this.DepartmentCode);
            this.DepartmentName = CUtils.CString(userInfo.sqlData.Rows[0]["department"], this.DepartmentName);
            this.GroupName = CUtils.CString(userInfo.sqlData.Rows[0]["cchlvl5"], this.GroupName);
            this.cchlvl1 = CUtils.CString(userInfo.sqlData.Rows[0]["cchlvl1"], this.cchlvl1);
            this.cchlvl4 = CUtils.CString(userInfo.sqlData.Rows[0]["cchlvl4"], this.cchlvl4);
            this.cchlvl5 = CUtils.CString(userInfo.sqlData.Rows[0]["cchlvl5"], this.cchlvl5);
            this.IsAdministrator = CUtils.CBool(userInfo.sqlData.Rows[0]["IsSysAdmin"], false);
            this.IsGroupAdministrator = CUtils.CBool(userInfo.sqlData.Rows[0]["IsGroupAdmin"], false);
            this.IsDepartmentAdministrator = CUtils.CBool(userInfo.sqlData.Rows[0]["IsDeptAdmin"], false);
            this.IsValidUser = CUtils.CBool(userInfo.sqlData.Rows[0]["IsValidUser"], false);

            this.ValidationMessage = CUtils.CString(userInfo.sqlData.Rows[0]["ValidationMessage"], this.ValidationMessage);

            this.EntryRange = CUtils.CInt(userInfo.sqlData.Rows[0]["EntryRange"], this.EntryRange);
            this.DirectReportees = CUtils.CInt(userInfo.sqlData.Rows[0]["ReportSpan"], this.DirectReportees);
            this.IsManager = (this.DirectReportees > 0);
            this.EntryStartDate = CUtils.CDate(userInfo.sqlData.Rows[0]["EntryStartDate"], this.EntryStartDate);
            this.EntryCurrentDate = CUtils.CDate(userInfo.sqlData.Rows[0]["EntryCurrentDate"], this.EntryCurrentDate);
            this.EntryGracePeriod = CUtils.CInt(userInfo.sqlData.Rows[0]["EntryGracePeriod"], this.EntryGracePeriod);
            this.LastLoginTime = CUtils.CDate(userInfo.sqlData.Rows[0]["LastLoginDate"], this.LastLoginTime);
        }
        # endregion Methods
    }
}