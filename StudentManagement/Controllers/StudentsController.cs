using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;
using PagedList.Mvc;
using PagedList; 


namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private StudentManagementEntities db = new StudentManagementEntities();

        // GET: Students
        public ActionResult Index(string txtSearch, string txtClass, string txtRank, int? i, string orderbyRanks)
        {
            getClass();  
            getRanks(); 
            orderbyRank();
            if (orderbyRanks == "Thấp -> Cao")
            {
                var desc = from student in db.Students
                           orderby student.Rank descending
                           select student;
                return View(desc.ToList().ToPagedList(i ?? 1, 10));
            }
            else if (orderbyRanks == "Cao -> Thấp")
            {
                var asc = from student in db.Students
                          orderby student.Rank
                          select student;
                return View(asc.ToList().ToPagedList(i ?? 1, 10));
            } 

            return View(db.Students
                .Where(s => s.Class.Contains(txtClass) && (s.Name.Contains(txtSearch) || s.Address.Contains(txtSearch))
                && s.Rank.Contains(txtRank) || txtClass == null || txtRank == null || txtSearch == null)
                .ToList().ToPagedList(i ?? 1, 10));
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            return View(await db.Students.ToListAsync());
        }

        // GET: Students/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = await db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}

        // GET: Students/Create
        public void getClass()
        {
            List<SelectListItem> listClass = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "10", Value = "10" },
                new SelectListItem() { Text = "11", Value = "11" },
                new SelectListItem() { Text = "12", Value = "12" }
            };
            ViewBag.Class = listClass;
        }

        public void orderbyRank()
        {
            List<SelectListItem> listOrderbyRank = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Cao -> Thấp", Value = "Cao -> Thấp"},
                new SelectListItem() {Text = "Thấp -> Cao", Value = "Thấp -> Cao"}
            }; 
            ViewBag.orderbyRank = listOrderbyRank;
        }

        public void getRanks()
        {
            List<SelectListItem> listRanks = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Yếu", Value = "Yếu" },
                new SelectListItem() { Text = "Trung Bình", Value = "Trung Bình"},
                new SelectListItem() { Text = "Khá", Value = "Khá" },
                new SelectListItem() { Text = "Giỏi", Value = "Giỏi" }
            };
            ViewBag.Ranks = listRanks;
        }

        public ActionResult Create()
        {
            getClass();
            getRanks();

            return View();
        }
         
        public ActionResult Reset()
        {
            return View(); 
        }


        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,BirthDate,Address,Class,Rank")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            getClass();
            getRanks();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,BirthDate,Address,Class,Rank")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = await db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student); 
        //} 

        // POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Student student = await db.Students.FindAsync(id);
        //    db.Students.Remove(student);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index"); 
        //}

        public JsonResult DeleteStudent(int StudentId)
        {
            bool result = false;
            Student std = db.Students.SingleOrDefault(s => s.Id == StudentId);
            if (std != null)
            {
                db.Students.Remove(std);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
