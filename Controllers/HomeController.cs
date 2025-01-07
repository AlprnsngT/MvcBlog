using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using PagedList;
using PagedList.Mvc;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        mvcBlogDB db = new mvcBlogDB();
        // GET: Home
        public ActionResult Index(int page=1)
        {
            var article = db.Articles.OrderByDescending(m => m.ArticleId).ToPagedList(page, 5);
            return View(article);
        }
        public ActionResult SearchBlog(string getSearch=null)
        {
            var search = db.Articles.Where(m => m.Header.Contains(getSearch)).ToList();
            return View(search.OrderByDescending(m=>m.Date));
        }
        public ActionResult LastComments()
        {

            return View(db.Comments.OrderByDescending(y=>y.UserId).Take(5));
        }
        public ActionResult PopArticles()
        {
            return View(db.Articles.OrderByDescending(m=>m.Read).Take(5));
        }
        public ActionResult CategoryAndArticle(int id, int Page=1)
        {
            var article = db.Articles.Where(m => m.CategoryId == id).ToList();
            return View(article.OrderByDescending(m=>m.Date).ToPagedList(Page,5));
        }
        public ActionResult ArticleDetails(int id)
        {
            var article = db.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (article==null)
            {
                return HttpNotFound();
            }
            return View(article);

        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Communication()
        {

            return View();
        }

        public ActionResult CategoryPartial()
        {
            return View(db.Categories.ToList());
        }
        public JsonResult AddComment(string comment, int ArticleId)
        {
            var UserId = Session["UserId"];
            if (comment == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            db.Comments.Add(new Comment { UserId = Convert.ToInt32(UserId), ArticleId = ArticleId, Content = comment, Date = DateTime.Now });
            db.SaveChanges();

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteComment(int id)
        {
            var UserId = Session["UserId"];
            var comment = db.Comments.Where(y => y.CommentId == id).SingleOrDefault();
            var article = db.Articles.Where(m => m.ArticleId == comment.ArticleId).SingleOrDefault();
            if (comment.UserId==Convert.ToInt32(UserId))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("ArticleDetail", "Home", new { id = article.ArticleId });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult OkunmaArtir(int ArticleId)
        {
            var article = db.Articles.Where(m => m.ArticleId == ArticleId).SingleOrDefault();
            article.Read += 1;
            db.SaveChanges();
            return View();
        }
    }
}