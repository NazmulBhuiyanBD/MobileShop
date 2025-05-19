using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Signup
        [HttpPost]
        public async Task<IActionResult> Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (_context.Users.Any(u => u.PhoneNumber == user.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber already taken.");
                return View(user);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }


        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string PhoneNumber, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber && u.Password == Password);

            if (user != null)
            {
                HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber.ToString());
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials!";
            return View();
        }

        public IActionResult Dashboard()
        {
            // Check if user is logged in
            string userId = HttpContext.Session.GetString("PhoneNumber");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            // Get user from the database
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }


            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
