using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using n0454501_Cumulatice_Part3.Models;// using the models folder inorder to access the teacher defining feilds and the database
using MySql.Data.MySqlClient;// connects to the mySql database NuGet package
using System.Web.Http.Cors;
using System.Diagnostics;


namespace n0454501_Cumulatice_Part3.Controllers
{
    public class TeacherDataController : ApiController
    {
        // ShoolDsContext api controller which will connect and get query from the schoolDb database in MAMP 
        private SchoolDbContext School = new SchoolDbContext();

        //This controller will access the teachers table from the schooldb database in MAMP
        /// <summary>
        /// this code will retrive list of authors and  their fields from the database to be used accordingly
        /// </summary>
        /// <example> GET api/TeacherData/ListTeachers </example>
        /// <returns>List of teachers with their related  columns from the teachers table</returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)  // since its a list of students, we have to use IEinumerable

        {     //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();

            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information from teachers table
            // query modified inorder to prevent direct input injection attacks
            Command.CommandText = "select * from teachers where lower(teacherfname) like  lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) or hiredate like @key or salary like @key";


            Command.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            Command.Prepare();
            //COnverts the query and stores it in a variable
            MySqlDataReader ResultSet = Command.ExecuteReader();

            // creates an empty array to store the listo of teachers
            List<Teacher> Teachers = new List<Teacher> { };
            // using while loop o itirate the list information fromt the teachers table 
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                //creating a new variable and lining it to the models controller 
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                //adding the variables to the empty list.
                Teachers.Add(NewTeacher);
            }

            //Closing the connection once the information is retrieved from the database
            Connection.Close();
            // outputs the lists of teachers to the web browser
            return Teachers;
        }


        /// <summary>
        /// this code will retreives single row of  author and  their fields information from the database to be used accordingly 
        /// Joining the Classes table inorder connect the course associated the respective teachers
        /// </summary>
        /// <example>GET/api/TeacherData/FindTeacher/{id}</example>
        /// <param name="id"> interger as an input as studentId</param>
        /// <returns> A single row of teacher information fromt the teachers table</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {

            Teacher NewTeacher = new Teacher();
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query and send it to the database to retrive the information with the help of id as a parameter input.This will retrive only one row of teachers from the teachers table as its looking up hroug each teacher id
            Command.CommandText = "SELECT * FROM teachers where teacherid =" + id;
            //COnverts the query and  stores it in a variable
            MySqlDataReader ResultSet = Command.ExecuteReader();

            // using while loop to itirate the list information fromt the teachers table.
            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                //creating a new variable and lining it to the models controller 
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;



            }


            // outputs a row of one teacher fromt he database to the web browser
            return NewTeacher;
        }



        /// <summary>
        /// Deletes Teacher row from the mySql database in Mamp if the id of that teacher exists.
        /// </summary>
        /// <param name="id"> Teacher Id</param>
        ///<example>POST : /api/TeacherData/DeleteTeacher/3</example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // allows to write a query to delete the selcted teacher with their teacher id from the mysql database
            Command.CommandText = "Delete from teachers where teacherid = @id";
            Command.Parameters.AddWithValue("@id", id);

            //creating a prepared version of the command 
            Command.Prepare();
            //This is a one way query with only server data to return
            Command.ExecuteNonQuery();
            // close the connection between the web server and the database once the action is completed
            Connection.Close();
        }

        /// <summary>
        /// Adds a teacher to the exitisting database table 
        /// </summary>
        /// <param name="NewTeacher"></param>
        [HttpPost]
        public void AddTeacher(Teacher NewTeacher)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server
            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // query modified inorder to prevent direct input injection attacks
            Command.CommandText = "insert into teachers (teacherfname, teacherlname,employeenumber,hiredate,salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber,@HireDate,@Salary)";
            Command.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            Command.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            Command.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            Command.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            Command.Parameters.AddWithValue("@Salary", NewTeacher.Salary);

            //creating a prepared version of the command 
            Command.Prepare();

            //This is a one way query with only server data to return
            Command.ExecuteNonQuery();
            // close the connection between the web server and the database once the action is completed
            Connection.Close();
        }

        /// <summary>
        /// Updates an Author on MySQL
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherInfo">An object with the fields that the map to the colums of the teacher's table</param>
        /// <example>
        /// POST/api/TeacherDATA/uPDATEtEACHER/208
        /// FORM DATA/ POST DATA/ REQUEST BODY
        /// {
        /// "TeacherFname":"Nelson",
        /// "TeacherLname": "Mandela",
        /// "EmployeeNumber": "T844",
        /// "HireDate":"2015-03-02",
        /// "Salary":"40.56"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*",methods: "*",headers: "*")]
        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            //Links and creates a connection to mySql database
            MySqlConnection Connection = School.AccessDatabase();
            //Connection linked and opens between the database and the web server

            Debug.WriteLine(TeacherInfo.TeacherFname);

            Connection.Open();
            //creates a new command to run the query from the database
            MySqlCommand Command = Connection.CreateCommand();
            // query modified inorder to prevent direct input injection attacks
            Command.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname,employeenumber=@EmployeeNumber,hiredate=@HireDate,salary=@Salary where teacherid=@TeacherId" ;
            Command.Parameters.AddWithValue("@TeacherFname",TeacherInfo.TeacherFname);
            Command.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            Command.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            Command.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            Command.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            Command.Parameters.AddWithValue("@TeacherId",id);

            //creating a prepared version of the command 
            Command.Prepare();

            //This is a one way query with only server data to return
            Command.ExecuteNonQuery();
            // close the connection between the web server and the database once the action is completed
            Connection.Close();
        }

    }
}
