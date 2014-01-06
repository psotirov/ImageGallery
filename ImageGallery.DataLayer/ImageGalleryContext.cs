using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageGallery.Models;
using System.Data.Entity;

namespace ImageGallery.DataLayer
{   
    public class ImageGalleryContext : DbContext
    {
        public ImageGalleryContext()
            : base("AppHarborGaleryEntities")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>();

        //    modelBuilder.Entity<Album>()
        //        .HasRequired(u => u.User)
        //        .WithOptional(a => a.Gallery)
        //        .Map(x => x.MapKey("UserId"));
                      

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
