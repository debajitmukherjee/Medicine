using System;
using System.IO;
using System.Text;
using System.Configuration;



namespace Medicine.Common
{
    /// <summary>
    /// This class is used to log error in server
    /// </summary> 
    public static class ErrorLog
    {
        /// <summary>
        /// The Error Log Folder Path
        /// </summary>
        private static  string errorLogFolderPath = string.Empty; 
        
        /// <summary>
        /// The Error Count
        /// </summary>
        public static long ErrorCount
        {
            get;
            private set;
        }

        static string errorFilePath = string.Empty;

        /// <summary>
        /// Generate Error File Name
        /// </summary>
        private static void GenerateErrorFileName()
        {
            if (string.IsNullOrEmpty(errorLogFolderPath))
            {
                errorLogFolderPath = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
            }

            if (string.IsNullOrEmpty(errorFilePath))
            {
                errorFilePath = ErrorLog.errorLogFolderPath + string.Format("log_{0}.html", DateTime.Now.ToString("MMM_dd_yyyy_hhmmtt"));
            }
        }
        
        /// <summary>
        /// Write Log into file
        /// </summary>
        /// <param name="className"></param>
        /// <param name="eventName"></param>
        /// <param name="shortMessage"></param>
        /// <param name="errorDescription"></param>
        /// <param name="queryCondtions"></param>
        public static void WriteLog(string className, string eventName, string shortMessage, string errorDescription, string queryCondtions)
        {
            ErrorCount = ErrorCount + 1;
            if (ErrorCount >= long.MaxValue)
            {
                ErrorCount = 1;
            }
            className = string.Format("<a name=\"{0}\" href=\"#{0}\">{0}.{1}</a>", ErrorCount, className);
            WriteLogInServer(className, eventName, shortMessage, errorDescription, queryCondtions);
        }

        /// <summary>
        /// Write Log into file
        /// </summary>
        /// <param name="className"></param>
        /// <param name="eventName"></param>
        /// <param name="ex"></param>
        /// <param name="queryCondtions"></param>
        public static void WriteLog(string className, string eventName, Exception ex, string queryCondtions)
        {
            WriteLog(className, eventName, ex.Message, ex.StackTrace, queryCondtions);
        }
        
        /// <summary>
        /// Write Log into file
        /// </summary>
        /// <param name="className"></param>
        /// <param name="eventName"></param>
        /// <param name="shortMessage"></param>
        /// <param name="errorDescription"></param>
        /// <param name="queryCondtions"></param>
        private static void WriteLogInServer(string className, string eventName, string shortMessage, string errorDescription, string queryCondtions)
        {
            FileStream fileStream = null;
            try
            {
                GenerateErrorFileName();

                bool isNew = false;

                if (!Directory.Exists(errorLogFolderPath))
                {
                    Directory.CreateDirectory(errorLogFolderPath);
                }

                if (!File.Exists(errorFilePath))
                {
                    isNew = true;
                }

                fileStream = new FileStream(errorFilePath, FileMode.Append, FileAccess.Write);
                using (StreamWriter oSW = new StreamWriter(fileStream))
                {
                    StringBuilder strMessage = new StringBuilder();

                    if (isNew)
                    {
                        strMessage.Append("<table border='1' cellspacing='1' width='100%'>");
                        strMessage.Append("<tr>");
                        strMessage.AppendFormat("<td>{0}</td>", "Date");
                        strMessage.AppendFormat("<td>{0}</td>", "Time");
                        strMessage.AppendFormat("<td>{0}</td>", "Source");
                        strMessage.AppendFormat("<td>{0}</td>", "Event");
                        strMessage.AppendFormat("<td>{0}</td>", "Message");
                        strMessage.AppendFormat("<td>{0}</td>", "StackTrace");
                        strMessage.AppendFormat("<td>{0}</td>", "Condition");
                        strMessage.Append("</tr>");
                    }
                    strMessage.Append("<tr>");
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", DateTime.Now.ToShortDateString());
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", DateTime.Now.ToLongTimeString());
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", className);
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", eventName);
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", shortMessage);
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", errorDescription);
                    strMessage.AppendFormat("<td valign=\"top\">{0}&#160;</td>", queryCondtions);
                    strMessage.Append("</tr>");
                   
                    oSW.WriteLine("");
                    oSW.WriteLine(strMessage);
                   
                    
                }
            }
            catch
            {
                throw;
            }
        }

        
    }
}
