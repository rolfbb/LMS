using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.ViewModels.Documents;
using Microsoft.AspNet.Identity;

namespace LMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult IndexDocumentCourse(int id)
        {
            var documents = db.Documents.Where(c => c.CourseId == id && c.ModuleId == null && c.ActivityId == null);
            if (Request.IsAjaxRequest())
                return PartialView(documents.ToList());
            return View(documents.ToList());
        }
        public ActionResult IndexDocumentModule(int id)
        {
            var documents = db.Documents.Where(c => c.ModuleId == id && c.ActivityId == null);
            if (Request.IsAjaxRequest())
                return PartialView(documents.ToList());
            return View(documents.ToList());
        }


        public ActionResult IndexDocumentActivity(int id)
        {
            var userlist = db.Users.ToList();
            AllDocuments allDoc = new AllDocuments();
            var documents = db.Documents.Where(c => c.ActivityId == id).ToList();
            var currentActivity = db.Activities.Find(id);
            var TeacherRoleId = db.Roles.FirstOrDefault(m => m.Name == "Teacher").Id;
            var StudentRoleId = db.Roles.FirstOrDefault(m => m.Name == "Student").Id;
            var ActivityTypeId = db.ActivityTypes.FirstOrDefault(m => m.Description == "Assignment").Id;
            //if (currentActivity.TypeId == ActivityTypeId)
            //{
            //var d = User.Identity.GetUserId();
            var studentDokumentsForActivity = currentActivity.Documents.Where(w => w.User.Roles.Select(q => q.RoleId).Contains(StudentRoleId)).ToList();
            var TeacherDokumentsForActivity = currentActivity.Documents.Where(w => w.User.Roles.Select(q => q.RoleId).Contains(TeacherRoleId)).ToList();

            allDoc.UL = userlist;
            allDoc.StudentDoc = studentDokumentsForActivity;
            allDoc.TeacherDoc = TeacherDokumentsForActivity;
            List<AllDocuments> ALLDOCUMENT = new List<AllDocuments>();
            ALLDOCUMENT.Add(allDoc);
            if (Request.IsAjaxRequest())
                return PartialView(ALLDOCUMENT);

            return View(ALLDOCUMENT);
            //}
            //else if (User.IsInRole("Teacher") && currentActivity.TypeId == ActivityTypeId)
            //{
            //	var d = User.Identity.GetUserId();
            //	var studentDokumentsForActivity = db.Documents.Where(c => c.ActivityId == id).ToList();
            //	var TeacherDokumentsForActivity = currentActivity.Documents.Where(w => w.User.Roles.Select(q => q.RoleId).Contains(TeacherRoleId)).ToList();


            //	allDoc.StudentDoc = studentDokumentsForActivity;
            //	allDoc.TeacherDoc = TeacherDokumentsForActivity;
            //	return View(allDoc);
            //}

            //else
            //{
            //	allDoc.STUDENTTEACHERDoc = documents;
            //	return View(documents);
            //}
        }



        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            byte[] Document = db.Documents.Where(w => w.Id == id).Select(c => c.FileContent).FirstOrDefault();
            return File(Document, "pdf");
        }

        // GET: Documents/Details/5
        public ActionResult DetailsCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }
        public ActionResult DetailsModule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }
        public ActionResult DetailsActivity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult UploadDocumentCourse(int id)
        {
            Document doc = new Document()
            {
                CourseId = id,
                UserId = User.Identity.GetUserId()
            };
            if (Request.IsAjaxRequest())
            {
                UploadDocumentViewModel docVM = new UploadDocumentViewModel()
                {
                    //ModuleId = id,
                    CourseId = id,
                    UserId = User.Identity.GetUserId(),
                    UpdateTarget = "Course" + id
                };
                return PartialView(docVM);
            }

            return View(doc);
        }

        // GET: Documents/Create
        public ActionResult UploadDocumentModule(int id)
        {
            var courseId = db.Modules.Where(c => c.Id == id).Select(w => w.CourseId).FirstOrDefault();
            Document doc = new Document()
            {
                ModuleId = id,
                CourseId = courseId,
                UserId = User.Identity.GetUserId()
            };
            //if (Request.IsAjaxRequest())
            //{
            //    UploadDocumentViewModel docVM = new UploadDocumentViewModel()
            //    {
            //        ModuleId = id,
            //        CourseId = courseId,
            //        UserId = User.Identity.GetUserId(),
            //        //UpdateTarget = "Module" + id
            //    };
            //    return PartialView(docVM);
            //}

            return View(doc);
        }

        public ActionResult UploadDocumentActivity(int id)
        {
            var moduleId = db.Activities.Where(c => c.Id == id).Select(w => w.ModuleId).FirstOrDefault();
            var courseId = db.Modules.Where(c => c.Id == moduleId).Select(w => w.CourseId).FirstOrDefault();
            Document doc = new Document()
            {
                ActivityId = id,
                ModuleId = moduleId,
                CourseId = courseId,
                UserId = User.Identity.GetUserId()
            };
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView(doc);
            //}
            return View(doc);
        }
        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocumentCourse([Bind(Include = "Id,Name,Description,UserId,CourseId")] Document document, HttpPostedFileBase FILE)
        {

            if (ModelState.IsValid && FILE != null && FILE.ContentLength > 0)
            {

                Stream str = FILE.InputStream;
                BinaryReader Br = new BinaryReader(str);
                byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                document.TimeStamp = DateTime.Now;
                document.FileContent = FileDet;
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index", "CourseDetails", new { id = document.CourseId });
                //return RedirectToAction("IndexDocumentCourse", "Documents", new { id = document.CourseId });

            }
            else
            {
                ViewBag.file = "Select file Please";
                return View(document);
            }
        }
        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocumentModule([Bind(Include = "Id,Name,Description,UserId,CourseId,ModuleId")] Document document, HttpPostedFileBase FILE)
        {

            if (ModelState.IsValid && FILE != null && FILE.ContentLength > 0)
            {

                Stream str = FILE.InputStream;
                BinaryReader Br = new BinaryReader(str);
                byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                document.TimeStamp = DateTime.Now;
                document.FileContent = FileDet;
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index", "CourseDetails", new { id = document.CourseId });
                //return RedirectToAction("IndexDocumentModule", "Documents", new { id = document.ModuleId });
            }
            else
            {
                ViewBag.file = "Select file Please";
                return View(document);
            }
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocumentActivity([Bind(Include = "Id,Name,Description,UserId,CourseId,ModuleId,ActivityId")] Document document, HttpPostedFileBase FILE)
        {
            if (ModelState.IsValid && FILE != null && FILE.ContentLength > 0)
            {

                Stream str = FILE.InputStream;
                BinaryReader Br = new BinaryReader(str);
                byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                document.TimeStamp = DateTime.Now;
                document.FileContent = FileDet;
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index", "CourseDetails", new { id = document.CourseId });
                //return RedirectToAction("IndexDocumentActivity", "Documents", new { id = document.ActivityId });
            }
            else
            {
                ViewBag.file = "Select file Please";
                return View(document);
            }
        }

        // GET: Documents/Delete/5
        public ActionResult DeleteCourseDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("DeleteCourseDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourseDocument(int id)
        {

            Document document = db.Documents.Find(id);
            var courseid = document.CourseId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("IndexDocumentCourse", new { id = courseid });
        }

        // GET: Documents/Delete/5
        public ActionResult DeleteModuleDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("DeleteModuleDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModuleDocument(int id)
        {
            Document document = db.Documents.Find(id);
            var Moduleid = document.ModuleId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("IndexDocumentModule", new { id = Moduleid });
        }

        // GET: Documents/Delete/5
        public ActionResult DeleteActivityDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("DeleteActivityDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActivityDocument(int id)
        {

            Document document = db.Documents.Find(id);
            var Activityid = document.ActivityId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("IndexDocumentActivity", new { id = Activityid });
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