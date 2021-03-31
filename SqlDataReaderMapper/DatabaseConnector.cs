using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        public List<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, List<SqlParameter> sqlParameters = null) where TReturnType : class, new()
        {
            List<TReturnType> result = new List<TReturnType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandTimeout = _commandTimeout;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if(sqlParameters != null && sqlParameters.Count != 0)
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        SqlDataReaderMapper<TReturnType> sqlMapper = new SqlDataReaderMapper<TReturnType>(reader);
                        while (reader.Read())
                        {
                            TReturnType returnObject = sqlMapper.Map();
                            result.Add(returnObject);
                        }
                    }
                }
            }
            return result;
        }
        public List<TReturnType> ReadDataFromStoredProcedure<TReturnType>(string storedProcedureName, Func<SqlDataReader, TReturnType> mapDataAction, List<SqlParameter> sqlParameters = null) where TReturnType : class, new()
        {
            List<TReturnType> result = new List<TReturnType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    sqlCommand.CommandTimeout = _commandTimeout;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParameters != null && sqlParameters.Count != 0)
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TReturnType returnObject = mapDataAction(reader);
                            result.Add(returnObject);
                        }
                    }
                }
            }
            return result;
        }
        public List<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, List<SqlParameter> sqlParameters = null) where TReturnType : new()
        {
            List<TReturnType> result = new List<TReturnType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                {
                    sqlCommand.CommandTimeout = _commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count != 0)
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        SqlDataReaderMapper<TReturnType> sqlMapper = new SqlDataReaderMapper<TReturnType>(reader);
                        while (reader.Read())
                        {
                            TReturnType returnObject = sqlMapper.Map();
                            result.Add(returnObject);
                        }
                    }
                }
            }
            return result;
        }
        public List<TReturnType> ReadDataFromSqlCommand<TReturnType>(string commandText, Func<SqlDataReader, TReturnType> mapDataAction, List<SqlParameter> sqlParameters=null) where TReturnType : class, new()
        {
            List<TReturnType> result = new List<TReturnType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                {
                    sqlCommand.CommandTimeout = _commandTimeout;
                    if (sqlParameters != null && sqlParameters.Count != 0)
                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TReturnType returnObject = mapDataAction(reader);
                            result.Add(returnObject);
                        }
                    }
                }
            }
            return result;
        }


    }
}
