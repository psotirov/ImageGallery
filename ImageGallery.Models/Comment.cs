using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int ImageId { get; set; }
        public int UserId { get; set; }

        public virtual Image Image { get; set; }
        public virtual User User { get; set; }
    }
}
