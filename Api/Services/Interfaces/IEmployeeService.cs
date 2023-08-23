using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<List<GetEmployeeDto>>> GetAllEmployees();
        Task<ApiResponse<GetEmployeeDto>> GetEmployeeById(int id);
        Task<ApiResponse<List<string>>> CalculateEmployeePayCheck(int employeeId);
    }
}
