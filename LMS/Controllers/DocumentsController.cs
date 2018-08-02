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
			return View(documents.ToList());
		}
		public ActionResult IndexDocumentModule(int id)
		{
			var documents = db.Documents.Where(c => c.ModuleId == id && c.ActivityId == null);
			return View(documents.ToList());
		}


		public ActionResult IndexDocumentActivity(int id)
		{
			

			//var currentActivity = db.Activities.Find(id);
			//var studentDokumentsForActivity = currentActivity.Documents.Where(d => d.User.Roles)

			var TypeId = db.Activities.FirstOrDefault(c => c.Id == id).TypeId;
			var AssignmentId = db.ActivityTypes.FirstOrDefault(m => m.Description == "Assignment").Id;
			
			//Student+ assignment
			if (User.IsInRole("Student") && TypeId == AssignmentId)
			{
				var alldocuments = db.Documents.Where(c => c.ActivityId == id);
				var TeacherRoleId = db.Roles.FirstOrDefault(w => w.Name == "Teacher").Id;
				var teacher = db.Users.Where(w => w.Roles.Select(q => q.RoleId).Contains(TeacherRoleId));
				List<Document> docList = new List<Document>();
				foreach (var item in teacher)
				{
					foreach (var item1 in alldocuments)
					{
						if (item1.UserId == item.Id)
						{
							docList.Add(item1);
						}
					}
				}
				//Teacher+ assignment
				var documents = db.Documents.Where(c => c.ActivityId == id && c.UserId == User.Identity.GetUserId());
				foreach (var item in documents)
				{
					docList.Add(item);
				}
				return View(docList);
			}
		//both without ASSIGNMENT
			else
			{
				var documents = db.Documents.Where(c => c.ActivityId == id);
			}
			return View();
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
				return RedirectToAction("IndexDocumentCourse", "Documents", new { id = document.CourseId });

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
				return RedirectToAction("IndexDocumentModule", "Documents", new { id = document.ModuleId });

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
				return RedirectToAction("IndexDocumentActivity", "Documents", new { id = document.ActivityId });


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
