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
using System.Threading.Tasks;
using System.Diagnostics;
using ImageGalery.Services.Persisters;

namespace ImageGalery.Services.Controllers
{
    public class ImagesController : ApiController
    {
        private ImageGalleryContext db = new ImageGalleryContext();

        public ImagesController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET api/Images
        public IEnumerable<Image> GetImages()
        {
            return db.Images.AsEnumerable();
        }

        public HttpResponseMessage GetImage(int id)
        {
            var comments = (from comment in db.Comments
                            where (comment.Image.ImageId == id)
                            select new
                            {
                                Content = comment.Content,
                                Username = comment.User.Username
                            }).ToList();
            var image = db.Images.Find(id);

            if (image != null)
            {
                var data = new
                {
                    ImageId = image.ImageId,
                    Title = image.Title,
                    Comments = comments,
                    Url = image.Url
                };

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // PUT api/Images/5
        public HttpResponseMessage PutImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != image.ImageId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Images
        // POST api/Images
        
        public async Task<HttpResponseMessage> PostImage(string title, int userId, int albumId)
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
         
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            
            try
            {
                // Read the form data.
                 await Request.Content.ReadAsMultipartAsync(provider);



                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    string fileName = file.LocalFileName;
                 

                    var url = DropBoxUploader.UploadProfilePicToDropBox(file.LocalFileName, file.Headers.ContentDisposition.FileName);
                    
                    db.Images.Add(new Image
                    {
                        Title = title,
                        Url = url,
                        Album = db.Albums.FirstOrDefault(x => x.AlbumId == albumId)
                    });
                    db.SaveChanges();
                    System.IO.File.Delete(file.LocalFileName);
                    break;
                }
             
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE api/Images/5
        public HttpResponseMessage DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Images.Remove(image);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, image);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}