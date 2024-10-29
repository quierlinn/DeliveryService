namespace DeliveryService.Core;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = Guid.NewGuid().ToString();
    public double Weight { get; set; }
    public string District { get; set; } = string.Empty;
    public DateTime DeliveryTime { get; set; }
}