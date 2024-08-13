using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class Customer
{
	public int Id { get; set; }

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string FirstName { get; set; } = null!;

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string LastName { get; set; } = null!;
}
