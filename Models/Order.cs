using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered
    }

    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}