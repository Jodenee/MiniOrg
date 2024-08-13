using TestApi.Models;

namespace TestApi.Interfaces;

public interface IEmployeeRepository
{
	Task<bool> Save();
	Task<bool> Exists(int id, CancellationToken cancellationToken = default);
	Task<ICollection<Employee>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<Employee> GetById(int id, CancellationToken cancellationToken = default);
	Task<ICollection<Employee>> GetByJobTitle(
		string jobTitle,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<Department> GetEmployeeDepartment(int id, CancellationToken cancellationToken = default);
	Task<bool> Create(int departmentId, Employee employee);
	Task<bool> Update(Employee employee);
	Task<bool> Delete(Employee employee);
}
