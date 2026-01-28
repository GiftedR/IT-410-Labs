public class Order
{
	public int OrderId { get; set; }
	public int CustomerId { get; set; }
	public DateTime OrderDate { get; set; }
	public string Status { get; set; } = default!; // Replace with enum later

	public override string ToString()
	{
		return $"[{Status}]: {OrderId}, {CustomerId} - {OrderDate}";
	}
}
