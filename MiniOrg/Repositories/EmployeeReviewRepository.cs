using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Repositories;

public class EmployeeReviewRepository : IEmployeeReviewRepository
{
	private readonly DataContext _dataContext;

	public EmployeeReviewRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<bool> Save()
	{
		return await _dataContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.EmployeeReviews.AnyAsync(er => er.Id == id, cancellationToken);
	}

	public async Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default)
	{
		return await _dataContext.EmployeeReviews
			.Where(er => ids.Contains(er.Id))
			.AnyAsync(cancellationToken);
	}

	public async Task<ICollection<EmployeeReview>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<EmployeeReview> employeeReviews = _dataContext.EmployeeReviews.AsQueryable();

		employeeReviews = Order.By(employeeReviews, orderBy, decsending);

		return await employeeReviews.ToListAsync(cancellationToken);
	}

	public async Task<EmployeeReview> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.EmployeeReviews
			.SingleAsync(er => er.Id == id, cancellationToken);
	}

	public async Task<ICollection<EmployeeReview>> GetForEmployee(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<EmployeeReview> employeeReviews = _dataContext.EmployeeReviews.
			Where(er => er.Employee.Id == id);

		employeeReviews = Order.By(employeeReviews, orderBy, decsending);

		return await employeeReviews.ToListAsync(cancellationToken);
	}

	public async Task<ICollection<EmployeeReview>> GetByCustomer(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<EmployeeReview> employeeReviews = _dataContext.EmployeeReviews
			.Where(er => er.Customer.Id == id);

		employeeReviews = Order.By(employeeReviews, orderBy, decsending);

		return await employeeReviews.ToListAsync(cancellationToken);
	}

	public async Task<ICollection<EmployeeReview>> GetByRatingRange(
		byte min,
		byte max,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<EmployeeReview> employeeReviews = _dataContext.EmployeeReviews
			.Where(er => er.Rating >= min && er.Rating <= max);

		employeeReviews = Order.By(employeeReviews, orderBy, decsending);

		return await employeeReviews.ToListAsync(cancellationToken);
	}

	public async Task<bool> Create(int customerId, int employeeId, EmployeeReview employeeReview)
	{
		Employee employee = await _dataContext.Employees.SingleAsync(e => e.Id == employeeId);
		Customer customer = await _dataContext.Customers.SingleAsync(er => er.Id == employeeId);

		employeeReview.Employee = employee;
		employeeReview.Customer = customer;

		await _dataContext.EmployeeReviews.AddAsync(employeeReview);
		return await Save();
	}

	public async Task<bool> Update(EmployeeReview employeeReview)
	{
		_dataContext.EmployeeReviews.Update(employeeReview);
		return await Save();
	}

	public async Task<bool> Delete(EmployeeReview employeeReview)
	{
		_dataContext.EmployeeReviews.Remove(employeeReview);
		return await Save();
	}
}
