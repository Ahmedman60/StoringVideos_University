using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using storingVedios2.Models;

namespace StoringVideos.Controllers
{[Authorize(Roles ="Admin")]
    public class CoursController : Controller
    {
        private VideoCollageEntities db = new VideoCollageEntities();

        // GET: Cours
        public async Task<ActionResult> Index()
        {
            var courses = db.Courses.Include(c => c.Doctor);
            return View(await courses.ToListAsync());
        }

        // GET: Cours/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = await db.Courses.FindAsync(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // GET: Cours/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorID", "DoctorName");
            return View();
        }

        // POST: Cours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseID,CourseName,DoctorId")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(cours);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorID", "DoctorName", cours.DoctorId);
            return View(cours);
        }

        // GET: Cours/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = await db.Courses.FindAsync(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorID", "DoctorName", cours.DoctorId);
            return View(cours);
        }

        // POST: Cours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseID,CourseName,DoctorId")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cours).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorID", "DoctorName", cours.DoctorId);
            return View(cours);
        }

        // GET: Cours/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = await db.Courses.FindAsync(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Cours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cours cours = await db.Courses.FindAsync(id);
            db.Courses.Remove(cours);
            await db.SaveChangesAsync();
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
