using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}