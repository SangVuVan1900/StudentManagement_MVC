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
using System.Text;
using System.Security.Cryptography;

namespace StudentManagement.Controllers
{
    public class AccountsController : Controller
    {
        private StudentManagementEntities db = new StudentManagementEntities();

        // GET: Accounts
        public ActionResult Login()
        {

            return View();
        }
        // encode, decode
        //https://www.youtube.com/watch?v=r94gKb-NzLM
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (!string.IsNullOrEmpty(account.Username) && !string.IsNullOrEmpty(account.Password))
            {
                string passwordEncode = EncodePassword(account.Password);
                var obj = db.Accounts
                    .Where(a => a.Username.Equals(account.Username) && a.Password.Equals(passwordEncode))
                    .FirstOrDefault();
                if (obj != null)
                {
                    Session["UserName"] = account.Username.Trim();
                    return RedirectToAction("Index", "Students");
                }
                else
                {
                    ViewBag.Valid = "Thông tin đăng nhập không hợp lệ!";
                    return View(account);
                }
            }
            else
            {
                ViewBag.Valid = "Thông tin đăng nhập không hợp lệ!";
                return View(account);
            }

        }

        public string EncodePassword(string txtPassword)
        {
            string text = "";
            byte[] toEncode = ASCIIEncoding.ASCII.GetBytes(txtPassword);
            text = Convert.ToBase64String(toEncode);
            return text;
        }

        public ActionResult Register()
        {
            return View();
        }
         
        [HttpPost] 
        public async Task<ActionResult> Register
            ([Bind(Include = "Id,Username,Password,ConfirmPassword,Question")] Account1 account1)
        {
            if (ModelState.IsValid)    
            {
                Account account = new Account()
                { 
                    Username = account1.Username,
                    Password = account1.Password,
                    Question = account1.Question
                };
                string password = EncodePassword(account.Password);
                account.Password = password; 
                db.Accounts.Add(account); 
                await db.SaveChangesAsync(); 
                return RedirectToAction("Login");
            }

            return View(account1);
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        } 

       
        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Username,Password,Question")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(account);
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
