using System.ComponentModel.DataAnnotations;

namespace TestApi.Models;

public class DepartmentManager
{
	public int DepartmentId { get; set; }
	public int ManagerId { get; set; }

	[Required]
	public Department Department { get; set; } = null!;

	[Required]
	public Manager Manager { get; set; } = null!;
}
