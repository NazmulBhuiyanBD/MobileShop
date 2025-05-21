using MobileShop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileShop.Controllers
{
    public class CheckoutViewModel
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
