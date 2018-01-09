using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace EmployeeService.Database
{
    /// <summary>
    /// This class sets up the SQLConnection, SQLCommand and SQLDataReader objects for connecting to the
    /// database and performing operations
    /// </summary>
    public class ConnectionService : IDisposable
    {
        public SqlConnection connection { get; private set; }
        public SqlCommand command;

        public SqlDataReader reader = null;

        /// <summary>
        /// Initializes the SQLConnection object with the provided connection string
        /// </summary>
        /// <param name="connectionString">
        /// A string containing all the connection details
        /// </param>
        public ConnectionService(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// This method initializes the SQLCommand object with the provided command string
        /// </summary>
        /// <param name="command">
        /// string containing the SQL command to be executed
        /// </param>
        /// <returns>
        /// SQL Command object
        /// </returns>
        public SqlCommand getSQLCommand(string command)
        {
            this.command = new SqlCommand(command, this.connection);
            return this.command;
        }

        /// <summary>
        /// This method initializes the SQL reader object and executes the reader with the SQL command
        /// </summary>
        /// <returns>
        /// SQL Data reader object
        /// </returns>
        public SqlDataReader getSQLReader()
        {
            this.reader = this.command.ExecuteReader();
            return this.reader;
        }

        /// <summary>
        /// This method disposes any objects that have been created in this class
        /// </summary>
        public void Dispose()
        {
            this.connection.Close();
            this.command.Dispose();

            if (this.reader != null)
                this.reader.Close();
        }
    }
}