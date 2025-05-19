using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileShop.Data;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    public class SupportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Supports
        public async Task<IActionResult> Index()
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(await _context.Supports.ToListAsync());
        }

        // GET: Supports/Create
        public IActionResult Create()
        {
            string userId = HttpContext.Session.GetString("PhoneNumber");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login","Account");
            }
            return View();
        }

        // POST: Supports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupportId,UserName,PhoneNumber,Message")] Support support)
        {
            string userId = HttpContext.Session.GetString("PhoneNumber");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login","Account");
            }
            if (ModelState.IsValid)
            {
                support.SupportId = Guid.NewGuid();
                _context.Add(support);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard","Account");
            }
            return View(support);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Admin");
            }

            var support = await _context.Supports.FindAsync(id);
            if (support == null)
            {
                TempData["ErrorMessage"] = "Support ticket not found!";
                return RedirectToAction(nameof(Index));
            }

            _context.Supports.Remove(support);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Support ticket deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool SupportExists(Guid id)
        {
            return _context.Supports.Any(e => e.SupportId == id);
        }
    }
}
