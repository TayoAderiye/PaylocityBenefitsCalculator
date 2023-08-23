using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var response = await _employeeService.GetEmployeeById(id);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
       
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var response = await _employeeService.GetAllEmployees();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get employee paychecks")]
    [HttpGet("paychecks/{employeeId}")]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetEmployeePayChecks([FromRoute] int employeeId)
    {
        //task: use a more realistic production approach
        var response = await _employeeService.CalculateEmployeePayCheck(employeeId);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}
