﻿using DeliveryService.Application;

public class Program
{
    static void Main(string[] args)
    {
        var arguments = ParseArguments(args);
        string logFilePath = arguments.ContainsKey("_deliveryLog") ? arguments["_deliveryLog"] : "delivery.log";
        string resultFilePath = arguments.ContainsKey("_deliveryOrder") ? arguments["_deliveryOrder"] : "deliveryOrders.txt";


        var service = new OrderService(logFilePath);
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Delivery Service");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. Filter Orders");
            Console.WriteLine("3. Save Filtered Orders (next 30 minutes)");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            
            var option = Console.ReadLine();
            
            switch (option)
            {
                case "1":
                    Console.Write("Enter weight (kg): ");
                    if (!double.TryParse(Console.ReadLine(), out double weight))
                    {
                        Console.WriteLine("Invalid weight format.");
                        continue;
                    }

                    Console.Write("Enter district: ");
                    string district = Console.ReadLine();

                    Console.Write("Enter delivery time (yyyy-MM-dd HH:mm:ss): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime deliveryTime))
                    {
                        Console.WriteLine("Invalid date format.");
                        continue;
                    }

                    if (service.AddOrder(weight, district, deliveryTime))
                    {
                        Console.WriteLine("Order added successfully.");
                    }
                    break;

                case "2":
                    Console.Write("Enter district to filter: ");
                    district = Console.ReadLine();

                    Console.Write("Enter start time (yyyy-MM-dd HH:mm:ss): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime from))
                    {
                        Console.WriteLine("Invalid date format.");
                        continue;
                    }

                    Console.Write("Enter end time (yyyy-MM-dd HH:mm:ss): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime to))
                    {
                        Console.WriteLine("Invalid date format.");
                        continue;
                    }

                    var orders = service.GetOrders(district, from, to);
                    Console.WriteLine("Filtered Orders:");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"Order {order.OrderNumber} - Weight: {order.Weight} kg - Delivery Time: {order.DeliveryTime}");
                    }
                    break;

                case "3":
                    Console.Write("Enter district to filter: ");
                    district = Console.ReadLine();

                    Console.Write("Enter start time for first delivery (yyyy-MM-dd HH:mm:ss): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime firstDelivery))
                    {
                        Console.WriteLine("Invalid date format.");
                        continue;
                    }

                    DateTime thirtyMinutesLater = firstDelivery.AddMinutes(30);
                    var filteredOrders = service.GetOrders(district, firstDelivery, thirtyMinutesLater);
                    service.SaveFilteredOrders(filteredOrders, resultFilePath);
                    Console.WriteLine($"Filtered orders saved to {resultFilePath}");
                    break;

                case "4":
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
    static Dictionary<string, string> ParseArguments(string[] args)
    {
        var arguments = new Dictionary<string, string>();
        
        for (int i = 0; i < args.Length; i += 2)
        {
            if (i + 1 < args.Length)
            {
                arguments[args[i]] = args[i + 1];
            }
        }
        
        return arguments;
    }
}