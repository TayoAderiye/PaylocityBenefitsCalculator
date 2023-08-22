using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.Data;
using Api.Models.DTOs;
using Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Implementations
{
    public class DependentService: IDependentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IEmployeeService _employeeService;

        public DependentService(IMapper mapper, DataContext context, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _context = context;
            _employeeService = employeeService;
        }
        public async Task<List<GetDependentDto>> GetAllDependents()
        {
            return _mapper.Map<List<GetDependentDto>>(await _context.Dependents.ToListAsync());
        }

        public async Task<GetDependentDto> GetDependentById(int id)
        {
            return _mapper.Map<GetDependentDto>(await _context.Dependents.Where(x => x.Id == id).FirstOrDefaultAsync());
        }

        public async Task<ApiResponse<GetDependentDto>> AddDependent(CreateDependentRequest request)
        {
            var result = new ApiResponse<GetDependentDto>();
            //check if employeeId exist
            var employee = await _employeeService.GetEmployeeById(request.EmployeeId);
            if (employee is null)
            {
                result.Success = false;
                result.Error = $"No Employee of {request.EmployeeId} found";
                return result;
            }
            //check if the dependent to be created is spouse and employee already has a spouse dependent
            foreach (var item in employee.Dependents)
            {
                if ((request.Relationship == Relationship.Spouse || request.Relationship == Relationship.DomesticPartner)
                    && (item.Relationship == Relationship.Spouse || item.Relationship == Relationship.DomesticPartner)
                    )
                {
                    result.Success = false;
                    result.Error = $"Employee already has a Spouse or Partner";
                    return result;
                }
            }
            var model = _mapper.Map<Dependent>(request);
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            result.Data = _mapper.Map<GetDependentDto>(model);
            result.Success = true;
            result.Message = $"Dependent successfully added to {request.EmployeeId}";
            return result;

        }
    }
}
