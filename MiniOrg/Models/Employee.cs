using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class Employee
{
	public int Id { get; set; }

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string FirstName { get; set; } = null!;

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string LastName { get; set; } = null!;

	[Required]
	public DateTime HireDate { get; set; }

	[Required]
	[StringLength(100)]
	public string JobTitle { get; set; } = null!;

	[Required]
	public int SalaryPerMonth { get; set; }

	[Required]
	public Department Department { get; set; } = null!;
}
