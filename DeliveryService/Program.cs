using DeliveryService.Application;
public class Program
{
    static void Main()
    {
        var service = new OrderService();
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Delivery Service");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. Filter Orders");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Enter weight (kg): ");
                    double weight = double.Parse(Console.ReadLine());

                    Console.Write("Enter district: ");
                    string district = Console.ReadLine();

                    Console.Write("Enter delivery time (yyyy-MM-dd HH:mm:ss): ");
                    DateTime deliveryTime = DateTime.Parse(Console.ReadLine());

                    service.AddOrder(weight, district, deliveryTime);
                    Console.WriteLine("Order added successfully.");
                    break;

                case "2":
                    Console.Write("Enter district to filter: ");
                    district = Console.ReadLine();

                    Console.Write("Enter start time (yyyy-MM-dd HH:mm:ss): ");
                    DateTime from = DateTime.Parse(Console.ReadLine());

                    Console.Write("Enter end time (yyyy-MM-dd HH:mm:ss): ");
                    DateTime to = DateTime.Parse(Console.ReadLine());

                    var orders = service.GetOrders(district, from, to);
                    Console.WriteLine("Filtered Orders:");
                    foreach (var order in orders)
                    {
                        Console.WriteLine(
                            $"Order {order.OrderNumber} - Weight: {order.Weight} kg - Delivery Time: {order.DeliveryTime}");
                    }

                    break;

                case "3":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}