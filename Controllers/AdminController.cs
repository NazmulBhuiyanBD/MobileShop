using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using MobileShop.Data;

namespace MobileShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin Login
        [HttpPost]
        public IActionResult Login(string AdminUserName, string Password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.AdminUserName == AdminUserName && a.Password == Password);
            if (admin != null)
            {
                // Store Admin ID in session
                HttpContext.Session.SetString("AdminUserName", admin.AdminUserName.ToString());
                return RedirectToAction("AdminDashBoard");
            }

            ViewBag.Error = "Invalid admin credentials!";
            return View();
        }

        // Admin Dashboard
        public async Task<IActionResult> AdminDashBoard()
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login");
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminUserName.ToString() == adminId);
            if (admin == null)
            {
                return RedirectToAction("Login");
            }

            return View(admin);
        }


        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        //public IActionResult BusList()
        //{
        //    var buses = _context.Busses.ToList();
        //    return View(buses);
        //}
    }
}