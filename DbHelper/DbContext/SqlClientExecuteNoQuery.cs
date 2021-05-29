

namespace DbHelper.DbContext
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    

    /// <summary>
    /// Client Sql Client Execute No query
    /// </summary>
    public static class SqlClientExecuteNoQuery
    {
        /// <summary>
        /// ExecuteNonQuerySqlClientInsert
        /// </summary>
        /// <param name="parm">The parm.</param>
        /// <param name="storeProcedureName">Name of the store procedure.</param>
        /// <returns></returns>
        public static int ExecuteNonQuerySqlClientInsert(SqlParameter[] parm, string storeProcedureName)
        {
            int executeResult;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString))
            {

                var cmd = new SqlCommand(storeProcedureName, con) { CommandType = CommandType.StoredProcedure };
                foreach (var sqlParam in parm)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                con.Open();
                executeResult = cmd.ExecuteNonQuery();
                con.Close();
            }
            return executeResult;
        }
    }
}
