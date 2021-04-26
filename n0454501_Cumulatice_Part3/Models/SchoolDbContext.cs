using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace n0454501_Cumulatice_Part3.Models
{
    public class SchoolDbContext
    {
        //Readonly credentials to access the mysql databale and tables from Mamp
        private static string User { get { return "root"; } }

        private static string Password { get { return "root"; } }

        private static string Database { get { return "schooldb"; } }

        private static string Server { get { return "localhost"; } }

        private static string Port { get { return "3306"; } }

        //Creating a series of credentials to access the database

        protected static string ConnectionString
        {
            //zero datetime coverts the datesto to 0000-00-00 if the ddates re null in any of the rows of the table
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }
        // This method fetches the database
        /// <summary>
        ///  Returns the connection to the schooldb database
        /// </summary>
        /// <exxample>
        /// private SchoolDbContext SchoolDb = new SchoolDbContext();
        /// MySqlConnection Conn = SchoolDb.AccessDatabase();
        /// </exxample>
        /// <returns>My SqlConnection Object</returns>

        public MySqlConnection AccessDatabase()
        {
            //We are representing the MySqlConnection Class to create an object
            //the object is an exclusive connection to the schooldb database at port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }

    }
}