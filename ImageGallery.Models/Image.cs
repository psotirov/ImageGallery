using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public virtual Album Album { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Image()
        {
            this.Comments = new HashSet<Comment>();
        }
    }
}
