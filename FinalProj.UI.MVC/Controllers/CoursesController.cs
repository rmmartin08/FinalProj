﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProj.DATA;

namespace FinalProj.UI.MVC.Controllers
{
    public class CoursesController : Controller
    {
        private LMSEntities db = new LMSEntities();

        // GET: Courses
        [Authorize(Roles = "Admin, Manager, Employee")]
        public ActionResult Index()
        {
            return View(db.Courses.ToList().OrderBy(c => c.CourseName));
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Admin, Manager, Employee")]
        public ActionResult Details(int? id)
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

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,CourseDescription,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,CourseDescription,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                bool isActive = db.Courses.Where(c => c.CourseId == course.CourseId).Select(c => c.IsActive).FirstOrDefault();
                if (course.IsActive != isActive)
                {
                    List<Lesson> allLessons = db.Lessons.Where(l => l.CourseId == course.CourseId).ToList();
                    if (course.IsActive == false)
                    {
                        foreach (var lesson in allLessons)
                        {
                            if (lesson.isActive == true)
                            {
                                lesson.isActive = !lesson.isActive;
                            }
                        }
                    }
                    else
                    {
                        db.Entry(course).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.CourseId = course.CourseId;
                        return View("LessonsSelection");
                    }
                }
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }



        [HttpPost]
        public void UpdateActiveCourseLessons(List<int> list)
        {
            if (list[0] != 0)
            {
                foreach (var listItem in list)
                {
                    Lesson lesson = db.Lessons.Where(l => l.LessonId == listItem).FirstOrDefault();
                    lesson.isActive = true;
                    db.SaveChanges();
                }
            }
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
