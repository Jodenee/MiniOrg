using TestApi.Models;

namespace TestApi.Interfaces;

public interface ICustomerRepository
{
	Task<bool> Save();
	Task<bool> Exists(int id, CancellationToken cancellationToken = default);
	Task<bool> Exists(ICollection<int> id, CancellationToken cancellationToken = default);
	Task<ICollection<Customer>> GetAll(string orderBy, bool decsending, CancellationToken cancellationToken = default);
	Task<Customer> GetById(int id, CancellationToken cancellationToken = default);
	Task<Customer?> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken = default);
	Task<bool> Create(Customer customer);
	Task<bool> Update(Customer customer);
	Task<bool> Delete(Customer customer);
}
