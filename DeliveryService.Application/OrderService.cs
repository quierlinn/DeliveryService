using DeliveryService.Core;
using DeliveryService.DataAccess;

namespace DeliveryService.Application;

public class OrderService
{
    private readonly DeliveryDbContext _context;

    public OrderService()
    {
        _context = new DeliveryDbContext();
    }

    public void AddOrder(double weight, string district, DateTime deliveryTime)
    {
        var order = new Order { Weight = weight, District = district, DeliveryTime = deliveryTime };
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public List<Order> GetOrders(string district, DateTime from, DateTime to)
    {
        return _context.Orders
            .Where(o => o.District == district && o.DeliveryTime >= from && o.DeliveryTime <= to)
            .ToList();
    }
}