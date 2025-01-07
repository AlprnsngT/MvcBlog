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
    public class AdminCommentController : Controller
    {
        private mvcBlogDB db = new mvcBlogDB();
        
        // GET: Admincomment
        public ActionResult Index()
        {
            var comment = db.Comments.Include(y => y.Article).Include(y => y.Uye);
            return View(comment.OrderByDescending(y=>y.CommentId).ToList());
        }

        // GET: Admincomment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Admincomment/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Header");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Content,UserId,ArticleId,Date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Header", comment.ArticleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "UserName", comment.UserId);
            return View(comment);
        }

        // GET: Admincomment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Header", comment.ArticleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "UserName", comment.UserId);
            return View(comment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Content,UserId,ArticleId,Date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "ArticleId", "Header", comment.ArticleId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "UserName", comment.UserId);
            return View(comment);
        }

        // GET: Admincomment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Admincomment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
