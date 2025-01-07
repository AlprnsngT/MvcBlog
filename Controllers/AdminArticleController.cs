using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Web.Helpers;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Net;

namespace MvcBlog.Controllers
{
    public class AdminArticleController : Controller
    {
        mvcBlogDB db = new mvcBlogDB();

        // GET: AdminMakale
        public ActionResult Index(int Page=1 )
        {
            var article = db.Articles.OrderByDescending(m => m.ArticleId).ToPagedList(Page,10);
            return View(article);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Article article, string tickets, HttpPostedFileBase Foto)
        {
            try
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/ArticlesFoto/" + newfoto);
                    makale.Foto= "/Uploads/UsersFoto/" + newfoto;

                    
                    
                }
                if (tickets!=null)
                {
                    string[] ticket = tickets.Split(',');
                    foreach (var i in ticket)
                    {
                        var newTicket = new Etiket { ticketName = i };
                        db.Etikets.Add(newTicket);
                        makale.Etikets.Add(newTicket);
                    }
                }
                article.UserId = Convert.ToInt32(Session["UserId"]);
                article.Read = 1;
                db.Articles.Add(article);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(article);
            }
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var article = db.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", article.CategoryId);
            return View(article);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Foto, Article article)
        {
            try
            {
                var article = db.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(article.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(article.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/ArticleFoto/" + newfoto);
                    article.Foto = "/Uploads/ArticleFoto/" + newfoto;                    
                }
                article.Header = article.Header;
                article.Content = article.Content;
                article.CategoryId = article.CategoryId;
                article.Date = article.Date;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var article = db.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var article = db.Articles.Where(m => m.ArticleId == id).SingleOrDefault();
                if (article == null)
                {
                    return HttpNotFound();
                }

                if (System.IO.File.Exists(Server.MapPath(article.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(article.Foto));
                }

                foreach (var i in article.Comments.ToList())
                {
                    db.Yorums.Remove(i);
                }
                foreach (var  i in article.Tickets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                db.Articles.Remove(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
