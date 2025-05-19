using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
        public class User
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();

            [Required]
            [MaxLength(20)]
            public string PhoneNumber { get; set; }

            [Required]
            public string Password { get; set; }

            public string Name { get; set; }
            public string Address { get; set; }
        }
}
