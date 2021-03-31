using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SqlDataReaderMapper
{
    public class DatabaseConnector
    {
        private string _connectionString;
        private int _commandTimeout;
        public DatabaseConnector(string connectionString, int commandTimeout = 30)
        {
            _connectionString = connectionString;
            _commandTimeout = commandTimeout;
        }

        public IEnumerable<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, IEnumerable<SqlParameter> sqlParameters = null) where TReturnType : class, new()
        {
            return Execute<TReturnType>(CommandType.StoredProcedure, storedProcedureName, sqlParameters);
        }
        public IEnumerable<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, Func<SqlDataReader, TReturnType> mapDataAction, IEnumerable<SqlParameter> sqlParameters = null) where TReturnType : class, new()
        {
            return Execute<TReturnType>(CommandType.StoredProcedure, storedProcedureName, sqlParameters, mapDataAction);
        }
        public IEnumerable<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, IEnumerable<SqlParameter> sqlParameters = null) where TReturnType : new()
        {
            return Execute<TReturnType>(CommandType.Text, commandText, sqlParameters);
        }
        public IEnumerable<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, Func<SqlDataReader, TReturnType> mapDataAction, IEnumerable<SqlParameter> sqlParameters=null) where TReturnType : class, new()
        {
            return Execute<TReturnType>(CommandType.Text, commandText, sqlParameters, mapDataAction);
        }
        private IEnumerable<TReturnType> Execute<TReturnType>(CommandType commandType, string commandText, IEnumerable<SqlParameter> sqlParameters = null, Func<SqlDataReader, TReturnType> mapDataAction = null) where TReturnType : new()
        {
            List<TReturnType> result = new List<TReturnType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                {
                    sqlCommand.CommandTimeout = _commandTimeout;
                    sqlCommand.CommandType = commandType;
                    if (sqlParameters != null)
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        SqlDataReaderMapper<TReturnType> sqlMapper = new SqlDataReaderMapper<TReturnType>(reader);
                        while (reader.Read())
                        {
                            TReturnType returnObject = mapDataAction == null ? sqlMapper.Map() : mapDataAction(reader);
                            result.Add(returnObject);
                        }
                    }
                }
            }
            return result;
        }


    }
}
