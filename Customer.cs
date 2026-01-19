public class Customer
{
	public int CustomerId { get; set; }
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public bool IsActive { get; set; }

	public override string ToString()
	{
		return $"[{(IsActive ? "Active" : "Inactive")}]: {FirstName}, {LastName} - {Email}";
	}
}