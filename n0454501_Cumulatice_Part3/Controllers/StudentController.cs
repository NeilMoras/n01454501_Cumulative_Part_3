using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n0454501_Cumulatice_Part3.Models; ///using student models field as a reference
using System.Diagnostics; // tool used for debugging

namespace n0454501_Cumulatice_Part3.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET: Student/List
        public ActionResult List(string Searchkey = null)
        {
            Debug.WriteLine("The Search key the is inputted is ");
            Debug.WriteLine(Searchkey);
            // get accesss from the Students dataController
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudents(Searchkey);
            return View(Students);
        }


        //GET: Student/Show/{id}
        public ActionResult Show(int id)
        {

            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);
            return View(NewStudent);
        }


        //GET: Student/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);

            return View(NewStudent);
        }


        //GET: Student/Delete/{id}
        public ActionResult Delete(int id)
        {
            StudentDataController controller = new StudentDataController();
            controller.DeleteStudent(id);

            return RedirectToAction("List");
        }

        //GET:/Student/New

        public ActionResult New()
        {
            return View();
        }


        //POST : /Student/Add
        [HttpPost]
        public ActionResult Add(string StudentFname, string StudentLname, string StudentNumber, DateTime EnrolDate)
        {
            //Indentify the inputs are proivded from the form
            Student NewStudent = new Student();
            NewStudent.StudentFname = StudentFname;
            NewStudent.StudentLname = StudentLname;
            NewStudent.StudentNumber = StudentNumber;
            NewStudent.EnrolDate = EnrolDate;

            StudentDataController controller = new StudentDataController();
            controller.AddStudent(NewStudent);

            return RedirectToAction("List");
        }

        [HttpGet]
        //GET : /Student/Update/{id}
        /// <summary>
        /// sends a get request to display the information of the student
        /// </summary>
        /// <param name="id">student Id</param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {
            // get accesss from the Student dataController
            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(id);
            return View(SelectedStudent);
        }

        [HttpPost]
        //POST : /Student/Update/{id}
        /// <summary>
        /// Receiving a POST request from the webserver to the database wuth information of the existing student ,updating the values.
        /// It sends the inofrmation to the API and then redirects it to the "Student/Show page of the updated teacher
        /// </summary>
        /// <param name="id"> Id of the student to update</param>
        /// <param name="StudentFname"> Updated student first namer</param>
        /// <param name="StudentLname">Updated student last name</param>
        /// <param name="StudentNumber">Updated student  number</param>
        /// <param name="EnrolDate">Updated student Hiring Date</param>
      
        /// <returns>dynamic updated webpage of the existing student</returns>
        /// <example>POST: /Student/Update/20
        /// 
        /// FORM DATA/ POST DATA/ REQUEST BODY
        /// {
        /// "StudentFname":"Jon",
        /// "StudentLname": "Don",
        /// "StudentNumber": "N6666",
        /// "EnrolDate":"2018-04-02"
        /// }
        /// </example>
        public ActionResult Update(int id, string StudentFname, string StudentLname, string StudentNumber, DateTime EnrolDate)
        {
            //Indentify the inputs are provided from the form
            Student StudentInfo = new Student();
            StudentInfo.StudentFname = StudentFname;
            StudentInfo.StudentLname = StudentLname;
            StudentInfo.StudentNumber = StudentNumber;
            StudentInfo.EnrolDate = EnrolDate;
            StudentDataController controller = new StudentDataController();
            controller.UpdateStudent(id, StudentInfo);
            return RedirectToAction("Show/" + id);
        }
    }
}