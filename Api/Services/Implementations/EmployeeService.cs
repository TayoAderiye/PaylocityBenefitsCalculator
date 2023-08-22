using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.Data;
using Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public EmployeeService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<GetEmployeeDto>> GetAllEmployees()
        {
             return _mapper.Map<List<GetEmployeeDto>>(await _context.Employees.Include(x => x.Dependents).ToListAsync());
        }

        public async Task<GetEmployeeDto> GetEmployeeById(int id)
        {
            return _mapper.Map<GetEmployeeDto>(await _context.Employees.Include(x => x.Dependents).Where(x => x.Id == id).FirstOrDefaultAsync());
        }
    }
}
