using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyRecordCollection.Models;
using MyRecordCollection.ViewModels;

namespace MyRecordCollection.Controllers
{
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            var vm = new ArtistFormViewModel();
            
            return View("ArtistForm", vm);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ArtistFormViewModel artistVm)
        {
            if (!ModelState.IsValid)
            {
                return View("ArtistForm", artistVm);            
            }

            if (artistVm.Id == 0)
            {
                // CREATE
                var artist = new Artist 
                {  
                    Id = 0,
                    Name = artistVm.Name,
                    Bio = artistVm.Bio,
                    Albums = new List<Album>()
                };
                db.Artists.Add(artist);
                db.SaveChanges();
            }
            else
            {
                //EDIT
                db.Entry(artistVm).State = EntityState.Modified;
                db.SaveChanges();
            }
                
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
