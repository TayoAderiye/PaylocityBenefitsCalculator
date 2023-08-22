using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<GetEmployeeDto>> GetAllEmployees();
        Task<GetEmployeeDto> GetEmployeeById(int id);
        Task<ApiResponse<List<string>>> CalculateEmployeePayCheck(int employeeId);
    }
}
