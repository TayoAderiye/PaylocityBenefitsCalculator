using Api.Models;
using Api.Models.Data;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Services.Implementations
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _context;
        private readonly ICacheService _cacheService;

        public DbInitializer(DataContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public void Initialize()
        {
            try
            {
                Migrate();
                SeedData();
            }
            catch (Exception e)
            {

                throw;
            }
        }


        private void SeedData()
        {
            DependentData();
            SystemConfigData();
            EmployeeData();
        }

        private void Migrate()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }


        private void DependentData()
        {
            if (!_context.Dependents.Any())
            {
                _context.Dependents.AddRangeAsync(
                    new Dependent
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3),
                        EmployeeId = 2 // Associate with Employee ID 2 (Ja Morant)
                    },
                    new Dependent
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23),
                        EmployeeId = 2 // Associate with Employee ID 2 (Ja Morant)
                    },
                    new Dependent
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18),
                        EmployeeId = 2
                    },
                    new Dependent
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2),
                        EmployeeId = 3
                    }
                 ).GetAwaiter().GetResult();
                _context.SaveChangesAsync().GetAwaiter().GetResult();

            }

        }

        private void SystemConfigData()
        {
            if (!_context.SystemConfig.Any())
            {
                _context.SystemConfig.AddAsync(new SystemConfig
                {
                    BaseCost = 1000,
                    DependentCost = 600,
                    AdditionalSalaryThreshold = 80000,
                    SalaryDeductionRate = 0.02m,
                    AgeBasedDeduction = 200,
                    PaycheckPerYear = 26
                }).GetAwaiter().GetResult();
                _context.SaveChangesAsync().GetAwaiter().GetResult();
            }

            var sysConfig = _context.SystemConfig.FirstOrDefaultAsync().GetAwaiter().GetResult();
            if (sysConfig != null)
            {
                _cacheService.Set("SystemConfiguration", sysConfig);
            }
        }

        private void EmployeeData()
        {
            if (!_context.Employees.Any())
            {
                _context.Employees.AddRangeAsync(
                    new Employee
                    {
                        Id = 1,
                        FirstName = "LeBron",
                        LastName = "James",
                        Salary = 75420.99m,
                        DateOfBirth = new DateTime(1984, 12, 30)
                    },
                    new Employee
                    {
                        Id = 2,
                        FirstName = "Ja",
                        LastName = "Morant",
                        Salary = 92365.22m,
                        DateOfBirth = new DateTime(1999, 8, 10)
                    },
                    new Employee
                    {
                        Id = 3,
                        FirstName = "Michael",
                        LastName = "Jordan",
                        Salary = 143211.12m,
                        DateOfBirth = new DateTime(1963, 2, 17)
                    }
                 ).GetAwaiter().GetResult();
                _context.SaveChangesAsync().GetAwaiter().GetResult();
            }

        }
    }
}