using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos;

public class EmployeeReviewDto
{
	public int Id { get; set; }

	[Required]
	[Range(0, 5)]
	public byte Rating { get; set; }

	[Required]
	[StringLength(100, MinimumLength = 10)]
	public string Title { get; set; } = null!;

	[Required]
	[StringLength(500, MinimumLength = 30)]
	public string Content { get; set; } = null!;
}
