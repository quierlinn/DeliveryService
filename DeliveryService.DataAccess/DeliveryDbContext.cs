using DeliveryService.Core;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.DataAccess;

public class DeliveryDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<FilteredOrder> FilteredOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=delivery.db");
}