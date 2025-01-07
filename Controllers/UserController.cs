using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace MvcBlog.Controllers
{

    public class UserController : Controller
    {
        mvcBlogDB db = new mvcBlogDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var user = db.Users.Where(u => u.UserId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["UserId"]) != user.UserId)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(User user, string Password)
        {
            var md5pass = Crypto.Hash(Password, "MD5");
            var login = db.Users.Where(u => u.UserName == user.UserName).SingleOrDefault();

            if (login != null && login.UserName == user.UserName && login.Email == user.Email && login.Password == md5pass)
            {
                Session["UserId"] = login.UserId;
                Session["userName"] = login.UserName;
                Session["authorityId"] = login.AuthorityId;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Error = "Kullanıcının bilgileri hatalıdır. kontrol ediniz";
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user, HttpPostedFileBase Foto, string Password)
        {
            var md5pass = Password;
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150,150);
                    img.Save("~/Uploads/UserFoto/" + newfoto);
                    user.Foto = "/Uploads/UserFoto/" + newfoto;

                    user.AuthorityId = 2;
                    user.Password = Crypto.Hash(md5pass, "MD5");
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["UserId"] = user.UserId;
                    Session["userName"] = user.UserName;
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ModelState.AddModelError("Fotoğraf", "Fotoğraf seçiniz");
            }
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var user = db.Users.Where(u => u.UserId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["UserId"]) != user.UserId)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user, int id, HttpPostedFileBase Foto, string Password)
        {
            if (ModelState.IsValid)
            {
                var md5pass = Password;
                var users = db.Users.Where(u => u.UserId == id).SingleOrDefault();
                if (Foto != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(users.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(users.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UserFoto/" + newfoto);
                    user.Photo = "/Uploads/UserFoto/" + newfoto;
                }
                users.FullName = user.FullName;
                users.UserName = user.UserName;
                users.Password = Crypto.Hash(md5pass, "MD5");
                users.Email = user.Email;
                db.SaveChanges();
                Session["userName"] = user.UserName;
                return RedirectToAction("Index", "Home", new { id = users.UserId });

            }
            return View();
        }
        public ActionResult Profile(int id)
        {
            var user = db.Users.Where(u => u.UserId == id).SingleOrDefault();
            return View(user);
        }
    }
}