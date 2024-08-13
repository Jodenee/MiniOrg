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
public class ManagerController : Controller
{
	private readonly IManagerRepository _managerRepository;
	private readonly IDepartmentRepository _departmentRepository;
	private readonly IMapper _mapper;

	public ManagerController(
		IManagerRepository managerRepository,
		IDepartmentRepository departmentRepository,
		IMapper mapper)
	{
		_managerRepository = managerRepository;
		_departmentRepository = departmentRepository;
		_mapper = mapper;
	}

	[HttpGet("GetAll")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<ManagerDto>))]
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

		ICollection<ManagerDto> mappedManagers = _mapper.Map<ICollection<ManagerDto>>(
			await _managerRepository.GetAll(orderBy, decsending, token)
		);

		IPagedList<ManagerDto> managers = mappedManagers.ToPagedList(pageNumber, pageSize);

		PagedResponse<ManagerDto> response = new PagedResponse<ManagerDto>(
			managers.PageNumber,
			managers.HasPreviousPage,
			managers.HasNextPage,
			managers.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("{id}")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(ManagerDto))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetById(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _managerRepository.Exists(id, token)) return NotFound();

		ManagerDto manager = _mapper.Map<ManagerDto>(
			await _managerRepository.GetById(id, token)
		);

		return Ok(manager);
	}

	[HttpGet("{id}/GetDepartments")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<DepartmentDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetDepartmentsManagedByManager(
		int id,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		ICollection<DepartmentDto> mappedDepartments = _mapper.Map<ICollection<DepartmentDto>>(
			await _managerRepository.GetDepartmentsOfManager(id, orderBy, decsending, token)
		);

		IPagedList<DepartmentDto> departments = mappedDepartments.ToPagedList(pageNumber, pageSize);

		PagedResponse<DepartmentDto> response = new PagedResponse<DepartmentDto>(
			departments.PageNumber,
			departments.HasPreviousPage,
			departments.HasNextPage,
			departments.ToImmutableList());

		return Ok(response);
	}

	[HttpPost("Create")]
	[ProducesResponseType(201, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Create(
		[Required][FromQuery] ICollection<int> departmentIds,
		[FromBody] ManagerDto manager,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (departmentIds.Count > 10) return BadRequest();
		if (!await _departmentRepository.Exists(departmentIds, token)) return NotFound();

		Manager mappedManager = _mapper.Map<Manager>(manager);

		if (!await _managerRepository.Create(departmentIds, mappedManager))
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
		[FromBody] ManagerDto manager,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (manager.Id != id) return BadRequest(ModelState);
		if (!await _managerRepository.Exists(id, token)) return NotFound();

		Manager mappedManager = _mapper.Map<Manager>(manager);

		if (!await _managerRepository.Update(mappedManager))
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
		if (!await _managerRepository.Exists(id, token)) return NotFound();

		ICollection<Department> departments = await _managerRepository.GetDepartmentsOfManager(id, cancellationToken: token);

		if (departments.Count > 0) return BadRequest("Cannot delete a manager who still manages departments.");

		Manager manager = await _managerRepository.GetById(id, token);

		if (!await _managerRepository.Delete(manager))
		{
			ModelState.AddModelError("", "Something went wrong while deleting.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}
}
