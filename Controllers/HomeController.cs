using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;

namespace MobileShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    //public IActionResult Index()
    //{
    //    return View();
    //}
    public IActionResult Index(string searchString)
    {
        var products = from p in _context.Products
                       select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p =>
                p.ProductName.Contains(searchString) ||
                p.ProductDescription.Contains(searchString));
        }

        return View(products.ToList());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
