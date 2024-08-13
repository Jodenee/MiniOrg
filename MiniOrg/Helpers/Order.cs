using TestApi.Models;

namespace TestApi.Helpers;

public static class Order
{
	public static IQueryable<Customer> By(IQueryable<Customer> customers, string field, bool decsending)
	{
		if (field.Equals("firstName", StringComparison.OrdinalIgnoreCase))
		{
			customers = decsending
				? customers.OrderByDescending(c => c.FirstName)
				: customers.OrderBy(c => c.FirstName);
		}
		else if (field.Equals("lastName", StringComparison.OrdinalIgnoreCase))
		{
			customers = decsending
				? customers.OrderByDescending(c => c.LastName)
				: customers.OrderBy(c => c.LastName);
		}
		else
		{
			customers = decsending
				? customers.OrderByDescending(c => c.Id)
				: customers.OrderBy(c => c.Id);
		}

		return customers;
	}

	public static IQueryable<Department> By(IQueryable<Department> departments, string field, bool decsending)
	{
		if (field.Equals("name", StringComparison.OrdinalIgnoreCase))
		{
			departments = decsending
				? departments.OrderByDescending(d => d.Name)
				: departments.OrderBy(d => d.Name);
		}
		else
		{
			departments = decsending
				? departments.OrderByDescending(d => d.Id)
				: departments.OrderBy(d => d.Id);
		}

		return departments;
	}

	public static IQueryable<Employee> By(IQueryable<Employee> employees, string field, bool decsending)
	{
		if (field.Equals("firstName", StringComparison.OrdinalIgnoreCase))
		{
			employees = decsending
				? employees.OrderByDescending(e => e.FirstName)
				: employees.OrderBy(e => e.FirstName);
		}
		else if (field.Equals("lastName", StringComparison.OrdinalIgnoreCase))
		{
			employees = decsending
				? employees.OrderByDescending(e => e.LastName)
				: employees.OrderBy(e => e.LastName);
		}
		else if (field.Equals("hireDate", StringComparison.OrdinalIgnoreCase))
		{
			employees = decsending
				? employees.OrderByDescending(e => e.HireDate)
				: employees.OrderBy(e => e.HireDate);
		}
		else if (field.Equals("jobTitle", StringComparison.OrdinalIgnoreCase))
		{
			employees = decsending
				? employees.OrderByDescending(e => e.JobTitle)
				: employees.OrderBy(e => e.JobTitle);
		}
		else if (field.Equals("salaryPerMonth", StringComparison.OrdinalIgnoreCase))
		{
			employees = decsending
				? employees.OrderByDescending(e => e.SalaryPerMonth)
				: employees.OrderBy(e => e.SalaryPerMonth);
		}
		else
		{
			employees = decsending
				? employees.OrderByDescending(e => e.Id)
				: employees.OrderBy(e => e.Id);
		}

		return employees;
	}

	public static IQueryable<EmployeeReview> By(IQueryable<EmployeeReview> employeeReviews, string field, bool decsending)
	{
		if (field.Equals("rating", StringComparison.OrdinalIgnoreCase))
		{
			employeeReviews = decsending
				? employeeReviews.OrderByDescending(er => er.Rating)
				: employeeReviews.OrderBy(er => er.Rating);
		}
		else if (field.Equals("title", StringComparison.OrdinalIgnoreCase))
		{
			employeeReviews = decsending
				? employeeReviews.OrderByDescending(er => er.Title)
				: employeeReviews.OrderBy(er => er.Title);
		}
		else if (field.Equals("content", StringComparison.OrdinalIgnoreCase))
		{
			employeeReviews = decsending
				? employeeReviews.OrderByDescending(er => er.Content)
				: employeeReviews.OrderBy(er => er.Content);
		}
		else
		{
			employeeReviews = decsending
				? employeeReviews.OrderByDescending(er => er.Id)
				: employeeReviews.OrderBy(er => er.Id);
		}

		return employeeReviews;
	}

	public static IQueryable<Manager> By(IQueryable<Manager> managers, string field, bool decsending)
	{
		if (field.Equals("firstName", StringComparison.OrdinalIgnoreCase))
		{
			managers = decsending
				? managers.OrderByDescending(m => m.FirstName)
				: managers.OrderBy(m => m.FirstName);
		}
		if (field.Equals("lastName", StringComparison.OrdinalIgnoreCase))
		{
			managers = decsending
				? managers.OrderByDescending(m => m.LastName)
				: managers.OrderBy(m => m.LastName);
		}
		else
		{
			managers = decsending
				? managers.OrderByDescending(m => m.Id)
				: managers.OrderBy(m => m.Id);
		}

		return managers;
	}
}
