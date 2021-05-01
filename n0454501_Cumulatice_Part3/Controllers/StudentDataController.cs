using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using n0454501_Cumulatice_Part3.Models; // using the models folder inorder to access the student defining feilds and the database
using MySql.Data.MySqlClient; // connects to the mySql database NuGet package
using System.Diagnostics;
using System.Web.Http.Cors;

namespace n0454501_Cumulatice_Part3.Controllers
{
    public class StudentDataController : ApiController
    {
        // ShoolDsContext api controller which will connect and get query from the schoolDb database in MAMP 
        private SchoolDbContext School = new SchoolDbContext();

        //This controller will access the students table from the schooldb database in MAMP
        /// <summary>
        /// this code will retrive list of students and  their fields from the database to be used accordingly
        /// </summary>
        /// <example> GET api/StudentData/ListStudents </example>
        /// <returns>List of students with their related  columns from the students table</returns>

        [HttpGet]
        [Route("api/StudentData/ListStudent/{SearchKey?}")]
        public IEnumerable<Student> ListStudents(string SearchKey = null)  // since its a list of students, we have to use IEinumerable
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information from students table
            Command.CommandText = "select * from students where lower(studentfname) like  lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname, ' ', studentlname)) like lower(@key) or enroldate like @key or lower(studentnumber) like lower(@key)";


            Command.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            Command.Prepare();
            //Converts the query and stores it in a variable
            MySqlDataReader ResultSet = Command.ExecuteReader();
            // creates an empty array to store the list of students

            List<Student> Students = new List<Student> { };
            // using while loop to itirate the list information fromt the students table 
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = (string)ResultSet["studentfname"];
                string StudentLname = (string)ResultSet["studentlname"];
                string StudentNumber = (string)ResultSet["studentnumber"];
                DateTime EnrolDate = (DateTime)ResultSet["enroldate"];

                //creating a new variable and lining it to the models controller 
                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;
                //adding the variables to the empty list.
                Students.Add(NewStudent);
            }
            //Closing the connection once the information is retrieved from the database
            Connection.Close();
            // outputs the lists of students to the web browser
            return Students;
        }

        /// <summary>
        /// this code will retreives single row of  student and  their fields information from the database to be used accordingly
        /// </summary>
        /// <example>GET/StudentData/FindStudents/{id}</example>
        /// <param name="id"> interger as an input as studentId</param>
        /// <returns> A single row of teacher information from the students table</returns>

        [HttpGet]
        public Student FindStudent(int id)
        {
            Student NewStudent = new Student();
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information with the help of id as a parameter input.This will retrive only one row of student data from the students table as its looking up through each teacher id
            Command.CommandText = "Select * from Students where studentid = " + id;
            //Converts the query and  stores it in a variable
            MySqlDataReader ResultSet = Command.ExecuteReader();

            // using while loop to itirate the list information fromt the teachers table.


            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = (string)ResultSet["studentfname"];
                string StudentLname = (string)ResultSet["studentlname"];
                string StudentNumber = (string)ResultSet["studentnumber"];
                DateTime EnrolDate = (DateTime)ResultSet["enroldate"];
                //creating a new variable and lining it to the models controller 
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;




            }
            // outputs a row of one student data from the database to the web browser
            return NewStudent;
        }



        /// <summary>
        /// Deletes Student row from the mySql database in Mamp if the id of that teacher exists.
        /// </summary>
        /// <param name="id"> Student Id</param>
        ///<example>POST : /api/Data/DeleteStudent/3</example>

        [HttpPost]
        public void DeleteStudent(int id)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = "Delete from students where studentid = @id";
            Command.Parameters.AddWithValue("@id", id);
            Command.Prepare();

            Command.ExecuteNonQuery();

            Connection.Close();
        }


        /// <summary>
        /// Adds a student to the exitisting database table 
        /// </summary>
        /// <param name="NewStudent"></param>
        [HttpPost]
        public void AddStudent(Student NewStudent)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = " insert into students (studentfname, studentlname,studentnumber,enroldate) values (@StudentFname,@StudentLname,@StudentNumber,@EnrolDate)";
            Command.Parameters.AddWithValue("@StudentFname", NewStudent.StudentFname);
            Command.Parameters.AddWithValue("@StudentLname", NewStudent.StudentLname);
            Command.Parameters.AddWithValue("@StudentNumber", NewStudent.StudentNumber);
            Command.Parameters.AddWithValue("@EnrolDate", NewStudent.EnrolDate);

            Command.Prepare();

            Command.ExecuteNonQuery();

            Connection.Close();
        }



        /// <summary>
        /// Updates an Student on MySQL
        /// </summary>
        /// <param name="id"></param>
        /// <param name="StudentInfo">An object with the fields that the map to the colums of the teacher's table</param>
        /// <example>
        /// POST/api/StudentData/UpdateStudent/12
        /// FORM DATA/ POST DATA/ REQUEST BODY
         ///  {
        ///"StudentFname":"Jon",
        ///"StudentLname": "Don",
        ///  "StudentNumber": "N6666",
        ///"EnrolDate":"2018-04-02"
        ///}     
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateStudent(int id, [FromBody] Student StudentInfo)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = "update students set studentfname=@StudentFname, studentlname=@StudentLname,studentnumber=@StudentNumber,enroldate=@EnrolDate where studentid=@StudentId";
            Command.Parameters.AddWithValue("@StudentFname", StudentInfo.StudentFname);
            Command.Parameters.AddWithValue("@StudentLname", StudentInfo.StudentLname);
            Command.Parameters.AddWithValue("@StudentNumber", StudentInfo.StudentNumber);
            Command.Parameters.AddWithValue("@EnrolDate", StudentInfo.EnrolDate);
            Command.Parameters.AddWithValue("@StudentId", id);
            Command.Prepare();

            Command.ExecuteNonQuery();

            Connection.Close();
        }

    }
}
