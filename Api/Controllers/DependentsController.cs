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
        var dependent = await _dependentService.GetDependentById(id);
        if (dependent is null)
        {
            var result = new ApiResponse<GetDependentDto>
            {
                Data = null,
                Success = false,
                Error = "Dependent not Found"
            };
            return NotFound(result);
        }
        else
        {
            return Ok(dependent);
        }
       
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllDependents();
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return Ok(result);
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
