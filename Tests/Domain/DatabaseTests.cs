using CourseCentral.IoC;
using Ninject;
using NUnit.Framework;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace CourseCentral.Tests.Domain
{
    [TestFixture, Database]
    public abstract class DatabaseTests
    {
        protected String connectionString;

        private IKernel kernel;

        [TestFixtureSetUp]
        public void IntegrationTestsFixtureSetup()
        {
            kernel = new StandardKernel();

            var domainLoader = new CourseCentralModuleLoader();
            domainLoader.LoadModules(kernel);

            connectionString = ConfigurationManager.AppSettings["connectionString"];
        }

        [SetUp]
        public void IntegrationTestsSetup()
        {
            kernel.Inject(this);
        }

        protected SqlConnection GetAndOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        protected SqlCommand GetCommand(String sql, SqlConnection connection, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(sql, connection);

            if (parameters.Any())
                command.Parameters.AddRange(parameters);

            return command;
        }
    }
}
