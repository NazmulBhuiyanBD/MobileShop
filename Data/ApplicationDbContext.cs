using Microsoft.EntityFrameworkCore;
using MobileShop.Models;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MobileShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options)
        {

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Support>Supports { get; set; }
    }
}
