using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos;

public class ManagerDto
{
	public int Id { get; set; }

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string FirstName { get; set; } = null!;

	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string LastName { get; set; } = null!;
}
