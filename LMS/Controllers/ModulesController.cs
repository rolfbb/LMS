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
            var nrOfDocs = module.Documents.Count();
            if (module == null)
            {
                return HttpNotFound();
            }
            ModuleEditViewModel model = Mapper.Map<Module, ModuleEditViewModel>(module);
            model.DatabaseModified = "DbUnchanged";
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Edit", model);
            }
            return View("_Edit", model);
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
                    if (Request.IsAjaxRequest())
                    {
                        //module.Activities = db.Activities.Where(act => act.ModuleId == module.Id).ToList();
                        //var moduleDb = db.Modules.Include(t=> t.Documents).FirstOrDefault(m=> m.Id == module.Id);
                        //moduleDb.Documents= db.Documents.Where(doc)
                        var moduleDb = db.Modules.Find(module.Id);
                        var documents = db.Documents.Where(doc => doc.ModuleId == moduleDb.Id).ToList();
                        moduleDb.Documents = documents;
                        var nrOfDocs = moduleDb.Documents.Count();
                        ModuleViewModel viewModel = new ModuleViewModel()
                        {
                            Module = moduleDb,
                            CollapseId = "collapse" + module.Id
                        };
                        return PartialView("_ModuleInfoEditDel", viewModel);
                        
                        //ModuleEditViewModel moduleEditVM = Mapper.Map<Module, ModuleEditViewModel>(module);
                        //moduleEditVM.DatabaseModified = "DbChanged";
                        //return PartialView("_Edit", moduleEditVM);
                    }
                    return RedirectToAction("Index", "CourseDetails", new { id = course.Id });
                }
            }
            ModuleEditViewModel model = Mapper.Map<Module, ModuleEditViewModel>(module);
            model.DatabaseModified = "DbUnchanged";
            return View("_Edit",model);
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
            return PartialView("_Delete",module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var documents = db.Documents.Where(m => m.ModuleId == id);
            db.Documents.RemoveRange(documents);
            db.SaveChanges();

            var activities = db.Activities.Where(a => a.ModuleId == id);
            db.Activities.RemoveRange(activities);
            db.SaveChanges();

            Module module = db.Modules.FirstOrDefault(m => m.Id == id);
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
