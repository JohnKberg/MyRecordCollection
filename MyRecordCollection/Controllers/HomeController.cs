using MyRecordCollection.Models;
using MyRecordCollection.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MyRecordCollection.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Take(18).ToList();

            HomePageViewModel vm = new HomePageViewModel() 
            {
                PageTitle = "Welcome to my record collection!",
                PageSubTitle = "A mix of Punk rock, Punk rock and Punk rock",
                Albums = albums
            };

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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