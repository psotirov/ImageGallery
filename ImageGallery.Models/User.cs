using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string SessionKey { get; set; }

        public virtual ICollection<Album> Galleries { get; set; }

        public User()
        {
            this.Galleries = new HashSet<Album>();
        }
    }
}
