using System.ComponentModel.DataAnnotations;

namespace WebApiBlogEngine.Models
{
    public class Users
    {
        [Key]
        public long idUser { get; set; }
        [Required]
        public long idRole { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string firstLastName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
