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
	public class AlbumsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Albums
		public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
		{
            int PageSize = 5;
            int PageNumber = (page ?? 1);


			ViewBag.Title = "My Albums";

            ViewBag.CurrentSort = sortOrder;
            
			// Assign sort string for sorting links
			ViewBag.ArtistSort = String.IsNullOrEmpty(sortOrder) ? "Artist_desc" : "";
			ViewBag.AlbumSort = sortOrder == "Album" ? "Album_desc" : "Album";
			ViewBag.YearSort = sortOrder == "Year" ? "Year_desc" : "Year";
			ViewBag.LabelSort = sortOrder == "Label" ? "Label_desc" : "Label";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

			var albums = db.Albums.Include(a => a.Artist);


			if (!String.IsNullOrEmpty(searchString))
			{
				albums = albums.Where(a => 
					a.Artist.Name.Contains(searchString) || 
					a.Title.Contains(searchString) ||
					a.Label.Contains(searchString));
			}

			switch (sortOrder)
			{
				case "Artist_desc":
					albums = albums.OrderByDescending(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
				case "Album":
					albums = albums.OrderBy(a => a.Title).ThenBy(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
				case "Album_desc":
					albums = albums.OrderByDescending(a => a.Title).ThenBy(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
				case "Year":
					albums = albums.OrderBy(a => a.Year).ThenBy(a => a.Artist.Name);
					break;
				case "Year_desc":
					albums = albums.OrderByDescending(a => a.Year).ThenBy(a => a.Artist.Name);
					break;
				case "Label":
					albums = albums.OrderBy(a => a.Label).ThenBy(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
				case "Label_desc":
					albums = albums.OrderByDescending(a => a.Label).ThenBy(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
				default:
					albums = albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Year);
					break;
			}

            return View(albums.ToPagedList(PageNumber, PageSize));
		}

		// GET: Albums/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Album album = db.Albums.Include(a => a.Artist).SingleOrDefault(a => a.Id == id);
			if (album == null)
			{
				return HttpNotFound();
			}
			return View(album);
		}

		// GET: Albums/Add
		public ActionResult Add()
		{
			var vm = new AlbumFormViewModel
			{
				ArtistsList = GetArtistsSelectListItem()
			};
			
			return View("AlbumForm", vm);
		}

		private List<SelectListItem> GetArtistsSelectListItem()
		{
			var artists = db.Artists.ToList();
			var selList = new List<SelectListItem>();
			foreach (Artist a in artists)
			{
				selList.Add(
					new SelectListItem()
					{
						Value = a.Id.ToString(),
						Text = a.Name
					});
			}			
			
			return selList;
		}

		// GET: Albums/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Album album = db.Albums.Find(id);
			if (album == null)
			{
				return HttpNotFound();
			}

			var vm = Mapper.Map<AlbumFormViewModel>(album);
			vm.ArtistsList = GetArtistsSelectListItem();

			return View("AlbumForm",vm);
		}

		// POST: Albums/Save/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Save(AlbumFormViewModel albumVm)
		{
			if (!ModelState.IsValid)
			{
				return View("AlbumForm", albumVm);
			}

			if (albumVm.Id == 0)
			{
				var album = Mapper.Map<Album>(albumVm);                
				db.Albums.Add(album);
			}
			else
			{
				//db.Entry(albumVm).State = EntityState.Modified;
				var albumInDb = db.Albums.Find(albumVm.Id);
				Mapper.Map<AlbumFormViewModel, Album>
					(
					source: albumVm,
					destination: albumInDb
					);

			}

			
			db.SaveChanges();
			return RedirectToAction("Index");
			
			//ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", albumVm.ArtistId);
			//return View(albumVm);
		}

		////////// GET: Albums/Delete/5
		////////public ActionResult Delete(int? id)
		////////{
		////////    if (id == null)
		////////    {
		////////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		////////    }
		////////    Album album = db.Albums.Find(id);
		////////    if (album == null)
		////////    {
		////////        return HttpNotFound();
		////////    }
		////////    return View(album);
		////////}

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? page, string sortOrder, string currentFilter)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();

            object routeValues = null;
            if (page.HasValue)
                routeValues = new { page = page, sortOrder = sortOrder, currentFilter = currentFilter };
            else
                routeValues = new { sortOrder = sortOrder, currentFilter = currentFilter };

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
