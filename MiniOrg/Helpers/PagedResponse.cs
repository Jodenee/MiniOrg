namespace TestApi.Helpers;

public class PagedResponse<T>
{
	public int PageNumber { get; set; }
	public bool HasPreviousPage { get; set; }
	public bool HasNextPage { get; set; }
	public ICollection<T> Data { get; set; } = null!;

	public PagedResponse(
		int pageNumber,
		bool hasPreviousPage,
		bool hasNextPage,
		ICollection<T> data)
	{
		PageNumber = pageNumber;
		HasPreviousPage = hasPreviousPage;
		HasNextPage = hasNextPage;
		Data = data;
	}
}
