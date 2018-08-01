using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LMS.Models;
using LMS.ViewModels;
using LMS.ViewModels.Activity;

namespace LMS.Controllers
{
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index(/*int? ID*/)
        {
            var activities = db.Activities/*.Where(m => m.ModuleId == ID)*/;
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        // For now we assume we have a correct moduleId, otherwise we need mode che
        public ActionResult Create(int? moduleId)
        {
            var module = db.Modules.FirstOrDefault(m => m.Id == moduleId);
            ActivityCreateViewModel model = new ActivityCreateViewModel()
            {
                Name = "",
                Description = "",
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                ModuleId = module.Id,
                CourseId = module.CourseId,
                //SelectModule = new SelectList(db.Modules, "Id", "Name", moduleId),
                SelectType = new SelectList(db.ActivityTypes, "Id", "Description")
            };
            return View(model);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,TypeId,ModuleId")] Activity activity,int CourseId)
        {
            if (ModelState.IsValid)
            {
                //Activity activity = Mapper.Map<ActivityEditViewModel,Activity>(activityVM);
                var module = db.Modules.FirstOrDefault(m => m.Id == activity.ModuleId);
                if (Util.Validation.DateRangeValidation(this, module, activity))
                {
                    db.Activities.Add(activity);
                    db.SaveChanges();
                    return RedirectToAction("Index", "CourseDetails", new { id = module.CourseId });
                }               
            }

            ActivityCreateViewModel model = Mapper.Map<Activity, ActivityCreateViewModel>(activity);
            //model.SelectModule = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            model.CourseId = CourseId;
            model.SelectType = new SelectList(db.ActivityTypes, "Id", "Description", activity.TypeId);
            return View(model);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);

            if (activity == null)
            {
                return HttpNotFound();
            }

            ActivityEditViewModel model = Mapper.Map<Activity, ActivityEditViewModel>(activity);
            var module = db.Modules.FirstOrDefault(m => m.Id == activity.ModuleId);
            model.CourseId = module.CourseId;
                
            //model.ModuleId = activity.ModuleId;
            //model.SelectModule = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            model.SelectType = new SelectList(db.ActivityTypes, "Id", "Description", activity.TypeId);
            return View(model);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,TypeId,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var module = db.Modules.FirstOrDefault(m => m.Id == activity.ModuleId);
                if (Util.Validation.DateRangeValidation(this, module, activity))
                {
                    db.Entry(activity).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "CourseDetails", new { id = module.CourseId });
                }
                activity.Module = module;
            }
            ActivityEditViewModel model = Mapper.Map<Activity, ActivityEditViewModel>(activity);
            //model.SelectModule = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            model.SelectType = new SelectList(db.ActivityTypes, "Id", "Description", activity.TypeId);
            return View(model);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var documents = db.Documents.Where(m => m.ActivityId == id);
            db.Documents.RemoveRange(documents);
            db.SaveChanges();

            Activity activity = db.Activities.Find(id);
            Module module = db.Modules.FirstOrDefault(m => m.Id == activity.ModuleId);
            db.Activities.Remove(activity);
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
