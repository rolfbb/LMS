using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LMS.Models;
using LMS.ViewModels.Activity;
using LMS.ViewModels.Course;
using LMS.ViewModels.Module;
using Microsoft.AspNet.Identity;

namespace LMS.Controllers
{
    [Authorize(Roles = "Teacher, Student")]
    public class CourseDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseDetails/1
        public ActionResult Index(int? id)
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

            var modulesVM = new List<ModuleViewModel>();
            //Store all assignmnents where deadline has been passes
            //List<Activity> assignmentsWithPassedDeadline = new List<Activity>();
            var userName = User.Identity.GetUserName();
            foreach (var module in course.Modules)
            {
                ModuleViewModel moduleVM = Mapper.Map<Module, ModuleViewModel>(module);
                List<ActivityViewModel> activitiesVM = new List<ActivityViewModel>();
                foreach (var activity in module.Activities)
                {
                    //if (activity.Type.Description == "Assignment" && activity.EndDate < DateTime.Now)
                    //assignmentsWithPassedDeadline.Add(activity);
                    var activityVM = Mapper.Map<Activity, ActivityViewModel>(activity);
                    if (activity.Type.Description == "Assignment" && User.IsInRole("Student"))
                    {
                        var studDocs = db.Documents.Where(doc => doc.ActivityId == activity.Id && doc.User.UserName == userName);
                        activityVM.StudentUploadedSolution = studDocs.Any();
                        if (activityVM.StudentUploadedSolution)
                            activityVM.StudentMissedDeadline = studDocs.FirstOrDefault().TimeStamp > activity.EndDate;
                    }

                    activityVM.Description = activity.Type.Description;
                    activityVM.NrOfDocuments = db.Documents.Count(doc => doc.ActivityId == activity.Id);
                    activitiesVM.Add(activityVM);
                }
                moduleVM.ActivitiesVM = activitiesVM;
                moduleVM.NrOfDocuments = db.Documents.Count(doc => doc.ModuleId == module.Id && doc.ActivityId == null);
                modulesVM.Add(moduleVM);
            }

            CourseViewModel courseVM = Mapper.Map<Course, CourseViewModel>(course);
            courseVM.ModulesVM = modulesVM;
            return View(courseVM);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = course.Id });
            }
            return View(course);
        }
    }
}