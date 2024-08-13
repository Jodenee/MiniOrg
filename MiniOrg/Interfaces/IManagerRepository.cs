using TestApi.Models;

namespace TestApi.Interfaces;

public interface IManagerRepository
{
	Task<bool> Save();
	Task<bool> Exists(int id, CancellationToken cancellationToken = default);
	Task<bool> Exists(ICollection<int> ids, CancellationToken cancellationToken = default);
	Task<ICollection<Manager>> GetAll(
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<Manager> GetById(int id, CancellationToken cancellationToken = default);
	Task<ICollection<Department>> GetDepartmentsOfManager(
		int id,
		string orderBy = "id",
		bool decsending = false,
		CancellationToken cancellationToken = default);
	Task<bool> Create(ICollection<int> departmentIds, Manager manager);
	Task<bool> Update(Manager manager);
	Task<bool> Delete(Manager manager);
}
