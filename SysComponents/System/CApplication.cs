#region References
using System;
using System.Configuration;
using System.Reflection;
#endregion References
namespace SysComponents
{
    public struct CSystemGlobals
    {
        public struct Defaults
        {
            public const string sWebAppCode = "TrackItSW";                  // Web Application Code
            public const string sWebAppName = "TrackIt 2.0 for Software";   // Web Application Name
            public const string sWebAppTitle = "TrackIt 2.0 !";             // Web Application Title
            public const string sWebServerName = "CA1GOPDBQA";              // Web Server 
            public const string sWebServerAlias = "CA1GOPDBQA";             // Web Server Alias
            public const string sVersion = "2.0";

            public const string sLogFilePath = @"C:\TrackItWeb\Logs";
            public const string sDatFilePath = @"C:\TrackItWeb\Data";
            //public const string sLogFilePath = @"F:\TrackItSoftwareTeams\Logs";
            //public const string sDatFilePath = @"F:\TrackItSoftwareTeams\Data";
            public const string sDBDevConnection = @"Server=CA1GOPDBQA;Database=TrackItSWQA;User Id=sa;Password=Pass@123;";
            public const string sDBQAConnection = @"Server=CA1GOPDBQA;Database=TrackItSWQA;User Id=sa;Password=Pass@123;";
            public const string sDBPrdConnection = @"Server=CA1GOPDBQA;Database=TrackItSWQA;User Id=sa;Password=Pass@123;";

            public const int iCommandTimeout = 60;

            public const bool CodeMethodLogging = true;
            public const bool DBCallsLogging = true;
            public const bool DBResultsLogging = false;

            public const string FilePattern = "*";
            public const string FileBackupExt = "BAK";
            public const string FileErrorExt = "ERR";
        }
 

        public struct ErrorMessages
        {
            public const string errAppInit = "Application Initialization Error.";
            public const string errServiceInit = "Error while Starting Service.";
            public const string errRegistryRead = "Error Reading Registry.";
            public const string errLogFileInit = "Error Initializing Log File.";
            public const string errConfigDBRead = "Error Reading System Configuration From Database.";
            public const string errConfigFileRead = "Error reading Configuration from Config file";
            public const string errUserSessionInvalid = "User Information can not be validated in the database";
        }   
        public struct Messages
        {
            public const string sAppInit = "Application Initialized.";
            public const string sRegistryRead = "Registry Reading Complete.";
            public const string sLogFileInit = "Log File Initialized.";
            public const string sConfigDBRead = "Congiguration Values read from Database.";
            public const string sConfigFileRead = "Configuration Values read from Config file";
            public const string sAppEnd = "Application Shutdown.";
            public const string sServiceInit = "Service Started.";
            public const string sServiceEnd = "Service Stopped.";

        }
    }
    public static class CApplication
    {
        # region Data
        public static string    sApplicationCode    = CSystemGlobals.Defaults.sWebAppCode;
        public static string    sApplicationName    = CSystemGlobals.Defaults.sWebAppName;
        public static string    sApplicationTitle   = CSystemGlobals.Defaults.sWebAppTitle;
        public static string    sVersion            = CSystemGlobals.Defaults.sVersion;
        public static string    WebServerName       = CSystemGlobals.Defaults.sWebServerName;
        public static string    sWebServerAlias     = CSystemGlobals.Defaults.sWebServerAlias;

        public static string    sLogFilePath        = @CSystemGlobals.Defaults.sLogFilePath;
        public static string    sDatFilePath        = @CSystemGlobals.Defaults.sDatFilePath;

        public static bool      CodeMethodLogging   = CSystemGlobals.Defaults.CodeMethodLogging;
        public static bool      DBCallsLogging      = CSystemGlobals.Defaults.DBCallsLogging;
        public static bool      DBResultsLogging    = CSystemGlobals.Defaults.DBResultsLogging;

        public static string    DBConnectionString  = CSystemGlobals.Defaults.sDBQAConnection;
        public static int       iCommandTimeout     = CSystemGlobals.Defaults.iCommandTimeout;

        # endregion Data

        # region Methods

        public static bool Start(string WebServer)
        {
            bool Success = false;
            try
            {
                CApplication.WebServerName = WebServer;
                string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
                if (!CApplication.ReadConfig()) RaiseException(CSystemGlobals.ErrorMessages.errConfigFileRead, Module);
                if (!CLog.Init(sApplicationCode)) RaiseException(CSystemGlobals.ErrorMessages.errLogFileInit, Module);
                CLog.Information(Module, CSystemGlobals.Messages.sAppInit);
                CLog.Information(Module, CSystemGlobals.Messages.sLogFileInit);
                CLog.Information(Module, CSystemGlobals.Messages.sConfigFileRead);
                DB.LogEvent(string.Format("Web Service Request @  {0}", CApplication.WebServerName), "Application Start", CApplication.WebServerName, Module);

                Success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(CSystemGlobals.ErrorMessages.errAppInit, ex);
            }
            return Success;
        }
        public static bool RaiseException(string ErrorMessage, string Source)
        {
            Exception ex = new Exception(ErrorMessage);
            ex.Source = string.IsNullOrEmpty(Source)?"CApplication.RaiseException": Source ;
            throw ex;
        }
       
        public static void End()
        {
            try
            {
                string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
                DB.LogEvent(string.Format("Web Service Request @  {0}", CApplication.WebServerName), "Application End", CApplication.WebServerName, Module);
            }
            catch (Exception ex)
            {
                throw new Exception(CSystemGlobals.ErrorMessages.errAppInit, ex);
            }
        }
        public static void Close() { End(); }
        public static void Stop() { End(); }
        public static bool ReadConfig()
        {
            bool Success = false;
            try
            {
                System.Collections.Specialized.NameValueCollection configFile = ConfigurationManager.AppSettings;
                if (configFile.HasKeys())
                {

                    CApplication.sApplicationCode = CUtils.CString(configFile.Get("AppCode"), sApplicationCode);
                    CApplication.sApplicationName = CUtils.CString(configFile.Get("AppName"), sApplicationName);
                    CApplication.sApplicationTitle = CUtils.CString(configFile.Get("AppTitle"), sApplicationTitle);
                    CApplication.sLogFilePath = @CUtils.CString(configFile.Get("LogFilePath"), sLogFilePath);
                    CApplication.sDatFilePath = @CUtils.CString(configFile.Get("DatFilePath"), sDatFilePath);
                    CApplication.sVersion = CUtils.CString(configFile.Get("Version"), sVersion);

                    CApplication.iCommandTimeout = CUtils.CInt(configFile.Get("CommandTimeout"), iCommandTimeout);
                    CApplication.CodeMethodLogging = CUtils.CBool(configFile.Get("CodeMethodLogging"), CodeMethodLogging);
                    CApplication.DBCallsLogging = CUtils.CBool(configFile.Get("DBCallsLogging"), DBCallsLogging);
                    CApplication.DBResultsLogging = CUtils.CBool(configFile.Get("DBResultsLogging"), DBResultsLogging);

                    CApplication.DBConnectionString = @CUtils.CString(configFile.Get("ConnectionString"), CApplication.DBConnectionString);
                    // TODO Process based on Web Server Name
                }
                Success = true;
            }
            catch
            {
                Success = false;
            }
            return Success;
        }
        #endregion "Methods"
    }
}
