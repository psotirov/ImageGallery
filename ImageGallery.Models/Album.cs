using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public string Title { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public int? ParentId { get; set; }
        public virtual Album Parent { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        public Album()
        {
            this.Albums = new HashSet<Album>();
            this.Images = new HashSet<Image>();
        }
    }
}
