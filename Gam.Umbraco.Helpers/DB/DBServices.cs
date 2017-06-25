using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gam.Umbraco.Helpers
{
    class DBServices
    {

        public static T ExecuteScalar<T>(string commandText, params SqlParameter[] commandParameters)
        {
            var connection = new SqlConnection(umbraco.BusinessLogic.Application.SqlHelper.ConnectionString);
            var command = new SqlCommand();

            PrepareCommand(command, connection, null, CommandType.StoredProcedure, commandText, commandParameters);
            var result = (T)command.ExecuteScalar();
            command.Parameters.Clear();
            connection.Close();

            return result;

        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter parameter in commandParameters)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                command.Parameters.Add(parameter);
            }
        }
    }
}
