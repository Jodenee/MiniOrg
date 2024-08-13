using TestApi.Data;
using TestApi.Models;

namespace TestApi;

public class Seed
{
	private readonly DataContext dataContext;

	public Seed(DataContext context)
	{
		dataContext = context;
	}

	public async Task SeedDatabase()
	{
		List<Customer> customers = new List<Customer>
		{
			new Customer
			{
				FirstName = "Tessa",
				LastName = "Elliott"
			},
			new Customer
			{
				FirstName = "Silas",
				LastName = "Watson"
			},
			new Customer
			{
				FirstName = "Elijah",
				LastName = "Clark"
			},
			new Customer
			{
				FirstName = "Delilah",
				LastName = "Nelson"
			},
			new Customer
			{
				FirstName = "Ivy",
				LastName = "Parker"
			},
			new Customer
			{
				FirstName = "Lydia",
				LastName = "Rodriguez"
			},
			new Customer
			{
				FirstName = "Claire",
				LastName = "Morales"
			},
			new Customer
			{
				FirstName = "Luna",
				LastName = "Garcia"
			},
			new Customer
			{
				FirstName = "Ethan",
				LastName = "Mitchell"
			},
			new Customer
			{
				FirstName = "Alexander",
				LastName = "Harris"
			}
		};

		List<Department> departments = new List<Department>
		{
			new Department
			{
				Name = "IT"
			},
			new Department
			{
				Name = "Sales"
			},
			new Department
			{
				Name = "Legal"
			},
			new Department
			{
				Name = "PR"
			},
			new Department
			{
				Name = "HR"
			}
		};

		List<Manager> managers = new List<Manager>
		{
			new Manager
			{
				FirstName = "Mike",
				LastName = "Schmidt"
			},
			new Manager
			{
				FirstName = "Thomas",
				LastName = "Rodriguez"
			},
			new Manager
			{
				FirstName = "Emilia",
				LastName = "Brown"
			},
			new Manager
			{
				FirstName = "Mia",
				LastName = "Stewart"
			},
			new Manager
			{
				FirstName = "Ezekiel",
				LastName = "Adams"
			}
		};

		List<DepartmentManager> departmentManagers = new List<DepartmentManager>
		{
			new DepartmentManager
			{
				Department = departments[0],
				Manager = managers[0]
			},
			new DepartmentManager
			{
				Department = departments[1],
				Manager = managers[1]
			},
			new DepartmentManager
			{
				Department = departments[2],
				Manager = managers[2]
			},
			new DepartmentManager
			{
				Department = departments[3],
				Manager = managers[3]
			},
			new DepartmentManager
			{
				Department = departments[4],
				Manager = managers[4]
			},
			new DepartmentManager
			{
				Department = departments[4],
				Manager = managers[0]
			}
		};

		List<Employee> employees = new List<Employee>
		{
			new Employee
			{
				FirstName = "Robert",
				LastName = "Jackson",
				HireDate = DateTime.Now.AddYears(-1),
				JobTitle = "Front End Developer",
				SalaryPerMonth = 3000,
				Department = departments[0]
			},
			new Employee
			{
				FirstName = "Beau",
				LastName = "Wood",
				HireDate = DateTime.Now.AddYears(-3),
				JobTitle = "Back End Developer",
				SalaryPerMonth = 5000,
				Department = departments[0]
			},
			new Employee
			{
				FirstName = "Harper",
				LastName = "Long",
				HireDate = DateTime.Now.AddYears(-2),
				JobTitle = "Fullstack Developer",
				SalaryPerMonth = 8000,
				Department = departments[0]
			},
			new Employee
			{
				FirstName = "Dan",
				LastName = "Cooper",
				HireDate = DateTime.Now.AddMonths(-5),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Jade",
				LastName = "Phillips",
				HireDate = DateTime.Now.AddYears(-3),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Christian",
				LastName = "Price",
				HireDate = DateTime.Now.AddYears(-2),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Aiden",
				LastName = "White",
				HireDate = DateTime.Now.AddYears(-1),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Asher",
				LastName = "Cook",
				HireDate = DateTime.Now.AddYears(-2),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Addison",
				LastName = "Baker",
				HireDate = DateTime.Now.AddMonths(-8),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Brandon",
				LastName = "Doorman",
				HireDate = DateTime.Now.AddMonths(-4),
				JobTitle = "Sales Person",
				SalaryPerMonth = 3000,
				Department = departments[1]
			},
			new Employee
			{
				FirstName = "Savannah",
				LastName = "Rogers",
				HireDate = DateTime.Now.AddYears(-2),
				JobTitle = "Coordinator",
				SalaryPerMonth = 2000,
				Department = departments[4]
			},
			new Employee
			{
				FirstName = "Christopher",
				LastName = "Alvarez",
				HireDate = DateTime.Now.AddYears(-6),
				JobTitle = "Assistant",
				SalaryPerMonth = 2000,
				Department = departments[4]
			},
			new Employee
			{
				FirstName = "Gianna",
				LastName = "Moore",
				HireDate = DateTime.Now.AddYears(-3),
				JobTitle = "Assistant",
				SalaryPerMonth = 4000,
				Department = departments[2]
			},
			new Employee
			{
				FirstName = "Anna",
				LastName = "Rodriguez",
				HireDate = DateTime.Now.AddYears(-9),
				JobTitle = "Senior Legal Consultant",
				SalaryPerMonth = 8000,
				Department = departments[2]
			},
			new Employee
			{
				FirstName = "Colton",
				LastName = "Parker",
				HireDate = DateTime.Now.AddYears(-10),
				JobTitle = "Senior Public Relations Specialist",
				SalaryPerMonth = 7000,
				Department = departments[3]
			}
		};

		List<EmployeeReview> reviews = new List<EmployeeReview>
		{
			new EmployeeReview
			{
				Rating = 5,
				Title = "Fantastic recommendation!",
				Content = "If you wish to find something to suit your needs ask this person, they're the best! The product this employee recommended fulfilled all my requirements!",
				Employee = employees[3],
				Customer = customers[0]
			},
			new EmployeeReview
			{
				Rating = 4,
				Title = "Great recommendation :)",
				Content = "Great recommendation! The product this employee recommended fulfilled almost all my requirements!",
				Employee = employees[4],
				Customer = customers[2]
			},
			new EmployeeReview
			{
				Rating = 4,
				Title = "Great person, great recommendations!",
				Content = "Great recommendation! The product this employee recommended pretty much fulfilled all my requirements.",
				Employee = employees[5],
				Customer = customers[8]
			},
			new EmployeeReview
			{
				Rating = 3,
				Title = "Could have been better... :/",
				Content = "Could've recommended a better product for my requirements. The product this employee recommended only fulfilled half my requirements...",
				Employee = employees[6],
				Customer = customers[4]
			},
			new EmployeeReview
			{
				Rating = 2,
				Title = "Not satisified!",
				Content = "Ask for a second opinion. The product this employee recommended only fulfilled one of my requirements",
				Employee = employees[7],
				Customer = customers[6]
			},
			new EmployeeReview
			{
				Rating = 1,
				Title = "Bad recommendations...",
				Content = "Don't ask this employee for recommendations. The product this employee recommended didn't fulfill all my requirements...",
				Employee = employees[8],
				Customer = customers[8]
			},
			new EmployeeReview
			{
				Rating = 0,
				Title = "This person ruined my life :(",
				Content = "This employee manipulated me into making horrible financial decisions.",
				Employee = employees[9],
				Customer = customers[8]
			}
		};

		await dataContext.Customers.AddRangeAsync(customers);
		await dataContext.DepartmentManagers.AddRangeAsync(departmentManagers);
		await dataContext.Employees.AddRangeAsync(employees);
		await dataContext.EmployeeReviews.AddRangeAsync(reviews);

		await dataContext.SaveChangesAsync();
	}
}
