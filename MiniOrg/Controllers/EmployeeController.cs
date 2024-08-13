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
public class EmployeeController : Controller
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IDepartmentRepository _departmentRepository;
	private readonly IMapper _mapper;

	public EmployeeController(
		IEmployeeRepository employeeRepository,
		IDepartmentRepository departmentRepository,
		IMapper mapper
	)
	{
		_employeeRepository = employeeRepository;
		_departmentRepository = departmentRepository;
		_mapper = mapper;
	}

	[HttpGet("GetAll")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<EmployeeDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
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

		ICollection<EmployeeDto> mappedEmployees = _mapper.Map<ICollection<EmployeeDto>>(
			await _employeeRepository.GetAll(orderBy, decsending, token)
		);

		IPagedList<EmployeeDto> employees = mappedEmployees.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeDto> response = new PagedResponse<EmployeeDto>(
			employees.PageNumber,
			employees.HasPreviousPage,
			employees.HasNextPage,
			employees.ToImmutableArray());

		return Ok(response);
	}

	[HttpGet("{id}")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(EmployeeDto))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetById(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _employeeRepository.Exists(id, token)) return NotFound(ModelState);

		EmployeeDto employee = _mapper.Map<EmployeeDto>(
			await _employeeRepository.GetById(id, token)
		);

		return Ok(employee);
	}

	[HttpGet("GetByJobTitle")]
	[ProducesResponseType(200, Type = typeof(ICollection<EmployeeDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetByJobTitle(
		[FromQuery] string jobTitle,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (pageNumber < 1) return BadRequest();
		if (pageSize < 1 || pageSize > 30) return BadRequest();

		ICollection<EmployeeDto> mappedEmployees = _mapper.Map<ICollection<EmployeeDto>>(
			await _employeeRepository.GetByJobTitle(
				jobTitle, 
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

	[HttpGet("{id}/Department")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(DepartmentDto))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetEmployeeDepartment(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _employeeRepository.Exists(id, token)) return NotFound();

		DepartmentDto department = _mapper.Map<DepartmentDto>(
			await _employeeRepository.GetEmployeeDepartment(id, token)
		);

		return Ok(department);
	}

	[HttpPost("Create")]
	[ProducesResponseType(201, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Create(
		[Required][FromQuery] int departmentId,
		[FromBody] EmployeeDto employee,
		CancellationToken token = default
	)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _departmentRepository.Exists(departmentId, token)) return NotFound();

		Employee mappedEmployee = _mapper.Map<Employee>(employee);

		if (!await _employeeRepository.Create(departmentId, mappedEmployee))
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
		[FromBody] EmployeeDto employee, 
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (employee.Id != id) return BadRequest(ModelState);
		if (!await _employeeRepository.Exists(id, token)) return NotFound();

		Employee mappedEmployee = _mapper.Map<Employee>(employee);

		if (!await _employeeRepository.Update(mappedEmployee))
		{
			ModelState.AddModelError("", "Something went wrong while updating.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}

	[HttpPatch("{id}/ChangeDepartment/{departmentId}")]
	[ProducesResponseType(204, Type = typeof(Nullable))]
	[ProducesResponseType(304, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> ChangeDepartment(
		int id, 
		int departmentId, 
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _employeeRepository.Exists(id, token)) return NotFound();
		if (!await _departmentRepository.Exists(departmentId, token)) return NotFound();
		if ((await _employeeRepository.GetEmployeeDepartment(id, token)).Id == departmentId) return StatusCode(304);

		Employee employee = await _employeeRepository.GetById(id, token);
		Department department = await _departmentRepository.GetById(departmentId, token);

		employee.Department = department;

		if (!await _employeeRepository.Update(employee))
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
		if (!await _employeeRepository.Exists(id, token)) return NotFound();

		Employee employee = await _employeeRepository.GetById(id, token);

		if (!await _employeeRepository.Delete(employee))
		{
			ModelState.AddModelError("", "Something went wrong while deleting.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}
}
