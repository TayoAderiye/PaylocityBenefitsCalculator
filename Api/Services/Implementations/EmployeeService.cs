using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.Data;
using Api.Repository.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;

namespace Api.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Employee> _employeeRepo;
        private const decimal BASE_COST = 1000; // Base cost per month
        private const decimal DEPENDENT_COST = 600; // Cost per dependent per month
        private const decimal ADDITIONAL_SALARY_THRESHOLD = 80000; // Salary threshold for additional deduction
        private const decimal SALARY_DEDUCTION_RATE = 0.02m; // 2% of salary
        private const decimal AGE_BASED_DEDUCTION = 200; // Additional cost for dependents over 50
        private const decimal PAYCHECKS_PER_YEAR = 26; // Number of paychecks per year

        public EmployeeService(IMapper mapper,IRepository<Employee> employeeRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
        }
        public async Task<List<GetEmployeeDto>> GetAllEmployees()
        {
            return _mapper.Map<List<GetEmployeeDto>>(await _employeeRepo.Query().Include(x => x.Dependents).ToListAsync());
        }

        public async Task<GetEmployeeDto> GetEmployeeById(int id)
        {
            return _mapper.Map<GetEmployeeDto>(await _employeeRepo.Query().Where(x => x.Id == id).Include(x => x.Dependents).FirstOrDefaultAsync());
        }

        public async Task<ApiResponse<List<string>>> CalculateEmployeePayCheck(int employeeId)
        {
            var result = new ApiResponse<List<string>>();
            var totalCost = BASE_COST;
            DateTime today = DateTime.Today;
            //get dependents of the employee
            var employee = await _employeeRepo.Query().Where(x => x.Id == employeeId).Include(x => x.Dependents).FirstOrDefaultAsync();
            if (employee == null)
            {
                result.Success = false;
                result.Error = "Employee not found";
                return result;
            }

            // Add dependent costs
            foreach (var dependent in employee.Dependents)
            {
                totalCost += DEPENDENT_COST;
                int age = today.Year - dependent.DateOfBirth.Year;
                // Add age-based deduction for dependents over 50
                if (age > 50)
                {
                    totalCost += AGE_BASED_DEDUCTION;
                }
            }

            // Check if salary exceeds the threshold for additional deduction
            if (employee.Salary > ADDITIONAL_SALARY_THRESHOLD)
            {
                var additionalDeduction = (employee.Salary * SALARY_DEDUCTION_RATE) / 12; // Monthly deduction
                totalCost += additionalDeduction;
            }

            // Calculate benefits cost per paycheck
            var benefitsCostPerPaycheck = totalCost / PAYCHECKS_PER_YEAR;

            // Calculate net salary for each paycheck
            var netSalary = employee.Salary / PAYCHECKS_PER_YEAR;

            var paychecks = new List<string>();


            for (int i = 0; i < PAYCHECKS_PER_YEAR; i++)
            {
                var paycheck = Math.Round(netSalary - benefitsCostPerPaycheck, 2);
                paychecks.Add($"PayCheck {i + 1}: {paycheck}");
            }
            result.Success = true;
            result.Message = "PayChecks Received";
            result.Data = paychecks;
            return result;

        }
    }
}

//console.log(`Paycheck ${ i + 1}: $${ paychecks[i].toFixed(2)}`);