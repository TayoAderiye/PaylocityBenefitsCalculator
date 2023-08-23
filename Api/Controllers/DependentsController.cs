using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.DTOs;
using Api.Services.Implementations;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var response = await _dependentService.GetDependentById(id);
        if (response.Success)
            return Ok(response);
        return NotFound(response);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var response = await _dependentService.GetAllDependents();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "Create a dependent")]
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Create(CreateDependentRequest request)
    {
        var response = await _dependentService.AddDependent(request);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}
