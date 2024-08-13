using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos;

public class DepartmentDto
{
	public int Id { get; set; }

	[Required]
	[StringLength(100)]
	public string Name { get; set; } = null!;
}
