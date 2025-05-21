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

        [HttpPost]
        public IActionResult Login(string PhoneNumber, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber && u.Password == Password);

            if (user != null)
            {
                // Store all needed session data
                HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("Address", user.Address);

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
        public IActionResult Profile()
        {
            string phoneNumber = HttpContext.Session.GetString("PhoneNumber");

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login");
            }

            // Get user from database
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                // User not found in database but session exists - clear session
                HttpContext.Session.Remove("PhoneNumber");
                return RedirectToAction("Login");
            }

            return View(user); // Pass the user object to the view
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            string phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update fields
            existingUser.Name = model.Name;
            existingUser.Address = model.Address;

            _context.Update(existingUser);
            _context.SaveChanges();

            // Refresh session values
            HttpContext.Session.SetString("UserName", existingUser.Name);
            HttpContext.Session.SetString("Address", existingUser.Address);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
