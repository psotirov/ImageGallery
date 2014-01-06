using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ImageGallery.Models;
using ImageGallery.DataLayer;

namespace ImageGalery.Services.Controllers
{
    public class AlbumsController : ApiController
    {
        private ImageGalleryContext db = new ImageGalleryContext();

        public AlbumsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }      

        // GET api/Albums/5
        [HttpGet]
        public HttpResponseMessage GetAlbums(int id)
        {
            // id = albumId
            var albumsAndImagesInAlbum = db.Albums.Where(x => x.AlbumId == id).Select(y => new
            {
                ParentAlbumId = y.ParentId,
                Images = y.Images.Select(x => new 
                {
                   
                    AlbumId = y.AlbumId,
                    ImageId = x.ImageId,
                    Title = x.Title,
                    Url=x.Url               
                }),
               Albums=y.Albums

            }).ToList();

            if (albumsAndImagesInAlbum.Count == 0)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Request.CreateResponse(HttpStatusCode.OK, albumsAndImagesInAlbum);
            
        }

        // POST api/Albums
        [ActionName("create")]
        public HttpResponseMessage PostAlbum(Album album)
        {
            
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, album);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = album.AlbumId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}