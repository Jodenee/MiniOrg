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
public class CustomerController : Controller
{
	private readonly ICustomerRepository _customerRepository;
	private readonly IMapper _mapper;

	public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
	{
		_customerRepository = customerRepository;
		_mapper = mapper;
	}

	[HttpGet("GetAll")]
	[ProducesResponseType(200, Type = typeof(PagedResponse<CustomerDto>))]
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

		ICollection<CustomerDto> mappedCustomers = _mapper.Map<ICollection<CustomerDto>>(
			await _customerRepository.GetAll(orderBy, decsending, token)
		);

		IPagedList<CustomerDto> customers = mappedCustomers.ToPagedList(pageNumber, pageSize);

		PagedResponse<CustomerDto> response = new PagedResponse<CustomerDto>(
			customers.PageNumber,
			customers.HasPreviousPage,
			customers.HasNextPage,
			customers.ToImmutableList());

		return Ok(response);
	}

	[HttpGet("{id}")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(CustomerDto))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetById(int id, CancellationToken token = default)
	{
		if (!await _customerRepository.Exists(id, token)) return NotFound();

		CustomerDto customer = _mapper.Map<CustomerDto>(
			await _customerRepository.GetById(id, token)
		);

		return Ok(customer);
	}

	[HttpGet("GetByFullName")]
	[OutputCache(PolicyName = "Expire30s")]
	[ProducesResponseType(200, Type = typeof(CustomerDto))]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	public async Task<IActionResult> GetByFullName(
		[Required][FromQuery] string firstName,
		[Required][FromQuery] string lastName,
		CancellationToken token = default)
	{
		CustomerDto? customer = _mapper.Map<CustomerDto?>(
			await _customerRepository.GetByFullName(firstName, lastName, token)
		);

		if (customer == null) return NotFound();

		return Ok(customer);
	}

	[HttpPost("Create")]
	[ProducesResponseType(201, Type = typeof(Nullable))]
	[ProducesResponseType(400)]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Create([FromBody] CustomerDto customer)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		Customer mappedCustomer = _mapper.Map<Customer>(customer);

		if (!await _customerRepository.Create(mappedCustomer))
		{
			ModelState.AddModelError("", "Something went wrong while saving.");
			return StatusCode(500, ModelState);
		}

		return StatusCode(201);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Update(int id, [FromBody] CustomerDto customer, CancellationToken token)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (customer.Id != id) return BadRequest(ModelState);
		if (!await _customerRepository.Exists(customer.Id, token)) return NotFound();

		Customer mappedCustomer = _mapper.Map<Customer>(customer);

		if (!await _customerRepository.Update(mappedCustomer))
		{
			ModelState.AddModelError("", "Something went wrong while updating.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404, Type = typeof(Nullable))]
	[ProducesResponseType(429, Type = typeof(Nullable))]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Delete(int id, CancellationToken token)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);
		if (!await _customerRepository.Exists(id, token)) return NotFound();

		Customer customer = await _customerRepository.GetById(id, token);

		if (!await _customerRepository.Delete(customer))
		{
			ModelState.AddModelError("", "Something went wrong while deleting.");
			return StatusCode(500, ModelState);
		}

		return NoContent();
	}
}
