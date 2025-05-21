using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; } = 1;
        public double Total => ProductPrice * Quantity;
    }
}
