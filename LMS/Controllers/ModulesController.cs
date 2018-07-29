using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LMS.Models;
using LMS.ViewModels.Module;

namespace LMS.Controllers
{
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public ActionResult Create(int? courseId)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
            ModuleCreateViewModel model = new ModuleCreateViewModel()
            {
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CourseId = course.Id,
                //SelectCourse = new SelectList(db.Modules, "Id", "Name", course.Id),
            };
            return View(model);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                var course = db.Courses.FirstOrDefault(c => c.Id == module.CourseId);
                if (Util.Validation.DateRangeValidation(this, course, module))
                {
                    db.Modules.Add(module);
                    db.SaveChanges();
                    return RedirectToAction("Index", "CourseDetails", new { id = module.CourseId });
                }
            }
            ModuleCreateViewModel model = Mapper.Map<Module, ModuleCreateViewModel>(module);
            //model.SelectCourse = new SelectList(db.Modules, "Id", "Name", module.Id);
            return View(model);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            //ModuleEditViewModel model = Mapper.Map<Module, ModuleEditViewModel>(module);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                var course = db.Courses.FirstOrDefault(c => c.Id == module.CourseId);
                if (Util.Validation.DateRangeValidation(this, course, module))
                {
                    db.Entry(module).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "CourseDetails", new { id = course.Id });
                }
            }
            //ModuleEditViewModel model = Mapper.Map<Module, ModuleEditViewModel>(module);
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index", "CourseDetails", new { id = module.CourseId });
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
