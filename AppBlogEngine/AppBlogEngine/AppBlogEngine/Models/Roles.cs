using System.ComponentModel.DataAnnotations;

namespace AppBlogEngine.Models
{
    public class Roles
    {
        [Key]
        public long idRole { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}
