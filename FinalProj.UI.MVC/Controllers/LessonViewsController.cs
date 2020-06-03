using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProj.DATA;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace FinalProj.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LessonViewsController : Controller
    {
        private LMSEntities db = new LMSEntities();

        // GET: LessonViews
        public ActionResult Index()
        {
            var lessionViews = db.LessionViews.Include(l => l.Lesson).Include(l => l.UserDetail);
            return View(lessionViews.ToList());
        }

        // GET: LessonViews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessionViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // GET: LessonViews/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle");
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OverrideAuthorization]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public ActionResult SystemCreate(int lessonId, string userId)
        {
            LessonView newLV = new LessonView();
            newLV.LessonId = lessonId;
            newLV.UserId = userId;
            newLV.DateViewed = DateTime.Now;
            string lessonName = db.Lessons.Where(l => l.LessonId == lessonId).Select(l => l.LessonTitle).FirstOrDefault();
            db.LessionViews.Add(newLV);
            db.SaveChanges();
            db.Entry(newLV).GetDatabaseValues();
            TempData["Confirmation"] = $"{lessonName} has been Completed!";
            int courseId = db.Lessons.Where(l => l.LessonId == lessonId).Select(l => l.CourseId).FirstOrDefault();
            int lessonCompleteCount = db.LessionViews.Where(lv => lv.Lesson.CourseId == courseId).Count();
            if (lessonCompleteCount == 6)
            {
                CourseCompletion newComplete = new CourseCompletion();
                newComplete.UserId = userId;
                newComplete.CourseId = courseId;
                newComplete.DateCompleted = DateTime.Now;
                db.CourseCompletions.Add(newComplete);
                db.SaveChanges();
                string courseName = db.Courses.Where(c => c.CourseId == courseId).Select(c => c.CourseName).FirstOrDefault();
                TempData["CourseConfirmation"] = $"{courseName} has been Completed!";
                string name = db.UserDetails.Where(u => u.UserId == userId).FirstOrDefault().FullName;
                string message = $"Your employee {name} has completed {courseName} on {newComplete.DateCompleted:d}";
                string managerEmail = "ryan.martin@vu.com";
                string admin = "admin@ryanmichaelmartin.com";
                MailMessage mm = new MailMessage(admin, managerEmail, $"Course Completed - {name}", message);
                SmtpClient client = new SmtpClient("mail.ryanmichaelmartin.com");
                client.Credentials = new NetworkCredential(admin, "Snake19(");
                try
                {
                    client.Send(mm);
                }
                catch (Exception e)
                {
                    TempData["CourseConfirmation"] = $"Completion email failed to send. Please contact your administrator. <br /> Error Message: <br /> {e.StackTrace}";
                    return RedirectToAction("Index", "Courses");
                }
            }
            return RedirectToAction("Index", "Courses");
        }

        // POST: LessonViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonViewId,UserId,LessonId,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.LessionViews.Add(lessonView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // GET: LessonViews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessionViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // POST: LessonViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonViewId,UserId,LessonId,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessonView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // GET: LessonViews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessionViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // POST: LessonViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LessonView lessonView = db.LessionViews.Find(id);
            db.LessionViews.Remove(lessonView);
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
