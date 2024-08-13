using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
	private readonly DataContext _dataContext;

	public DepartmentRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<bool> Save()
	{
		return await _dataContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Departments.AnyAsync(d => d.Id == id, cancellationToken);
	}

	public async Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Departments
			.Where(d => ids.Contains(d.Id))
			.AnyAsync(cancellationToken);
	}

	public async Task<bool> IsManagedBy(int id, int managerId, CancellationToken cancellationToken = default)
	{
		return await _dataContext.DepartmentManagers
			.AnyAsync(dm => dm.DepartmentId == id && dm.ManagerId == managerId, cancellationToken);
	}

	public async Task<ICollection<Department>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Department> departments = _dataContext.Departments.AsQueryable();

		departments = Order.By(departments, orderBy, decsending);

		return await departments.ToListAsync(cancellationToken);
	}

	public async Task<Department> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Departments
			.SingleAsync(d => d.Id == id, cancellationToken);
	}

	public async Task<ICollection<Manager>> GetManagersOfDepartment(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Manager> managers = _dataContext.DepartmentManagers
			.Where(dm => dm.DepartmentId == id)
			.Select(dm => dm.Manager);

		managers = Order.By(managers, orderBy, decsending);

		return await managers.ToListAsync(cancellationToken);
	}

	public async Task<ICollection<Employee>> GetEmployeesOfDepartment(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Employee> employees = _dataContext.Employees
			.Where(e => e.Department.Id == id);

		employees = Order.By(employees, orderBy, decsending);

		return await employees.ToListAsync(cancellationToken);
	}

	public async Task<ICollection<DepartmentManager>> GetDepartmentManagers(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.DepartmentManagers
			.Where(dm => dm.DepartmentId == id)
			.ToListAsync(cancellationToken);
	}

	public async Task<bool> Create(ICollection<int> managerIds, Department department)
	{
		department.Managers = new List<DepartmentManager>();

		foreach (int managerId in managerIds)
		{
			Manager manager = await _dataContext.Managers
				.SingleAsync(m => m.Id == managerId);
			DepartmentManager departmentManager = new DepartmentManager
			{
				Department = department,
				Manager = manager
			};

			department.Managers.Add(departmentManager);
			await _dataContext.AddAsync(departmentManager);
		}

		await _dataContext.AddAsync(department);
		return await Save();
	}

	public async Task<bool> Update(Department department)
	{
		_dataContext.Departments.Update(department);
		return await Save();
	}

	public async Task<bool> Delete(Department department)
	{
		_dataContext.Departments.Remove(department);
		return await Save();
	}
}
