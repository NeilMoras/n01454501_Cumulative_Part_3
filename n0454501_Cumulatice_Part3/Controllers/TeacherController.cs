using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n0454501_Cumulatice_Part3.Models;///using teachers models field as a reference
using System.Diagnostics;// tool used for debugging


namespace n0454501_Cumulatice_Part3.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET: Teacher/List
        public ActionResult List(string Searchkey = null)
        {
            Debug.WriteLine("The Search key the is inputted is ");
            Debug.WriteLine(Searchkey);
            // get accesss from the Teachers dataController
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(Searchkey);
            return View(Teachers);
        }

        //GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {// get accesss from the Teachers dataController
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        //GET: Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {// get accesss from the Teachers dataController
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }


        //GET: Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {// get accesss from the Teachers dataController
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);

            return RedirectToAction("List");
        }

        //GET:/Teacher/New

        public ActionResult New()
        {
            //returns it to the new view
            return View();
        }




        //POST : /Teacher/Add
        [HttpPost]
        public ActionResult Add(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            //Indentify the inputs are provided from the form
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;
            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);
            //redirected the user to the list page once the teacher is added to the database and the list is updated
            return RedirectToAction("List");
        }

        [HttpGet]
        //GET : /Teacher/Update/{id}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {
            // get accesss from the Teachers dataController
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);
            return View(SelectedTeacher);
        }

        [HttpPost]
        //POST : /Teacher/Update/{id}
        /// <summary>
        /// Receiving a POST request from the webserver to the database wuth information of the existing teacher ,updating the values.
        /// It sends the inofrmation to the API and then redirects it to the "Teacher/Show page of the updated teacher
        /// </summary>
        /// <param name="id"> Id of the teacher to update</param>
        /// <param name="TeacherFname"> Updated teacher first namer</param>
        /// <param name="TeacherLname">Updated last name</param>
        /// <param name="EmployeeNumber">Updated employee Id</param>
        /// <param name="HireDate">Updated Hiring Date</param>
        /// <param name="Salary">Updated Salary of the Teacher</param>
        /// <returns>dynamic updated webpage of the existing teacher</returns>
        /// <example>POST: /Teacher/Update/20
        /// 
        /// FORM DATA/ POST DATA/ REQUEST BODY
        /// {
        /// "TeacherFname":"Nelson",
        /// "TeacherLname": "Mandela",
        /// "EmployeeNumber": "T844",
        /// "HireDate":"2015-03-02",
        /// "Salary":"40.56"
        /// }
        /// </example>
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            //Indentify the inputs are provided from the form
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.HireDate = HireDate;
            TeacherInfo.Salary = Salary;
            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);
            return RedirectToAction("Show/" + id);
        }
    }
}