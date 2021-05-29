

namespace DbHelper.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;


    /// <summary>
    /// Class Sql Query
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    /// <seealso cref="Home.DbActivity.Service.DbContext.ISqlQuery" />
    public class SqlQuery : System.Data.Entity.DbContext, ISqlQuery
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQuery"/> class.
        /// </summary>
        public SqlQuery()
            : base(ConnectionString)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQuery"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlQuery(string connectionString)
            : base(ConnectionString)
        {
        }



        /// <summary>
        /// ExecuteNonQuery- this method used to return collection type of result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteNonQuery<T>(string routineName, IDictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();

            if (parameters != null && parameters.Count > 0)
                sqlParameters.AddRange(parameters.Keys.Select(key => new SqlParameter(key, parameters[key] ?? SqlInt32.Null)));
            return
                Database.SqlQuery<T>(string.Format("{0} {1}", routineName, ParameterList(parameters)),
                    sqlParameters.ToArray()).ToArrayAsync().Result;
        }

        /// <summary>
        /// ExecuteNonQueryTblType- this method used to return collection type of result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="tblName">Name of the table.</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteNonQueryTblType<T>(string routineName, IDictionary<string, object> parameters, string tblName)
        {
            var sqlParameters = new List<SqlParameter>();

            if (parameters != null && parameters.Count > 0)
                foreach (var param in parameters)
                {
                    if (param.Key != "TV")
                        sqlParameters.Add(new SqlParameter(param.Key, param.Value));
                    else
                    {
                        var tbleTypeSqlParam = new SqlParameter(param.Key, SqlDbType.Structured);
                        tbleTypeSqlParam.Value = param.Value;
                        tbleTypeSqlParam.TypeName = tblName;
                        sqlParameters.Add(tbleTypeSqlParam);
                    }
                }
            return
                Database.SqlQuery<T>(string.Format("{0} {1}", routineName, ParameterList(parameters)),
                    sqlParameters.ToArray()).ToList<T>();
        }

        /// <summary>
        /// ExecuteUpdate- this method use for update/delete where return type is not rquired
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public bool ExecuteUpdate(string routineName, IDictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();

            if (parameters != null && parameters.Count > 0)
                sqlParameters.AddRange(parameters.Keys.Select(key => new SqlParameter(key, parameters[key] == null ? SqlInt32.Null : parameters[key])));
            return
                Database.SqlQuery<object>(string.Format("{0} {1}", routineName, ParameterList(parameters)),
                    sqlParameters.ToArray()).AnyAsync().Result;
        }

        /// <summary>
        /// Executes the type of the update table.
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="tblName">Name of the table.</param>
        /// <returns></returns>
        public bool ExecuteUpdateTblType(string routineName, IDictionary<string, object> parameters, string tblName)
        {
            var sqlParameters = new List<SqlParameter>();

            if (parameters != null && parameters.Count > 0)
                foreach (var param in parameters)
                {
                    if (!param.Key.Contains("_TVP"))
                        sqlParameters.Add(new SqlParameter(param.Key, param.Value));
                    else
                    {
                        var tbleTypeSqlParam = new SqlParameter(param.Key, SqlDbType.Structured);
                        tbleTypeSqlParam.Value = param.Value;
                        tbleTypeSqlParam.TypeName = tblName;
                        sqlParameters.Add(tbleTypeSqlParam);
                    }
                }

            return
                Database.SqlQuery<object>(string.Format("{0} {1}", routineName, ParameterList(parameters)),
                    sqlParameters.ToArray()).AnyAsync().Result;
        }

        /// <summary>
        /// ExecuteScalar-this method used to return scalar type of result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string routineName, IDictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();

            if (parameters != null && parameters.Count > 0)
                foreach (var key in parameters.Keys)
                    sqlParameters.Add(new SqlParameter(key, parameters[key] ?? SqlInt32.Null));
            return
                Database.SqlQuery<T>(string.Format("{0} {1}", routineName, ParameterList(parameters)),
                    sqlParameters.ToArray()).FirstAsync().Result;
        }

        /// <summary>
        /// ExecuteNonQuery- this method use for Insert where return type is not rquired
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string routineName, IDictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();
            var result = 0;
            if (parameters != null && parameters.Count > 0)
                sqlParameters.AddRange(parameters.Keys.Select(key => new SqlParameter(key, parameters[key] ?? SqlInt32.Null)));
            var output = Database.SqlQuery<object>(string.Format("{0} {1}", routineName, ParameterList(parameters)), sqlParameters.ToArray()).AnyAsync().Result;
            if (!output) result = 1;
            return result;
        }



        /// <summary>
        /// ParameterList
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private static string ParameterList(IDictionary<string, object> parameters)
        {
            var list = parameters.Keys.Aggregate(string.Empty, (current, key) => current + string.Format(",@{0}", key));
            return list.Substring(1);
        }

        // <summary>
        /// <summary>
        /// Multis the result set SQL query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public List<object> MultiResultSetSqlQuery<T1, T2>(string routineName, IDictionary<string, object> parameters)
        {
            List<object> list = new List<object>();
            var sqlParameters = new List<SqlParameter>();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var key in parameters.Keys)
                {
                    sqlParameters.Add(new SqlParameter(key, parameters[key] ?? SqlInt32.Null));

                }
            }
            using (var multiResultSet = new MultiResultSetReader(this, string.Format("{0} {1}", routineName, ParameterList(parameters)), sqlParameters.ToArray()))
            {
                var resultSetFirst = multiResultSet.ResultSetFor<T1>().ToList();
                var resultSetSecond = multiResultSet.ResultSetFor<T2>().ToList();
                list.Add(resultSetFirst);
                list.Add(resultSetSecond);
            }
            return list;

        }

        // <summary>
        /// return multiple result sets
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="routineName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<object> MultiResultSetSqlQuery<T1, T2, T3>(string routineName, IDictionary<string, object> parameters)
        {
            List<object> list = new List<object>();
            var sqlParameters = new List<SqlParameter>();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var key in parameters.Keys)
                {
                    sqlParameters.Add(new SqlParameter(key, parameters[key] ?? SqlInt32.Null));

                }
            }
            using (var multiResultSet = new MultiResultSetReader(this, string.Format("{0} {1}", routineName, ParameterList(parameters)), sqlParameters.ToArray()))
            {
                var resultSetFirst = multiResultSet.ResultSetFor<T1>().ToList();
                var resultSetSecond = multiResultSet.ResultSetFor<T2>().ToList();
                var resultSetThird = multiResultSet.ResultSetFor<T3>().ToList();
                list.Add(resultSetFirst);
                list.Add(resultSetSecond);
                list.Add(resultSetThird);
            }
            return list;

        }
    }
}
