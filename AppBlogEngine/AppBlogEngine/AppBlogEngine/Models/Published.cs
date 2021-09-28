using System;
using System.ComponentModel.DataAnnotations;

namespace AppBlogEngine.Models
{
    public class Published
    {
        [Key]
        public long idPublished { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string textContent { get; set; }
        [Required]
        public DateTime datePublishing { get; set; }
        
        [Required]
        public long idUser { get; set; }
        [Required]
        public int status { get; set; }

        public string Comment { get; set; }
    }
}
