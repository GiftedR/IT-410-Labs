public class Order
{
	public int OrderId { get; set; }
	public int CustomerId { get; set; }
	public DateTime OrderDate { get; set; }
	public string OrderStatus { get; set; } = default!; // Replace with enum later

	public override string ToString()
	{
		return $"[{OrderStatus}]: {OrderId}, {CustomerId} - {OrderDate}";
	}
}
