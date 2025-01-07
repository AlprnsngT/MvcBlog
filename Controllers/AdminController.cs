using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class AdminController : Controller
    {
        mvcBlogDB db = new mvcBlogDB();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Article = db.Articles.Count();
            ViewBag.Comment = db.Comments.Count();
            ViewBag.Category = db.Categories.Count();
            ViewBag.User = db.Users.Count();
            return View();
        }
    }
}