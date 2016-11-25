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
using AutoMapper;
using PagedList;

namespace MyRecordCollection.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        [AllowAnonymous]
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            int PageSize = 5;
            int PageNumber = (page ?? 1);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<Artist> artists = db.Artists.OrderBy(a => a.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(a => a.Name.Contains(searchString)).OrderBy(a => a.Name);
            }

            return View(artists.ToPagedList(PageNumber, PageSize));
        }

        // GET: Artists/Details/5
        [AllowAnonymous]
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

            // "Lazy Loading"
            // If we declare Artist.Albums as virtual ICollection<Album> we don't need to load albums like below
            // or with .Include() on db.Artists collection above
            //OFF: artist.Albums = db.Albums.Where(album => album.ArtistId == id);

            return View(artist);
        }


        // GET: Artists/Add
        public ActionResult Add()
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

            var artistFormVm = Mapper.Map<Artist, ArtistFormViewModel>(artist);

            return View("ArtistForm", artistFormVm);
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
                var artistInDb = db.Artists.Find(artistVm.Id);
                Mapper.Map<ArtistFormViewModel, Artist>
                    (
                    source: artistVm, 
                    destination: artistInDb
                    );
                db.SaveChanges();
            }
                
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? page, string currentFilter)
        {
            var artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
            db.SaveChanges();

            object routeValues;
            if(page.HasValue)
                routeValues = new { page = page, currentFilter = currentFilter };
            else
                routeValues = new { currentFilter = currentFilter };

            return RedirectToAction("Index", routeValues);
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
