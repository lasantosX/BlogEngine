using System.ComponentModel.DataAnnotations;

namespace WebApiBlogEngine.Models
{
    public class RoleUser
    {
        [Key]
        public long idRole { get; set; }
        [Required]
        public long idUser { get; set; }
    }
}
