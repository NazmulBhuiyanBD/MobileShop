using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MobileShop.Models
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public double ProductPrice { get; set; }

        public string ProductDescription { get; set; }

        public string? ExistingImagePath { get; set; }

        public IFormFile? ProductImageFile { get; set; }
    }
}
