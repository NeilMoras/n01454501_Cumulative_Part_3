using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;  // connects to the mySql database NuGet package
using n0454501_Cumulatice_Part3.Models; // using the models folder inorder to access the classes defining feilds and the database


namespace n0454501_Cumulatice_Part3.Controllers
{
    public class ClassesDataController : ApiController
    {
        // ShoolDsContext api controller which will connect and get query from the schoolDb database in MAMP 
        private SchoolDbContext School = new SchoolDbContext();

        //This controller will access the classes table from the schooldb database in MAMP
        /// <summary>
        /// this code will retrive list of classes(Subject Names) and  their fields from the database to be used accordingly
        /// </summary>
        /// <example> GET api/ClassesData/ListClasses </example>
        /// <returns>List of Classes with their related  columns from the Classes table</returns>


        [HttpGet]
        public IEnumerable<Class> ListClasses()

        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information from Classes table

            Command.CommandText = "Select * from Classes";
            //Converts the query and stores it in a variable

            MySqlDataReader ResultSet = Command.ExecuteReader();
            // creates an empty array to store the list of Classes

            List<Class> Classes = new List<Class> { };
            // using while loop to itirate the list information fromt the students table 
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = (string)ResultSet["classcode"];
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FinishDate = (DateTime)ResultSet["finishdate"];
                string ClassName = (string)ResultSet["classname"];

                //creating a new variable and lining it to the models controller 
                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.StartDate = StartDate;
                NewClass.FinishDate = FinishDate;
                NewClass.ClassName = ClassName;
                //adding the variables to the empty list.
                Classes.Add(NewClass);


            }

            //Closing the connection once the information is retrieved from the database
            Connection.Close();
            // outputs the lists of students to the web browser
            return Classes;
        }

        /// <summary>
        /// this code will retreives single row of classes data and information and  their fields information from the database to be used accordingly
        /// </summary>
        /// <example>GET/api/ClassesData/FindClass/{id}</example>
        /// <param name="id"> integer as an input as ClassId</param>
        /// <returns> A single row of Class data and  information from the Classes table</returns>

        [HttpGet]
        public Class FindClass(int id)
        {
            Class NewClass = new Class();
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database

            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information with the help of id as a parameter input.This will retrive only one row of Class data from the Classes table as its looking up through each class id

            Command.CommandText = "Select * from Classes where classid = " + id;
            //Converts the query and  stores it in a variable

            MySqlDataReader ResultSet = Command.ExecuteReader();
            // using while loop to itirate the list information fromt the Classes table.


            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = (string)ResultSet["classcode"];
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FinishDate = (DateTime)ResultSet["finishdate"];
                string ClassName = (string)ResultSet["classname"];

                //creating a new variable and lining it to the models controller 
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.StartDate = StartDate;
                NewClass.FinishDate = FinishDate;
                NewClass.ClassName = ClassName;


            }
            // outputs from a row of  Class data from the database to the web browser
            return NewClass;
        }

        /// <summary>
        /// Adding a forign key to the  class table in school db dababase in order to maintain referenctial action
        /// in class if the teacher referred to the particular class is deleted, it will set the teacherid forerign key to null
        /// </summary>

        // Plan: Noticed that the teacheid in the classes table was referring to the primary key of the teachers table but it wasnt assigned as a foriegn key,
        //hence when the teacher was deleted , the teacherid of the teacher referring to the class was still referring to the teacher which was deleted
        //Solution: To assign teacherid in the classes table as a foreign key to maintain referential action

        // the Below code was an attempt to  add teacherid as a forign key but was not successfull after multiple attempts
        //Altnative working solution: Investigated the mysql database and assigned teacher id as the foreign key by using the alter command query which set the foreign key on delete to NULL
        // mySql query  : "Alter table classes add constraint teacher_id_fk foreign key (teacherid) references teachers (teacherid) on delete set null on update restrict"

        //<returns>
        // if a particular teacher is deleted, the classes assigned to that particular teeacher will be set to NULL
        // </returns>

        [HttpPost]
        public void AddForeignKey()
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            //query command to the database inorder to alter by making the teacherid in the classes tables a foreign key inorder to maintain referential action
            Command.CommandText = "Alter table classes add constraint teacher_id_fk foreign key (teacherid) references teachers (teacherid) on delete set null on update restrict";

            Command.Prepare();

            Command.ExecuteNonQuery();

            Connection.Close();
        }

    }
}
