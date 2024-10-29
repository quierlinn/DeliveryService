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
        if (weight <= 0 || string.IsNullOrEmpty(district))
        {
            Log("Invalid order data.");
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
            return false;
        }
    }

    public List<Order> GetFilteredOrders(string district, DateTime from, DateTime to)
    {
        try
        {
            Log($"Retrieving orders for district {district} between {from} and {to}");
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

    public void FilterAndSaveOrders(string district, DateTime firstDeliveryTime)
    {
        DateTime thirtyMinutesLater = firstDeliveryTime.AddMinutes(30);
        
        try
        {
            var filteredOrders = GetFilteredOrders(district, firstDeliveryTime, thirtyMinutesLater);

            foreach (var order in filteredOrders)
            {
                var filteredOrder = new FilteredOrder
                {
                    OrderNumber = order.OrderNumber,
                    Weight = order.Weight,
                    District = order.District,
                    DeliveryTime = order.DeliveryTime
                };
                
                _context.FilteredOrders.Add(filteredOrder);
            }
            _context.SaveChanges();
            Log($"Filtered {filteredOrders.Count} orders for district {district} saved to FilteredOrders table.");
        }
        catch (Exception ex)
        {
            Log("Error saving filtered orders: " + ex.Message);
            Console.WriteLine("An error occurred while saving filtered orders.");
        }
    }
}