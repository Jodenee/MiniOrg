using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using TestApi.Dtos;
using TestApi.Helpers;
using TestApi.Interfaces;
using TestApi.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace TestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[OutputCache]
public class DepartmentController : Controller
{
	private readonly IDepartmentRepository _departmentRepository;
	private readonly IManagerRepository _managerRepository;
	private readonly IMapper _mapper;

	public DepartmentController(
		IDepartmentRepository departmentRepository,
		IManagerRepository managerRepository,
		IMapper mapper)
	{
		_departmentRepository = departmentRepository;
		_managerRepository = managerRepository;
		_mapper = mapper;
	}

	[HttpGet("GetAll")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<DepartmentDto>))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetAll(
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (pageNumber < 1) return BadRequest();
		if (pageSize < 1 || pageSize > 30) return BadRequest();

		ICollection<DepartmentDto> mappedDepartments = _mapper.Map<ICollection<DepartmentDto>>(
			await _departmentRepository.GetAll(orderBy, decsending, token)
		);

		IPagedList<DepartmentDto> departments = mappedDepartments.ToPagedList(pageNumber, pageSize);

		PagedResponse<DepartmentDto> response = new PagedResponse<DepartmentDto>(
			departments.PageNumber,
			departments.HasPreviousPage,
			departments.HasNextPage,
			departments.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("{id}")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(DepartmentDto))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetById(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _departmentRepository.Exists(id, token)) return NotFound();

		DepartmentDto department = _mapper.Map<DepartmentDto>(
			await _departmentRepository.GetById(id, token)
		);

		return Ok(department);
	}

	[HttpGet("{id}/Managers")]
	[ProducesResponseType(200, Type = typeof(ICollection<ManagerDto>))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> Managers(
		int id,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (pageNumber < 1) return BadRequest();
		if (pageSize < 1 || pageSize > 30) return BadRequest();
		if (!await _departmentRepository.Exists(id, token)) return NotFound();

		ICollection<ManagerDto> mappedManagers = _mapper.Map<ICollection<ManagerDto>>(
			await _departmentRepository.GetManagersOfDepartment(
				id,
				orderBy,
				decsending,
				token)
		);

		IPagedList<ManagerDto> managers = mappedManagers.ToPagedList(pageNumber, pageSize);

		PagedResponse<ManagerDto> response = new PagedResponse<ManagerDto>(
			managers.PageNumber,
			managers.HasPreviousPage,
			managers.HasNextPage,
			managers.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("{id}/Employees")]
	[ProducesResponseType(200, Type = typeof(ICollection<EmployeeDto>))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetEmployees(
		int id,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (pageNumber < 1) return BadRequest();
		if (pageSize < 1 || pageSize > 30) return BadRequest();
		if (!await _departmentRepository.Exists(id, token)) return NotFound();

		ICollection<EmployeeDto> mappedEmployees = _mapper.Map<ICollection<EmployeeDto>>(
			await _departmentRepository.GetEmployeesOfDepartment(
				id,
				orderBy,
				decsending,
				token)
		);

		IPagedList<EmployeeDto> employees = mappedEmployees.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeDto> response = new PagedResponse<EmployeeDto>(
			employees.PageNumber,
			employees.HasPreviousPage,
			employees.HasNextPage,
			employees.ToImmutableList());

		return Ok(response);
	}

	[HttpPost("Create")]
	[ProducesResponseType(201, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Crate(
		[Required][FromQuery] ICollection<int> managerIds,
		[FromBody] DepartmentDto department,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (managerIds.Count > 10) return BadRequest();
		if (!await _managerRepository.Exists(managerIds, token)) return NotFound();

		Department mappedDepartment = _mapper.Map<Department>(department);

		if (!await _departmentRepository.Create(managerIds, mappedDepartment))
		{
			ModelState.AddModelError("", "Something went wrong while saving.");
			return StatusCode(500, ModelState);
		}

		return StatusCode(201);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(204, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] DepartmentDto department,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (department.Id != id) return BadRequest(ModelState);
		if (!await _departmentRepository.Exists(id, token)) return NotFound();

		Department mappedDepartment = _mapper.Map<Department>(department);

		if (!await _departmentRepository.Update(mappedDepartment))
		{
			ModelState.AddModelError("", "Something went wrong while updating.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}

	[HttpPatch("{id}/RemoveManager/{managerId}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(304, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> RemoveManager(
		int id,
		int managerId,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _departmentRepository.Exists(id, token)) return NotFound();
		if (!await _managerRepository.Exists(managerId, token)) return NotFound();

		Department department = await _departmentRepository.GetById(id, token);
		ICollection<DepartmentManager> managers = await _departmentRepository.GetDepartmentManagers(id, token);

		if (!managers.Any(dm => dm.ManagerId == managerId)) return StatusCode(304);

		foreach (DepartmentManager manager in managers)
		{
			if (manager.ManagerId == managerId)
			{
				managers.Remove(manager);
				break;
			}
		}

		department.Managers = managers;

		if (!await _departmentRepository.Update(department))
		{
			ModelState.AddModelError("", "Something went wrong while updating.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Delete(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _departmentRepository.Exists(id, token)) return NotFound();

		ICollection<Manager> managers = await _departmentRepository.GetManagersOfDepartment(id, cancellationToken: token);

		if (managers.Count > 0) return BadRequest("Cannot delete a department that still has managers.");

		ICollection<Employee> employees = await _departmentRepository.GetEmployeesOfDepartment(id, cancellationToken: token);

		if (employees.Count > 0) return BadRequest("Cannot delete a department that still has employees.");

		Department department = await _departmentRepository.GetById(id, token);

		if (!await _departmentRepository.Delete(department))
		{
			ModelState.AddModelError("", "Something went wrong while deleting.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}
}
