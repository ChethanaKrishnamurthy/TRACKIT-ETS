#region References
using System;
using System.Diagnostics;
using System.Reflection;
#endregion References

namespace SysComponents
{
    public static class CLog
    {
        # region Data
        public static Microsoft.VisualBasic.Logging.FileLogTraceListener LogFile = null;
        public static Microsoft.VisualBasic.Logging.FileLogTraceListener LogFileEx = null;
        public static string    sLogFormat = "{1:s}{0}{2}{0}{3}{0}{4}{0}{5}"; // {0}-Delimiter {1}-Time {2}-Application Name {3}-Log Type {4}-Module {5}-Message
        public static string    sLogDelimiter = "~";
        public static string    sLogCodeDebug = "DG";
        public static string    sLogCodeError = "ER";
        public static string    sLogCodeInfo = "IN";
        public static string    sLogCodeWarn = "WA";
        public static string    sLogCodeException = "XX";
        # endregion Data

        # region Methods

        #region Core Functions
        public static bool Init()
        {
            return Init(CApplication.sApplicationName);
        }
        public static bool Init(string sAppName)
        {
            bool bStatus = true;
            try
            {
                LogFile = new Microsoft.VisualBasic.Logging.FileLogTraceListener();
                LogFile.BaseFileName = string.Format("{0}{1}", sAppName, "Log"); 
                LogFile.Location = Microsoft.VisualBasic.Logging.LogFileLocation.Custom;
                LogFile.CustomLocation = CApplication.sLogFilePath;
                LogFile.MaxFileSize = long.MaxValue; // 5 GB is the max size
                LogFile.DiskSpaceExhaustedBehavior = Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption.DiscardMessages;
                LogFile.IncludeHostName = true;
                LogFile.LogFileCreationSchedule = Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption.Daily;
                LogFile.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.DateTime | TraceOptions.LogicalOperationStack | TraceOptions.ProcessId | TraceOptions.ThreadId | TraceOptions.Timestamp;
                LogFile.Append = true;
                LogFile.AutoFlush = true;
                LogEntry(LogFile.FullLogFileName, sLogCodeInfo, "Logfile Opened");

                LogFileEx = new Microsoft.VisualBasic.Logging.FileLogTraceListener();
                LogFileEx.BaseFileName = string.Format("{0}{1}", sAppName, "LogEx"); 
                LogFileEx.Location = Microsoft.VisualBasic.Logging.LogFileLocation.Custom;
                LogFileEx.CustomLocation = CApplication.sLogFilePath;
                LogFileEx.MaxFileSize = long.MaxValue; // 5 GB is the max size
                LogFileEx.DiskSpaceExhaustedBehavior = Microsoft.VisualBasic.Logging.DiskSpaceExhaustedOption.DiscardMessages;
                LogFileEx.IncludeHostName = true;
                LogFileEx.LogFileCreationSchedule = Microsoft.VisualBasic.Logging.LogFileCreationScheduleOption.Daily;
                LogFileEx.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.DateTime | TraceOptions.LogicalOperationStack | TraceOptions.ProcessId | TraceOptions.ThreadId | TraceOptions.Timestamp;
                LogFileEx.Append = true;
                LogFileEx.AutoFlush = true;
                LogEntry(LogFileEx.FullLogFileName, sLogCodeInfo, "LogfileEx Opened");

            }
            catch { bStatus = false; }

            return bStatus;

        }


        public static void Close()
        {
            LogEntry(LogFileEx.FullLogFileName, sLogCodeInfo, "LogfileEx Closed");
            LogFileEx.Close();
            LogFileEx.Dispose();
            if (LogFileEx != null) LogFileEx = null;

            LogEntry(LogFile.FullLogFileName, sLogCodeInfo, "Logfile Closed");
            LogFile.Close();
            LogFile.Dispose();
            if (LogFile != null) LogFile = null;
        
        }

        public static void LogEntry(string Module, string LogType, string Message)
        {
            if (LogFile == null && !Init()) return;
            LogFile.WriteLine(string.Format(sLogFormat, sLogDelimiter, System.DateTime.Now, CApplication.sApplicationCode, LogType, Module, Message));
        }

        #endregion Core Functions

        #region Info, Debug, Error & Warning
        #region Information
        public static void Information(string Module, string Message) { LogEntry(Module, sLogCodeInfo, Message); }
        public static void Information(string Message) { LogEntry(CUtils.CModule(MethodInfo.GetCurrentMethod()), sLogCodeInfo, Message); }



        #endregion Information

        #region WebLogs
        public static void WebEvent(string Event, string PageTitle, CUserInfo LoggedInUser)
        {
            if (CApplication.CodeMethodLogging)
            LogEntry(PageTitle, sLogCodeInfo, String.Format("<Server>{0}~<Page>{1}~<Session>{2}<User>{3}", CApplication.WebServerName, Event, LoggedInUser.LoginSessionID, LoggedInUser.LoginAlias));
        }
        public static void WebError(string Event, string PageTitle, CUserInfo LoggedInUser, Exception ex)
        {
            LogEntry(PageTitle, sLogCodeException, String.Format("<Server>{0}~<Page>{1}~<Session>{2}<User>{3}<Error>{4}", CApplication.WebServerName, Event, LoggedInUser.LoginSessionID, LoggedInUser.LoginAlias,ex.Message));
            LogFileEx.TraceData(new System.Diagnostics.TraceEventCache(), String.Format("{0}=>{1}",PageTitle,Event), System.Diagnostics.TraceEventType.Error, 0, ex);
        }
        #endregion WebLogs

        #region Debug
        public static void Debug(string Module, string Message) { LogEntry(Module, sLogCodeDebug, Message); }
        public static void Debug(string Message) { LogEntry(CUtils.CModule(MethodInfo.GetCurrentMethod()), sLogCodeDebug, Message); }
        #endregion Debug

        #region Error
        public static void Error(string Module, string Message) { LogEntry(Module, sLogCodeError, Message); }
        public static void Error(string Message) { LogEntry(CUtils.CModule(MethodInfo.GetCurrentMethod()), sLogCodeError, Message); }
        public static void Error(string Module, bool Condition, String Message, bool ThrowError)
        {
            if (!Condition) return;
            LogEntry(Module, sLogCodeWarn, Message);
            if (ThrowError) throw new Exception("Exception Raised for : " + Message);
        }
        #endregion Error

        #region Warning
        public static void Warning(string Module, string Message) { LogEntry(Module, sLogCodeWarn, Message); }
        public static void Warning(string Message) { LogEntry(CUtils.CModule(MethodInfo.GetCurrentMethod()), sLogCodeWarn, Message); }
        #endregion Warning

        #endregion

        #region Exceptions
        public static void Exception(string Module,System.Data.SqlClient.SqlException e)
        {
            LogEntry(Module, sLogCodeException , "SQL Server Exception Occured");
            LogFileEx.TraceData(new System.Diagnostics.TraceEventCache(), Module, System.Diagnostics.TraceEventType.Error, e.Number , e);
        }

        public static void Exception(string Module, int Error, object Exception)
        {
            LogEntry(Module, sLogCodeException, "System Exception Occured");
            LogFileEx.TraceData(new System.Diagnostics.TraceEventCache(), Module, System.Diagnostics.TraceEventType.Error, Error, Exception);
        }
        public static void Exception(string Module, Exception e)
        {
            Exception(Module, 0, e);
        }

        #endregion Exceptions

        # endregion Methods
    }
}
