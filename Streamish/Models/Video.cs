using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Streamish.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        //[StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        //[StringLength(255, MinimumLength = 3)]
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
