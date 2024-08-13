using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
	private readonly DataContext _dataContext;

	public EmployeeRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<bool> Save()
	{
		return await _dataContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Employees.AnyAsync(e => e.Id == id, cancellationToken);
	}

	public async Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Employees
			.Where(e => ids.Contains(e.Id))
			.AnyAsync(cancellationToken);
	}

	public async Task<ICollection<Employee>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Employee> employees = _dataContext.Employees.AsQueryable();

		employees = Order.By(employees, orderBy, decsending);

		return await employees.ToListAsync(cancellationToken);
	}

	public async Task<Employee> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Employees
			.SingleAsync(e => e.Id == id, cancellationToken);
	}

	public async Task<ICollection<Employee>> GetByJobTitle(
		string jobTitle,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Employee> employees = _dataContext.Employees
			.Where(e => e.JobTitle == jobTitle);

		employees = Order.By(employees, orderBy, decsending);

		return await employees.ToListAsync(cancellationToken);
	}

	public async Task<Department> GetEmployeeDepartment(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Employees
			.Where(e => e.Id == id)
			.Select(e => e.Department)
			.SingleAsync(cancellationToken);

	}

	public async Task<bool> Create(int departmentId, Employee employee)
	{
		Department department = await _dataContext.Departments.SingleAsync(d => d.Id == departmentId);

		employee.Department = department;

		await _dataContext.Employees.AddAsync(employee);
		return await Save();
	}

	public async Task<bool> Update(Employee employee)
	{
		_dataContext.Employees.Update(employee);
		return await Save();
	}

	public async Task<bool> Delete(Employee employee)
	{
		_dataContext.Employees.Remove(employee);
		return await Save();
	}
}
