using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileShop.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string Address { get; set; }
        public string UserPhone { get; set; }
    }
}
