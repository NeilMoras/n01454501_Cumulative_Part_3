using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n0454501_Cumulatice_Part3.Models;

namespace n0454501_Cumulatice_Part3.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show list of authors connectiing to the views folder
        /// </summary>
        /// <param name="id">int as ID</param>
        /// <returns>List of class rendered to the list page</returns>
        //GET Class/List
        public ActionResult List()
        {

            ClassesDataController controller = new ClassesDataController();
            IEnumerable<Class> Classes = controller.ListClasses();


            return View(Classes);
        }

        /// <summary>
        /// Show particluar selected class details connecting to the views folder
        /// </summary>
        /// <param name="id">int as ID</param>
        /// <returns>class detials of that selected class</returns>
        ///  //GET Class/Show/{id}
        public ActionResult Show(int id)
        {

            ClassesDataController controller = new ClassesDataController();
            Class NewClass = controller.FindClass(id);

            return View(NewClass);
        }





        [HttpPost]
        // POST Class/AddSecondaryKey
        public ActionResult AddSecondaryKey()
        {
            ClassesDataController controller = new ClassesDataController();
            controller.AddForeignKey();

            return RedirectToAction("List");


        }
    }
}