using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyRecordCollection.Models;
using MyRecordCollection.Dtos;
using AutoMapper;

namespace MyRecordCollection.Controllers.Api
{
    public class ArtistsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Artists
        public IHttpActionResult GetArtists()
        {
            var artists = db.Artists.ToList();

            var artistsDto = artists.Select(AutoMapper.Mapper.Map<Artist, ArtistDto>);

            return Ok(artistsDto);
        }

        // GET: api/Artists/5
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        

        // POST: api/Artists
        [ResponseType(typeof(Artist))]
        public IHttpActionResult PostArtist(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Artists.Add(artist);//(Mapper.Map<Artist>(artist));
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = artist.Id }, artist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(int id)
        {
            return db.Artists.Count(e => e.Id == id) > 0;
        }
    }
}