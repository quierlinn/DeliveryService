using DeliveryService.Core;
using DeliveryService.DataAccess;

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
        try
        {
            using (var writer = new StreamWriter(_logFilePath, append: true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error writing to log file: " + ex.Message);
        }
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

        try
        {
            var order = new Order { Weight = weight, District = district, DeliveryTime = deliveryTime };
            _context.Orders.Add(order);
            _context.SaveChanges();
            Log($"Order {order.OrderNumber} added.");
            return true;
        }
        catch (Exception ex)
        {
            Log("Error adding order: " + ex.Message);
            Console.WriteLine("An error occurred while adding the order.");
            return false;
        }
    }

    public List<Order> GetOrders(string district, DateTime from, DateTime to)
    {
        try
        {
            Log($"Filtering orders for district: {district} between {from} and {to}");
            return _context.Orders
                .Where(o => o.District == district && o.DeliveryTime >= from && o.DeliveryTime <= to)
                .ToList();
        }
        catch (Exception ex)
        {
            Log("Error retrieving orders: " + ex.Message);
            Console.WriteLine("An error occurred while retrieving orders.");
            return new List<Order>();
        }
    }

    public void SaveFilteredOrders(List<Order> orders, string resultFilePath)
    {
        try
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
        catch (IOException ex)
        {
            Log("Error saving filtered orders: " + ex.Message);
            Console.WriteLine("Error saving filtered orders to file.");
        }
    }
}