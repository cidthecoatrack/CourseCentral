using System;
using System.Data.SqlClient;
using System.Linq;

namespace CourseCentral.Domain.Repositories.Domain
{
    public abstract class Repository
    {
        private readonly String connectionString;

        protected Repository(String connectionString)
        {
            this.connectionString = connectionString;
        }

        protected SqlConnection GetAndOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        protected SqlCommand GetCommand(String sql, SqlConnection connection,
            params SqlParameter[] parameters)
        {
            var command = new SqlCommand(sql, connection);

            if (parameters.Any())
                command.Parameters.AddRange(parameters);

            return command;
        }
    }
}
