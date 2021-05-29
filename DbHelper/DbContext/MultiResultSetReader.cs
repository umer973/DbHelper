

namespace DbHelper.DbContext
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Data.Entity.Core.Objects;
    using System.Data.Common;
    using System.Data;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Class MultiResultSetReader
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MultiResultSetReader : IDisposable
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly DbContext _context;
        /// <summary>
        /// The command
        /// </summary>
        private readonly DbCommand _command;
        /// <summary>
        /// The connection needs to be closed
        /// </summary>
        private bool _connectionNeedsToBeClosed;
        /// <summary>
        /// The reader
        /// </summary>
        private DbDataReader _reader;

        /// <summary>
        /// constructor to instantiate db context object
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        public MultiResultSetReader(DbContext context, string query, SqlParameter[] parameters)
        {
            _context = context;
            _command = _context.Database.Connection.CreateCommand();
            _command.CommandText = query;
            if (parameters != null && parameters.Any()) _command.Parameters.AddRange(parameters);
        }

        /// <summary>
        /// dispose to dispose sql reader and close connection
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            if (_connectionNeedsToBeClosed)
            {
                _context.Database.Connection.Close();
                _connectionNeedsToBeClosed = false;
            }
        }

        /// <summary>
        /// return result sets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ObjectResult<T> ResultSetFor<T>()
        {
            if (_reader == null)
            {
                _reader = GetReader();
            }
            else
            {
                _reader.NextResult();
            }

            var objContext = ((IObjectContextAdapter)_context).ObjectContext;

            return objContext.Translate<T>(_reader);
        }

        /// <summary>
        /// return db reader objects
        /// </summary>
        /// <returns></returns>
        private DbDataReader GetReader()
        {
            if (_context.Database.Connection.State != ConnectionState.Open)
            {
                _context.Database.Connection.Open();
                _connectionNeedsToBeClosed = true;
            }

            return _command.ExecuteReader();
        }
    }
}
