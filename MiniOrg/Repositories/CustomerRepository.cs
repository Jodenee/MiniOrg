using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
	private readonly DataContext _dataContext;

	public CustomerRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<bool> Save()
	{
		return await _dataContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Customers.AnyAsync(c => c.Id == id, cancellationToken);
	}

	public async Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Customers
			.Where(c => ids.Contains(c.Id))
			.AnyAsync(cancellationToken);
	}

	public async Task<ICollection<Customer>> GetAll(
		string orderBy,
		bool decsending,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Customer> customers = _dataContext.Customers.AsQueryable();

		customers = Order.By(customers, orderBy, decsending);

		return await customers.ToListAsync(cancellationToken);
	}

	public async Task<Customer> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Customers
			.SingleAsync(c => c.Id == id, cancellationToken);
	}

	public async Task<Customer?> GetByFullName(
		string firstName, 
		string lastName, 
		CancellationToken cancellationToken = default)
	{
		return await _dataContext.Customers
			.FirstOrDefaultAsync(
				c => c.FirstName == firstName && c.LastName == lastName,
				cancellationToken
			);
	}

	public async Task<bool> Create(Customer customer)
	{
		await _dataContext.Customers.AddAsync(customer);
		return await Save();
	}

	public async Task<bool> Update(Customer customer)
	{
		_dataContext.Customers.Update(customer);
		return await Save();
	}

	public async Task<bool> Delete(Customer customer)
	{
		_dataContext.Customers.Remove(customer);
		return await Save();
	}
}
