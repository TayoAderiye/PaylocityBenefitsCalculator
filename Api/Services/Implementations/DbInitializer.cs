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

        public DbInitializer(DataContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            try
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
            catch (Exception e)
            {
                throw;
            }



        }
    }
}
