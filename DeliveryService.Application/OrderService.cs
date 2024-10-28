using DeliveryService.Core;
using DeliveryService.DataAccess;

namespace DeliveryService.Application;

public class OrderService
{
    private readonly DeliveryDbContext _context;
    private readonly string _logFilePath;

    public OrderService(string logFilePath)
    {
        _context = new DeliveryDbContext();
        _logFilePath = logFilePath;
    }

    public void Log(string message)
    {
        using var writer = new StreamWriter(_logFilePath, append: true);
        writer.WriteLine($"{DateTime.Now}: {message}");
    }

    public bool AddOrder(double weight, string district, DateTime deliveryTime)
    {
        if (weight <= 0)
        {
            Log("Invalid weight specified.");
            Console.WriteLine("Weight must be greater than zero.");
            return false;
        }
        
        if (string.IsNullOrEmpty(district))
        {
            Log("Invalid district specified.");
            Console.WriteLine("District cannot be empty.");
            return false;
        }

        var order = new Order { Weight = weight, District = district, DeliveryTime = deliveryTime };
        _context.Orders.Add(order);
        _context.SaveChanges();
        Log($"Order {order.OrderNumber} added.");
        return true;
    }

    public List<Order> GetOrders(string district, DateTime from, DateTime to)
    {
        Log($"Filtering orders for district: {district} between {from} and {to}");
        return _context.Orders
            .Where(o => o.District == district && o.DeliveryTime >= from && o.DeliveryTime <= to)
            .ToList();
    }
    public void SaveFilteredOrders(List<Order> orders, string resultFilePath)
    {
        using (var writer = new StreamWriter(resultFilePath))
        {
            foreach (var order in orders)
            {
                writer.WriteLine($"Order {order.OrderNumber} - Weight: {order.Weight} kg - Delivery Time: {order.DeliveryTime}");
            }
        }
        Log("Filtered orders saved to file.");
    }
}