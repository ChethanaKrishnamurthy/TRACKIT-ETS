#region References
using System;
using System.IO;
using System.Reflection;
using System.Text;
#endregion References

namespace SysComponents
{
    public class CUtils
    {

        # region Data
        # endregion Data

        # region Properties
        # endregion Properties

        # region Constructors
        # endregion Constructors



        # region Data Conversions
        public static bool CBool(object obj, bool Default)
        {
            bool ReturnData;
            if (obj == null) ReturnData = Default; else 
            switch (obj.ToString().ToUpper())
            {
                case "1" :
                case "TRUE":
                case ".TRUE.":
                case "YES":
                case "Y":
                case "T":
                case ".T.":
                case "OK": ReturnData = true; break;
                case "0":
                case "FALSE":
                case ".FALSE.":
                case "NO":
                case "N":
                case "F":
                case ".F.":
                case "NOT": ReturnData = false; break;
                default: ReturnData = Default; break;
            }
            return ReturnData;
        }

        public static float CFloat(object obj, float  Default)
        {
            float ReturnData;
            if (obj == null) ReturnData = Default; else
            if (!float.TryParse(obj.ToString(), out ReturnData)) ReturnData = Default;
            return ReturnData;
        }
        public static Decimal CDecimal(object obj, Decimal Default)
        {
            Decimal ReturnData;
            if (obj == null) ReturnData = Default;
            else
            if (!Decimal.TryParse(obj.ToString(), out ReturnData)) ReturnData = Default;
            return ReturnData;
        }
        public static int CInt(object obj, int Default)
        {
            int ReturnData;
            if (obj == null) ReturnData = Default; else 
            if (!int.TryParse(obj.ToString(), out ReturnData)) ReturnData = Default;
            return ReturnData;
        }

        public static long CLong(object obj, long Default)
        {
            long ReturnData;
            if (obj == null) ReturnData = Default;
            else
                if (!long.TryParse(obj.ToString(), out ReturnData)) ReturnData = Default;
            return ReturnData;
        }

        public static string CString(object obj, string Default)
        {
            return ((obj == null || string.IsNullOrEmpty(obj.ToString())) ? Default : obj.ToString().Trim());
        }

        public static DateTime  CDate(object obj, DateTime Default)
        {
            DateTime ReturnData;
            if (obj == null) ReturnData = Default; else 
            if (!DateTime.TryParse(obj.ToString(), out ReturnData)) ReturnData = Default;
            return ReturnData;
        }

 
        # endregion Data Conversions

        # region Data Validations
        public static bool  IsDate(object obj)
        {
            DateTime dummy;
            return (DateTime.TryParse(obj.ToString(), out dummy) && dummy >= DateTime.MinValue && dummy <= DateTime.MaxValue);
        }
        #endregion Data Validations

        # region ModuleInfo
        public static string CModule(MethodBase Obj)
        {
            string sModule = String.Format("{0}.{1}", Obj.ReflectedType.FullName, Obj.Name);
            //if (CApplication.bLogCallsMethod) CLog.Debug("Module.Enter", sModule);
            return sModule;
        }
        # endregion ModuleInfo

  



        #region NotInUse

        public static void FileCreate(string sFullPathFileName)
        {
            using (FileStream fs = File.Create(sFullPathFileName))
            {
                fs.Close();
                fs.Dispose();
            }
        }
        public static string GenerateTimeStampedFileName(string sName, bool bBackup, bool bError)
        {
            string sFileName = string.IsNullOrEmpty(sName) ? "" : sName;
            sFileName = sFileName + System.DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            if (bBackup) sFileName = sFileName + "." + CSystemGlobals.Defaults.FileBackupExt;
            else if (bError) sFileName = sFileName + "." + CSystemGlobals.Defaults.FileErrorExt;
            return sFileName;
        }
 

       public static string Left(string sInput, int len)
        {
            return string.IsNullOrEmpty(sInput) || len >= sInput.Length ? sInput : sInput.Remove(len);
        }
        public static string Substring(string input, int start, int len)
        {
            if (string.IsNullOrEmpty(input) || start <= 0) return "";
            if ((start + len - 1) > input.Length) return input;
            return input.Substring(start - 1, len);
        }
        public static string Repeat(string input, int Times)
        {
            StringBuilder tmp = new StringBuilder("");
            for (int x=0; x < Times; x++) { tmp.Append(input); }
            return tmp.ToString();
        }
         public static string RepeatLines(string input, int Times)
         {
             StringBuilder tmp = new StringBuilder("");
             for (int x=0; x < Times; x++) { tmp.AppendLine (input); }
             return tmp.ToString();
         }
    
        #endregion NotInUse
    }
}
