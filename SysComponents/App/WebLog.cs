using System;
using System.Configuration;
using System.IO;

namespace SysComponents
{
    public class WebLog
    {
        public static void Log(String LoggedInUser,String PageName,String Action,String Data)
        {
            FileStream file = new FileStream(ConfigurationManager.AppSettings["LogFilePath"] + LoggedInUser + "_WebLog_" + DateTime.Today.ToString("MM_dd_yyyy") + ".txt", FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            try
            {
               
                sw.WriteLine(DateTime.Now + "|" + LoggedInUser + "|" + PageName + "|" + Action + "|" + Data);
                sw.Close();
                file.Close();
            }
            catch (Exception e)
            {
                sw.WriteLine(DateTime.Now + "|" + LoggedInUser + "|" + PageName + "|" + Action + "|" + "InnerException: "+ e.InnerException + "Stack Trace: "+ e.StackTrace );
                sw.Close();
                file.Close();
            }
        }
     
    }
}
