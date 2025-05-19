using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Support
    {
        [Key]
        public Guid SupportId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
