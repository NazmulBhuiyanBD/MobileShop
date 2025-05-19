using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string AdminUserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
