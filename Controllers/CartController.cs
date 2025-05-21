using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private const string CartSessionKey = "Cart";

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.Get<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(Guid productId)
    {
        var product = _context.Products.Find(productId);
        if (product == null) return NotFound();

        var cart = HttpContext.Session.Get<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        var existing = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existing != null)
            existing.Quantity++;
        else
            cart.Add(new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductImage = product.ProductImage,
                Quantity = 1
            });

        HttpContext.Session.Set(CartSessionKey, cart);
        return RedirectToAction("Index");
    }

    public IActionResult ContinueShopping()
    {
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ProceedOrder()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var phone = HttpContext.Session.GetString("PhoneNumber"); // 🔄 fixed key name
        var address = HttpContext.Session.GetString("Address");

        // Validate session data
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
        {
            TempData["Error"] = "Customer info missing. Please login again or complete checkout.";
            return RedirectToAction("Login", "Account");
        }

        var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
        if (cart == null || !cart.Any())
            return RedirectToAction("Index");

        foreach (var item in cart)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                CustomerName = userName,
                CustomerPhone = phone, // ✅ no longer null
                ShippingAddress = address,
                ProductName = item.ProductName,
                Price = (decimal)(item.ProductPrice * item.Quantity),
                Status = "Pending"
            };
            _context.Orders.Add(order);
        }

        _context.SaveChanges();
        HttpContext.Session.Remove("Cart");

        TempData["OrderSuccess"] = "Your order has been placed successfully!";
        return RedirectToAction("Index", "Home");
    }

}
