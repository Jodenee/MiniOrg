using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class Department
{
	public int Id { get; set; }

	[Required]
	[StringLength(100)]
	public string Name { get; set; } = null!;

	[Required]
	public ICollection<Employee> Employees { get; set; } = null!;

	[Required]
	public ICollection<DepartmentManager> Managers { get; set; } = null!;
}
