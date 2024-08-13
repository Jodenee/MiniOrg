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
public class EmployeeReviewController : Controller
{
	private readonly IEmployeeReviewRepository _employeeReviewRepository;
	private readonly IEmployeeRepository _employeeRepository;
	private readonly ICustomerRepository _customerRepository;
	private readonly IMapper _mapper;

	public EmployeeReviewController(
		IEmployeeReviewRepository employeeReviewRepository,
		IEmployeeRepository employeeRepository,
		ICustomerRepository customerRepository,
		IMapper mapper)
	{
		_employeeReviewRepository = employeeReviewRepository;
		_employeeRepository = employeeRepository;
		_customerRepository = customerRepository;
		_mapper = mapper;
	}

	[HttpGet("GetAll")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<EmployeeReviewDto>))]
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

		ICollection<EmployeeReviewDto> mappedEmployeeReviews = _mapper.Map<ICollection<EmployeeReviewDto>>(
			await _employeeReviewRepository.GetAll(orderBy, decsending, token)
		);

		IPagedList<EmployeeReviewDto> employeeReviews = mappedEmployeeReviews.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeReviewDto> response = new PagedResponse<EmployeeReviewDto>(
			employeeReviews.PageNumber,
			employeeReviews.HasPreviousPage,
			employeeReviews.HasNextPage,
			employeeReviews.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("{id}")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(EmployeeReviewDto))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetById(int id, CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _employeeReviewRepository.Exists(id, token)) return NotFound();

		EmployeeReviewDto employeeReview = _mapper.Map<EmployeeReviewDto>(
			await _employeeReviewRepository.GetById(id, token)
		);

		return Ok(employeeReview);
	}

	[HttpGet("ByCustomer/{id}")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<EmployeeReviewDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetByCustomer(
		int id,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		ICollection<EmployeeReviewDto> mappedEmployeeReviews = _mapper.Map<ICollection<EmployeeReviewDto>>(
			await _employeeReviewRepository.GetByCustomer(
				id,
				orderBy,
				decsending,
				token)
		);

		IPagedList<EmployeeReviewDto> employeeReviews = mappedEmployeeReviews.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeReviewDto> response = new PagedResponse<EmployeeReviewDto>(
			employeeReviews.PageNumber,
			employeeReviews.HasPreviousPage,
			employeeReviews.HasNextPage,
			employeeReviews.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("ForEmployee/{id}")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<EmployeeReviewDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetForEmployee(
		int id,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		ICollection<EmployeeReviewDto> mappedEmployeeReviews = _mapper.Map<ICollection<EmployeeReviewDto>>(
			await _employeeReviewRepository.GetForEmployee(
				id,
				orderBy,
				decsending,
				token)
		);

		IPagedList<EmployeeReviewDto> employeeReviews = mappedEmployeeReviews.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeReviewDto> response = new PagedResponse<EmployeeReviewDto>(
			employeeReviews.PageNumber,
			employeeReviews.HasPreviousPage,
			employeeReviews.HasNextPage,
			employeeReviews.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("GetByRatingRange")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<EmployeeReviewDto>))]
	[ProducesResponseType(400, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetByRatingRange(
		[Required][FromQuery] byte min,
		[Required][FromQuery] byte max,
		[Required][FromQuery] string orderBy = "id",
		[Required][FromQuery] bool decsending = false,
		[Required][FromQuery] int pageNumber = 1,
		[Required][FromQuery] int pageSize = 10,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		ICollection<EmployeeReviewDto> mappedEmployeeReviews = _mapper.Map<ICollection<EmployeeReviewDto>>(
			await _employeeReviewRepository.GetByRatingRange(
				min,
				max,
				orderBy,
				decsending,
				token)
		);

		IPagedList<EmployeeReviewDto> employeeReviews = mappedEmployeeReviews.ToPagedList(pageNumber, pageSize);

		PagedResponse<EmployeeReviewDto> response = new PagedResponse<EmployeeReviewDto>(
			employeeReviews.PageNumber,
			employeeReviews.HasPreviousPage,
			employeeReviews.HasNextPage,
			employeeReviews.ToImmutableList());

		return Ok(response);
	}

	[HttpPost("Create")]
	[ProducesResponseType(201, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Create(
		[Required][FromQuery] int customerId,
		[Required][FromQuery] int employeeId,
		[FromBody] EmployeeReviewDto employeeReview,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _customerRepository.Exists(customerId, token)) return NotFound();
		if (!await _employeeRepository.Exists(employeeId, token)) return NotFound();

		EmployeeReview mappedEmployeeReview = _mapper.Map<EmployeeReview>(employeeReview);

		if (!await _employeeReviewRepository.Create(customerId, employeeId, mappedEmployeeReview))
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
		[FromBody] EmployeeReviewDto employeeReview,
		CancellationToken token = default)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (employeeReview.Id != id) return BadRequest(ModelState);
		if (!await _employeeReviewRepository.Exists(id, token)) return NotFound();

		EmployeeReview mappedEmployeeReview = _mapper.Map<EmployeeReview>(employeeReview);

		if (!await _employeeReviewRepository.Update(mappedEmployeeReview))
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
		if (!await _employeeReviewRepository.Exists(id, token)) return NotFound();

		EmployeeReview employeeReview = await _employeeReviewRepository.GetById(id, token);

		if (!await _employeeReviewRepository.Delete(employeeReview))
		{
			ModelState.AddModelError("", "Something went wrong while deleting.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}
}
