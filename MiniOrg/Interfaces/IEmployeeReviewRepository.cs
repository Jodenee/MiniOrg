using TestApi.Models;

namespace TestApi.Interfaces;

public interface IEmployeeReviewRepository
{
	Task<bool> Save();
	Task<bool> Exists(int id, CancellationToken cancellationToken = default);
	Task<ICollection<EmployeeReview>> GetAll(string orderBy = "id", bool decsending = false, CancellationToken cancellationToken = default);
	Task<EmployeeReview> GetById(int id, CancellationToken cancellationToken = default);
	Task<ICollection<EmployeeReview>> GetForEmployee(int id, string orderBy = "id", bool decsending = false, CancellationToken cancellationToken = default);
	Task<ICollection<EmployeeReview>> GetByCustomer(int id, string orderBy = "id", bool decsending = false, CancellationToken cancellationToken = default);
	Task<ICollection<EmployeeReview>> GetByRatingRange(
		byte min,
		byte max,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<bool> Create(int customerId, int employeeId, EmployeeReview employeeReview);
	Task<bool> Update(EmployeeReview employeeReview);
	Task<bool> Delete(EmployeeReview employeeReview);
}
