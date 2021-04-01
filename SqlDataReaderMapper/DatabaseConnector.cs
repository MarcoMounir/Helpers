using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SqlDataReaderMapper
{
    public class DatabaseConnector
    {
        private IDbConnection _dbConnection;
        private int _commandTimeout;
        public DatabaseConnector(IDbConnection sqlConnection, int commandTimeout = 30)
        {
            _dbConnection = sqlConnection;
            _commandTimeout = commandTimeout;
        }

        public IEnumerable<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, IEnumerable<IDbDataParameter> sqlParameters = null) where TReturnType : new()
        {
            return Execute<TReturnType>(CommandType.StoredProcedure, storedProcedureName, sqlParameters);
        }
        public IEnumerable<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, Func<IDataReader, TReturnType> mapDataAction, IEnumerable<SqlParameter> sqlParameters = null) where TReturnType : new()
        {
            return Execute<TReturnType>(CommandType.StoredProcedure, storedProcedureName, sqlParameters, mapDataAction);
        }
        public IEnumerable<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, IEnumerable<IDbDataParameter> sqlParameters = null) where TReturnType : new()
        {
            return Execute<TReturnType>(CommandType.Text, commandText, sqlParameters);
        }
        public IEnumerable<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, Func<IDataReader, TReturnType> mapDataAction, IEnumerable<SqlParameter> sqlParameters = null) where TReturnType : new()
        {
            return Execute<TReturnType>(CommandType.Text, commandText, sqlParameters, mapDataAction);
        }
        private IEnumerable<TReturnType> Execute<TReturnType>(CommandType commandType, string commandText, IEnumerable<IDbDataParameter> sqlParameters = null, Func<IDataReader, TReturnType> mapDataAction = null) where TReturnType : new()
        {
            List<TReturnType> result = new List<TReturnType>();
            InitConnection();
            using (IDbCommand sqlCommand = GetCommandInstanceAccordingToDatabaseType(commandText))
            {
                sqlCommand.CommandTimeout = _commandTimeout;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    foreach (IDbDataParameter dbDataParameter in sqlParameters)
                        sqlCommand.Parameters.Add(dbDataParameter);
                using (IDataReader reader = sqlCommand.ExecuteReader())
                {
                    DataReaderMapper<TReturnType> mapper = new DataReaderMapper<TReturnType>(reader);
                    while (reader.Read())
                    {
                        TReturnType returnObject = mapDataAction == null ? mapper.Map() : mapDataAction(reader);
                        result.Add(returnObject);
                    }
                }
            }

            return result;
        }

        private void InitConnection()
        {
            if (_dbConnection.State != ConnectionState.Open || _dbConnection.State != ConnectionState.Connecting)
                _dbConnection.Open();
        }

        private IDbCommand GetCommandInstanceAccordingToDatabaseType(string commandText)
        {
            Type type = _dbConnection.GetType();
            if (type == typeof(System.Data.Odbc.OdbcConnection))
                return new System.Data.Odbc.OdbcCommand(commandText, (System.Data.Odbc.OdbcConnection) _dbConnection);
            else if (type == typeof(System.Data.OleDb.OleDbConnection))
                return new System.Data.OleDb.OleDbCommand(commandText, (System.Data.OleDb.OleDbConnection)_dbConnection);
            else if (type == typeof(System.Data.SqlClient.SqlConnection))
                return new System.Data.SqlClient.SqlCommand(commandText, (System.Data.SqlClient.SqlConnection)_dbConnection);
            else if (type == typeof(System.Data.OracleClient.OracleConnection))
                return new System.Data.OracleClient.OracleCommand(commandText,
                    (System.Data.OracleClient.OracleConnection) _dbConnection);
            else
                throw new Exception("Unsupported Database type");
        }

    }
}
