#region References
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
#endregion References


namespace SysComponents
{
    public class CData : IDisposable
    {
        # region Data
        public bool Success;
        public bool HasRows;
        public bool Executed;
        public int RowsReturned;
        public int ColsReturned;
        public int ReturnValue; // This can be Object
        public DataTable sqlData;
        public SqlException sqlErr;
        public Exception sysErr;
        public string sqlErrMsg;
        public string sqlQuery;
        # endregion Data

        #region Constructors
        public CData()
        {
            Success = false;
            HasRows = false;
            Executed = true;
            RowsReturned = 0;
            ReturnValue = -1;
        }
        #endregion Constructors

        # region Methods
        public void Close()
        {
            if (sqlData != null) { sqlData.Clear(); sqlData.Dispose(); sqlData = null; }
            if (sqlErr != null) sqlErr = null;
            if (sysErr != null) sysErr = null;
        }
        public void Clear()
        {
            this.Close();
        }
        void IDisposable.Dispose() { this.Close(); }
        # endregion Methods
    }

    public class CDBCommand : IDisposable 
    {
        # region Data
        private System.Data.SqlClient.SqlCommand cmd; 
        # endregion Data

        # region Constructors
        public CDBCommand()
        {
            cmd = new SqlCommand("", new SqlConnection(CApplication.DBConnectionString));
            cmd.CommandTimeout = CApplication.iCommandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
        }
        # endregion Constructors

        # region Methods

        public void SetProcedure(string sProcedureName)
        {
            try
            {
                cmd.CommandText = sProcedureName;
                if (cmd.Parameters.Count > 0) cmd.Parameters.Clear();
                cmd.Parameters.Add("RETV", SqlDbType.Int);
                cmd.Parameters["RETV"].Direction = ParameterDirection.ReturnValue;
            }
            catch (Exception) { };
        }

        public void SetParameter(string sParamName, object ParamValue, SqlDbType dbType, ParameterDirection ParamDirection, bool NullIfEmpty)
        {
            try
            {
                cmd.Parameters.Add(sParamName, dbType);
                cmd.Parameters[sParamName].Direction = ParamDirection;
                if (!NullIfEmpty) cmd.Parameters[sParamName].Value = ParamValue;
                else cmd.Parameters[sParamName].Value = (ParamValue == null || String.IsNullOrEmpty(ParamValue.ToString()) || ParamValue.ToString().Trim() == "" || ParamValue == DBNull.Value) ? DBNull.Value : ParamValue;
            }
            catch (Exception ){};
        }

        public void ExecuteCommand(CData sqlResults)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            if (CApplication.DBCallsLogging) CLog.Debug(Module, CommandDump());
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                sqlResults.RowsReturned = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                sqlResults.sqlErr = e;
            }
            catch (Exception e)
            {
                sqlResults.sysErr = e;
            }
            finally
            {
                sqlResults.Success = (sqlResults.sqlErr == null && sqlResults.sysErr == null);
                if (!sqlResults.Success)
                {
                    if (sqlResults.sqlErr != null)
                    {
                        sqlResults.ReturnValue = sqlResults.sqlErr.Number * -1;
                        sqlResults.sqlErrMsg = sqlResults.sqlErr.Source + "@" + sqlResults.sqlErr.Procedure + "  " + sqlResults.sqlErr.Message;
                    }
                    else
                    {
                        sqlResults.ReturnValue = -1;
                        sqlResults.sqlErrMsg = sqlResults.sysErr.Source + " ( " + sqlResults.sqlErr.Message + " )";
                    }                   
                }
                else sqlResults.ReturnValue = CUtils.CInt(cmd.Parameters["RETV"].Value, -1);
            }
        }

        // Executes query on DB and stores results into CData
        public void ExecuteQuery(CData sqlResults)
        {
            string Module = CUtils.CModule(MethodInfo.GetCurrentMethod());
            if (CApplication.DBCallsLogging) CLog.Debug(Module, CommandDump());
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (System.Data.SqlClient.SqlDataReader sqlRead = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    if (sqlRead.HasRows)
                    {
                        sqlResults.HasRows = true;
                        sqlResults.sqlData = new DataTable(cmd.CommandText);
                        sqlResults.sqlData.Load(sqlRead, LoadOption.OverwriteChanges);
                        sqlResults.RowsReturned = sqlResults.sqlData.Rows.Count;
                        sqlResults.ColsReturned = sqlResults.sqlData.Columns.Count;                         
                    }
                }
            }
            catch (SqlException e)
            {
                sqlResults.sqlErr = e;
            }
            catch (Exception e)
            {
                sqlResults.sysErr = e;
             }
            finally
            {
                sqlResults.Success = (sqlResults.sqlErr == null && sqlResults.sysErr == null);
                if (!sqlResults.Success)
                {
                    if (sqlResults.sqlErr != null)
                    {
                        sqlResults.ReturnValue = sqlResults.sqlErr.Number * -1;
                        sqlResults.sqlErrMsg = sqlResults.sqlErr.Source + "@" + sqlResults.sqlErr.Procedure + "  " + sqlResults.sqlErr.Message;
                    }
                    else
                    {
                        sqlResults.ReturnValue = -1;
                        sqlResults.sqlErrMsg = sqlResults.sysErr.Source + " ( " + sqlResults.sqlErr.Message + " )";
                    }
                }
                else sqlResults.ReturnValue = CUtils.CInt(cmd.Parameters["RETV"].Value, -1);
                if (CApplication.DBResultsLogging) CLog.Debug(Module, ResultsDump(sqlResults));
            }
        }

        public void Close()
        {
            if (cmd != null) 
            {
                cmd.Connection.Close(); 
                cmd.Dispose();
                cmd = null;
            }
        }

        void IDisposable.Dispose()
        {
            this.Close ();
        }

        // For testing.  Returns the SQL Command after construction
        public string CommandDump()
        {
            String Result;
            try
            {
                StringBuilder LogString = new StringBuilder();
                LogString.AppendFormat("{0}~{1}  ", cmd.CommandType, cmd.CommandText);
                foreach (SqlParameter Current in cmd.Parameters) LogString.AppendFormat("{0}={1}~[{2}~{3}]", Current.ParameterName, Current.Value, Current.SqlDbType, Current.Direction);
                Result=LogString.ToString();
            }
            catch (Exception e) { Result = e.ToString(); }
            return Result;
        }

        // For testing.  Returns the SQL Results after execution
        string ResultsDump(CData sqlResults)
        {
            String Result;
            try
            {
                System.IO.StringWriter str = new System.IO.StringWriter();
                str.WriteLine("sqlResults.Success {0}",sqlResults.Success);                
                str.WriteLine("sqlResults.HasRows {0}",sqlResults.HasRows);                
                str.WriteLine("sqlResults.ReturnValue {0}",sqlResults.ReturnValue);                
                str.WriteLine("sqlResults.RowsReturned {0}",sqlResults.RowsReturned);
                str.WriteLine("sqlResults.ColsReturned {0}", sqlResults.ColsReturned);
                str.WriteLine("sqlResults.sqlErrMsg {0}",sqlResults.sqlErrMsg);  
                if (sqlResults.HasRows)
                {
                    str.WriteLine("Results");
                    sqlResults.sqlData.WriteXml(str,XmlWriteMode.WriteSchema,true);
                }
                Result= str.ToString();
            }
            catch (Exception e) { Result = e.ToString(); }
            return Result;
        }

        # endregion Methods

    }
}
