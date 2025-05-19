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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                return RedirectToAction("Cart","Products");
            }

            var model = new CheckoutViewModel
            {
                CartItems = cart,
                TotalAmount = (decimal)cart.Sum(item => item.ProductPrice * item.Quantity)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetString("PhoneNumber");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                ModelState.AddModelError("", "Your cart is empty");
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cart;
                model.TotalAmount = (decimal)cart.Sum(item => item.ProductPrice * item.Quantity);
                return View(model);
            }

            try
            {
                var order = new Order
                {
                    CustomerName = model.FullName,
                    CustomerPhone = model.PhoneNumber,
                    ShippingAddress = model.Address,
                    TotalAmount = (decimal)cart.Sum(item => item.ProductPrice * item.Quantity),
                    OrderItems = cart.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = (decimal)item.ProductPrice,
                        Quantity = item.Quantity
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Clear cart
                HttpContext.Session.Remove("Cart");

                return RedirectToAction("OrderConfirmation", new { id = order.OrderId });
            }
            catch (Exception ex)
            {
                // Log the error (you should implement proper logging)
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");

                // Repopulate model data
                model.CartItems = cart;
                model.TotalAmount = (decimal)cart.Sum(item => item.ProductPrice * item.Quantity);
                return View(model);
            }
        }

        public IActionResult OrderConfirmation(Guid id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login","Admin");
            }
            return View(await _context.Orders.ToListAsync());
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrderId,ProductName,ProductPrice,Address,UserPhone")] Order order)
        {
            string adminId = HttpContext.Session.GetString("AdminUserName");

            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Admin");
            }

            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5

        // GET: Orders/Delete/5 (No longer needed if you're not using a confirmation page)
        // You can remove this action if you don't need it anymore

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error (you should implement proper logging)
                TempData["ErrorMessage"] = "Error deleting order. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
