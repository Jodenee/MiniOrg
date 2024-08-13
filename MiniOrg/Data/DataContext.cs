using Microsoft.EntityFrameworkCore;
using TestApi.Models;

namespace TestApi.Data;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{

	}

	public DbSet<Customer> Customers { get; set; }
	public DbSet<Department> Departments { get; set; }
	public DbSet<DepartmentManager> DepartmentManagers { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<EmployeeReview> EmployeeReviews { get; set; }
	public DbSet<Manager> Managers { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DepartmentManager>()
			.HasKey(pc => new { pc.DepartmentId, pc.ManagerId });
		modelBuilder.Entity<DepartmentManager>()
			.HasOne(p => p.Department)
			.WithMany(d => d.Managers)
			.HasForeignKey(p => p.DepartmentId);
		modelBuilder.Entity<DepartmentManager>()
			.HasOne(p => p.Manager)
			.WithMany(d => d.Departments)
			.HasForeignKey(p => p.ManagerId);
	}
}
