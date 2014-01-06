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
using System.Text;

namespace ImageGalery.Services.Controllers
{
    public class UsersController : ApiController
    {
        public UsersController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        } 
        private ImageGalleryContext db = new ImageGalleryContext();

        // GET api/Users
        public HttpResponseMessage GetUsers()
        {
            var data = (from users in db.Users
                        select new
                        {
                            UserId = users.UserId,
                            Username = users.Username,
                            SessionKey = users.SessionKey
                        }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        // GET api/Users/5
        public HttpResponseMessage GetUser(int id)
        {
            
            var galleries = (from albums in db.Albums
                             where (albums.User.UserId == id /*&& albums.Albums == null*/)
                             select new
                             {
                                 AlbumId = albums.AlbumId,
                                 Title = albums.Title
                             }).ToList();
            var user = db.Users.Find(id);

            if (user != null)
            {
                var data = new
                            {
                                UserId = user.UserId,
                                Username = user.Username,
                                Galleries = galleries,
                                SessionKey = user.SessionKey
                            };

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/Users
        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.SessionKey = GenerateSessionKey(13);
                db.Users.Add(user);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LogUser(User user)
        {
            if (ModelState.IsValid)
            {
                var userChecked = db.Users.Select(u => u).Where(u => u.AuthCode == user.AuthCode &&
                    u.Username == user.Username).ToList();

                if (userChecked.Count != 0)
                {
                    var newSessionKey = GenerateSessionKey(userChecked[0].UserId);

                    var data = new
                    {
                        UserId = userChecked[0].UserId,
                        Username = user.Username,
                        SessionKey = newSessionKey
                    };

                    userChecked[0].SessionKey = newSessionKey;
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
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

        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;
        private static string GenerateSessionKey(int userId)
        {
            StringBuilder keyChars = new StringBuilder(50);
            var rand = new Random();
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }
    }
}