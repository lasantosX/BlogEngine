using System.ComponentModel.DataAnnotations;

namespace AppBlogEngine.Models
{
    public class Comments
    {
        [Key]
        public long idComments { get; set; }
        [Required]
        public long idPublished { get; set; }
        [Required]
        public string textComment { get; set; }
        
    }
}
