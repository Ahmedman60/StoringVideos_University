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
using System.IO;
namespace StoringVideos.Controllers
{
    public class BlobsController : Controller
    {//Copy right Mohamed Fathallah
        private VideoCollageEntities db = new VideoCollageEntities();

        #region getingtheCourses
        public JsonResult GetCourses(string DoctorID)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                int docit = Convert.ToInt32(DoctorID);
                var Courses = db.Courses.Where(e => e.DoctorId == docit).ToList();
                //ViewBag.AreasID = new SelectList(db.Areas, "Areaid", "Name");
                return Json(Courses, JsonRequestBehavior.AllowGet);
            }
            catch (FormatException)
            {
                this.Dispose();
                return null;
            }
        }
        #endregion

        public ActionResult MyAction(string FilePath)
        {
            try
            {
                FilePath = Path.Combine(Server.MapPath("~/Uploads/Videos/") + FilePath);
                byte[] myVideo = System.IO.File.ReadAllBytes(FilePath);
                return new FileContentResult(myVideo, "video/*");
            }
            catch (Exception)
            {
                return View("_NotFoundImage");
            }


        }
        // GET: Blobs
        public async Task<ActionResult> Index()
        {
            return View(await db.Blobs.ToListAsync());
        }

        // GET: Blobs/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blob blob = await db.Blobs.FindAsync(id);
            if (blob == null)
            {
                return HttpNotFound();
            }
            return View(blob);
        }
        [Authorize(Roles ="Admin")]
        // GET: Blobs/Create
        public ActionResult Create()
        {
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorName");
           
            return View();
        }

        // POST: Blobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase VideoPath,HttpPostedFileBase FilePath ,string CourseID)
        {
            if (FilePath.ContentType == "application/pdf" || FilePath.ContentType == "application/docx" ||FilePath.ContentType == "application/pptx")
            {
                if (VideoPath == null && FilePath == null)
                {
                    ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorName");
                    ViewData["Message"] = "You Didn't select anything";
                    return RedirectToAction("Create");
                }
            }
            else
            {
                ViewData["Message"] = "Select PDF or DOCX or PPTX (Powerpoint,World,PDF)";
                ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorName");
                return View("~/Views/Blobs/Create.cshtml");
            }
            Blob blob = new Blob();
            if(FilePath!=null)
            {

                byte[] Filedata = new byte[FilePath.ContentLength];
                FilePath.InputStream.Read(Filedata, 0, FilePath.ContentLength);
                blob.FilePath = Filedata;
                blob.FileName = Path.GetFileNameWithoutExtension(FilePath.FileName);
            }
            if (VideoPath != null)
            {
                if (VideoPath.ContentLength < 104857600)
                {
             
                    string _Filename = "";
                    string _Extenstion = Path.GetExtension(VideoPath.FileName);
                    //i will change the name of the video to name that can't be replicated
                    _Filename = VideoPath.FileName + DateTime.Now.ToString("yyMMddhhssfff") + _Extenstion;
                    blob.Path = _Filename;
                    blob.Name = Path.GetFileNameWithoutExtension(VideoPath.FileName);
                    
                    //the Path In server that will be stoed
                    _Filename = Path.Combine(Server.MapPath("~/Uploads/Videos/") + _Filename);
                    //if you want to restrict the size of the video make path.contectlleght<200mb for example but i will not do that (100mb)
                    VideoPath.SaveAs(_Filename);
                    

                    
                }                 
            }
            if (CourseID != "")
            {
                blob.CourseID = int.Parse(CourseID);
            }
            else
            {
                blob.CourseID = 3;
            }
          

            db.Blobs.Add(blob);
            await db.SaveChangesAsync();
            //ViewData["Message"] = "Recorded Saved Successfully";

            return RedirectToAction("Index");
        }
        
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Admin()
        {
            return View("~/Views/Blobs/Admin.cshtml", await db.Blobs.ToListAsync());
        }

        // GET: Blobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blob blob = await db.Blobs.FindAsync(id);
            if (blob == null)
            {
                return HttpNotFound();
            }
            return View(blob);
        }

        // POST: Blobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,type,Path")] Blob blob)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blob).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blob);
        }

        // GET: Blobs/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Error400s", "CustomError");
            }
            Blob blob = await db.Blobs.FindAsync(id);
            if (blob == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Error404s", "CustomError");
            }
            return View(blob);
        }

        // POST: Blobs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //delete from the database first then delete the video in the file in the server so it will not take a space .
            Blob blob = await db.Blobs.FindAsync(id);
            string path = Path.Combine(Server.MapPath("~/Uploads/Videos/") + blob.Path);
            System.IO.File.Delete(path);
            db.Blobs.Remove(blob);
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
        public FileResult DownLoadFile(int id)
        {
            var FileById = db.Blobs.Find(id);
            return File(FileById.FilePath,"application/pdf", FileById.FileName);
        }
        [HttpPost]
        public ActionResult SearchSubjectName(string Search,string type)
        {
            if (type=="Doctor Name")
            {
                var SearchProduct = db.Blobs.Where(q => q.Cours.Doctor.DoctorName.ToLower().StartsWith(Search.ToLower()));
                return View("Index", SearchProduct.ToList());
            }
            else if(type=="Course Name")
            {
                var SearchProduct = db.Blobs.Where(q => q.Cours.CourseName.ToLower().StartsWith(Search.ToLower()));
                return View("Index", SearchProduct.ToList());
            }
            else if(type=="Video Name")
            {
                var SearchProduct = db.Blobs.Where(q => q.Name.ToLower().StartsWith(Search.ToLower()));
                return View("Index", SearchProduct.ToList());
            }
            else if(type=="File Name")
            {
                var SearchProduct = db.Blobs.Where(q => q.FileName.ToLower().StartsWith(Search.ToLower()));
                return View("Index", SearchProduct.ToList());
            }
            else
            {
                return View("Index",new List<Blob>(){ });
            }
            
           

        }

    }
}
