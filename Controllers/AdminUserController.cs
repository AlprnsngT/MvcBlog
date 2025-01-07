using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class AdminUserController : Controller
    {
        //Türkçeleştirme işlemlerini yap
        private mvcBlogDB db = new mvcBlogDB();

        // GET: Adminuser
        public ActionResult Index()
        {
            var user = db.Users.Include(u => u.Authority);
            return View(user.OrderByDescending(u=>u.UserId).ToList());
        }

        // GET: Adminuser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Adminuser/Create
        public ActionResult Create()
        {
            ViewBag.AuthorityId = new SelectList(db.Authoritys, "AuthorityId", "Authority1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Email,Password,NameSurname,AuthorityId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorityId = new SelectList(db.Authoritys, "AuthorityId", "Authority1", user.AuthorityId);
            return View(user);
        }

        // GET: Adminuser/Edit/5
        public ActionResult Edit(int? id)
        {
            //düzenledikten sonra fotoğraf kayboluyor düzelt ya da düzenle bölümünü kaldır
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorityId = new SelectList(db.Authoritys, "AuthorityId", "Authority1", user.AuthorityId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Email,Password,NameSurname,AuthorityId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorityId = new SelectList(db.Authoritys, "AuthorityId", "Authority1", user.AuthorityId);
            return View(user);
        }

        // GET: Adminuser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Adminuser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
