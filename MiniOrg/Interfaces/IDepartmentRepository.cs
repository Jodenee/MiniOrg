using TestApi.Models;

namespace TestApi.Interfaces;

public interface IDepartmentRepository
{
	Task<bool> Save();
	Task<bool> Exists(int id, CancellationToken cancellationToken = default);
	Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default);
	Task<bool> IsManagedBy(int id, int managerId, CancellationToken cancellationToken = default);
	Task<ICollection<Department>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<Department> GetById(int id, CancellationToken cancellationToken = default);
	Task<ICollection<Manager>> GetManagersOfDepartment(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<ICollection<Employee>> GetEmployeesOfDepartment(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<ICollection<DepartmentManager>> GetDepartmentManagers(int id, CancellationToken cancellationToken = default);
	Task<bool> Create(ICollection<int> managerIds, Department department);
	Task<bool> Update(Department department);
	Task<bool> Delete(Department department);
}
