using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using com.helpers.DataReaderMapper.Mappers;

namespace com.helpers.DataReaderMapper
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
                    return mapDataAction != null ? Map<TReturnType>(reader, mapDataAction) : Map<TReturnType>(reader);
                }
            }
        }

        private List<TReturnType> Map<TReturnType>(IDataReader dataReader, Func<IDataReader, TReturnType> mapDataAction)
        {
            List<TReturnType> result = new List<TReturnType>();
            while (dataReader.Read())
            {
                TReturnType returnObject = mapDataAction(dataReader);
                result.Add(returnObject);
            }
            return result;
        }
        private List<TReturnType> Map<TReturnType>(IDataReader dataReader) where TReturnType : new()
        {
            List<TReturnType> result = new List<TReturnType>();
            BaseMapper<TReturnType> mapper = BaseMapper<TReturnType>.GetMapper(dataReader);
            while (dataReader.Read())
            {
                TReturnType returnObject = mapper.Map();
                result.Add(returnObject);
            }
            return result;
        }
        private void InitConnection()
        {
            if (_dbConnection.State == ConnectionState.Closed)
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
