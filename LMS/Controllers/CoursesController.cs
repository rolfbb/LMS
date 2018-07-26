using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            HeaderViewModel header = new HeaderViewModel()
            {
                Empty = true
            };
            CoursesViewModel model = new CoursesViewModel()
            {
                //Default active courses
                Courses = db.Courses.Where(c => c.EndDate >= DateTime.Now).ToList(),
                Header = header
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string active)
        {
            if (active == "on")
            {
                    return PartialView("_Courses", db.Courses.Where(c => c.EndDate >= DateTime.Now).ToList());
            }
            else
            {
                    return PartialView("_Courses", db.Courses.ToList());
            }
        }

       

        // GET: Courses/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index","CourseDetails",new {id=course.Id});
            }
           
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
