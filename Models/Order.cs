using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileShop.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }

        public string ProductName { get; set; } 
        public decimal Price { get; set; }     

        public string Status { get; set; }
    }

}