using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RetroFootballWeb.Repository
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<VoucherApplied> VoucherApplied { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; } 
        public DbSet<FavoriteTeam> FavoriteTeams { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(c => new { c.CustomerID, c.ProductID, c.Size });
            modelBuilder.Entity<WishList>().HasKey(w => new { w.CustomerID, w.ProductID });
            modelBuilder.Entity<OrderDetail>().HasKey(o => new { o.OrderID, o.ProductID, o.Size });
            modelBuilder.Entity<DeliveryInfo>().HasKey(d => new { d.CustomerID, d.Priority });
            modelBuilder.Entity<Feedback>().HasKey(f => new { f.CustomerID, f.ProductID });
            modelBuilder.Entity<VoucherApplied>().HasKey(v => new { v.VoucherID, v.CustomerID });
            modelBuilder.Entity<FavoriteTeam>().HasKey(v => new { v.CustomerID, v.TeamName });
            base.OnModelCreating(modelBuilder);
        }
    }
}
