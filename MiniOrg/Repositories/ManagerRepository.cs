using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Repositories;

public class ManagerRepository : IManagerRepository
{
	private readonly DataContext _dataContext;

	public ManagerRepository(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<bool> Save()
	{
		return await _dataContext.SaveChangesAsync() > 0;
	}

	public async Task<bool> Exists(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Managers.AnyAsync(m => m.Id == id, cancellationToken);
	}

	public async Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Managers
			.Where(m => ids.Contains(m.Id))
			.AnyAsync(cancellationToken);
	}

	public async Task<ICollection<Manager>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Manager> managers = _dataContext.Managers.AsQueryable();

		managers = Order.By(managers, orderBy, decsending);

		return await managers.ToListAsync(cancellationToken);
	}

	public async Task<Manager> GetById(int id, CancellationToken cancellationToken = default)
	{
		return await _dataContext.Managers
			.SingleAsync(m => m.Id == id, cancellationToken);
	}

	public async Task<ICollection<Department>> GetDepartmentsOfManager(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Department> departments = _dataContext.DepartmentManagers
			.Where(dm => dm.ManagerId == id)
			.Select(dm => dm.Department);

		departments = Order.By(departments, orderBy, decsending);

		return await departments.ToListAsync(cancellationToken);
	}

	public async Task<bool> Create(ICollection<int> departmentIds, Manager manager)
	{
		List<DepartmentManager> departmentsManagers = new List<DepartmentManager>();

		foreach (int departmentId in departmentIds)
		{
			Department department = await _dataContext.Departments
				.SingleAsync(d => d.Id == departmentId);
			DepartmentManager departmentManager = new DepartmentManager
			{
				Department = department,
				Manager = manager
			};

			await _dataContext.DepartmentManagers.AddAsync(departmentManager);
			departmentsManagers.Add(departmentManager);
		}

		manager.Departments = departmentsManagers;

		await _dataContext.Managers.AddAsync(manager);
		return await Save();
	}

	public async Task<bool> Update(Manager manager)
	{
		_dataContext.Managers.Update(manager);
		return await Save();
	}

	public async Task<bool> Delete(Manager manager)
	{
		_dataContext.Managers.Remove(manager);
		return await Save();
	}
}
