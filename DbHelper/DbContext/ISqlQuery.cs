


namespace DbHelper.DbContext
{
    using System;
    using System.Collections.Generic;

    internal interface ISqlQuery : IDisposable
    {

        /// <summary>\
        /// Executes the non query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteNonQuery<T>(string routineName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes the update.
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool ExecuteUpdate(string routineName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        T ExecuteScalar<T>(string routineName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteNonQuery(string routineName, IDictionary<string, object> parameters);
        /// <summary>
        /// Executes the type of the non query table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="tblName">Name of the table.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteNonQueryTblType<T>(string routineName, IDictionary<string, object> parameters, string tblName);

        /// <summary>
        /// Multis the result set SQL query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        List<object> MultiResultSetSqlQuery<T1, T2>(string routineName, IDictionary<string, object> parameters);

        /// <summary>
        /// Executes the type of the update table.
        /// </summary>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="tblName">Name of the table.</param>
        /// <returns></returns>
        bool ExecuteUpdateTblType(string routineName, IDictionary<string, object> parameters, string tblName);

        /// <summary>
        /// Multis the result set SQL query.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 2.</typeparam>
        /// <param name="routineName">Name of the routine.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        List<object> MultiResultSetSqlQuery<T1, T2, T3>(string routineName, IDictionary<string, object> parameters);
    }
}
