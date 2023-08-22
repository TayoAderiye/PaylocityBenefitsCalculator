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
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee is null)
        {
            var result = new ApiResponse<GetEmployeeDto>
            {
                Data = null,
                Success = false,
                Error = "Employee not Found"
            };
            return NotFound(result);
        }
        return Ok(employee);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var employees = await _employeeService.GetAllEmployees();
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get employee paychecks")]
    [HttpGet("paychecks/{employeeId}")]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetEmployeePayChecks([FromRoute] int employeeId)
    {
        //task: use a more realistic production approach
        var employeePayChecks = await _employeeService.CalculateEmployeePayCheck(employeeId);
        if (employeePayChecks.Success)
            return Ok(employeePayChecks);
        return BadRequest(employeePayChecks);
    }
}
