using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository.Interfaces;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
    private decimal BASE_COST = 1000; // Base cost per month
    private decimal DEPENDENT_COST = 600; // Cost per dependent per month
    private decimal ADDITIONAL_SALARY_THRESHOLD = 80000; // Salary threshold for additional deduction
    private decimal SALARY_DEDUCTION_RATE = 0.02m; // 2% of salary
    private decimal AGE_BASED_DEDUCTION = 200; // Additional cost for dependents over 50
    private decimal PAYCHECKS_PER_YEAR = 26; // Number of paychecks per year

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    //new test case 
    [Fact]
    public async Task CalculateEmployeePayCheck_EmployeeNotFound_ReturnsError()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/paychecks/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);

    }

    //new test case 
    [Fact]
    public async Task CalculateEmployeePayCheck_ReturnsCorrectPaychecks()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/paychecks/2");
        var employee = new GetEmployeeDto
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
            }

        };

        var totalCost = BASE_COST;
        DateTime today = DateTime.Today;
        decimal dependentAllowance = 0.0m;
        decimal additionalDeduction = 0.0m;

        // Add dependent costs
        foreach (var dependent in employee.Dependents)
        {
            dependentAllowance += DEPENDENT_COST;
            int age = today.Year - dependent.DateOfBirth.Year;
            // Add age-based deduction for dependents over 50
            if (age > 50)
            {
                dependentAllowance += AGE_BASED_DEDUCTION;
            }
        }

        // Check if salary exceeds the threshold for additional deduction
        if (employee.Salary > ADDITIONAL_SALARY_THRESHOLD)
        {
            additionalDeduction = (employee.Salary * SALARY_DEDUCTION_RATE) / 12; // Monthly deduction
        }

        totalCost = totalCost + dependentAllowance + additionalDeduction;
        // Calculate benefits cost per paycheck
        // var benefitsCostPerPaycheck = totalCost / PAYCHECKS_PER_YEAR;
        //assuming the cost is deducted from employee salary
        var totalAnnaulCost = totalCost * 12;

        // Calculate net salary for each paycheck
        var netSalary = employee.Salary - totalAnnaulCost;

        var paychecks = new List<string>();


        for (int i = 0; i < PAYCHECKS_PER_YEAR; i++)
        {
            //var paycheck = Math.Round(netSalary - benefitsCostPerPaycheck, 2);
            var paycheck = Math.Round(netSalary / PAYCHECKS_PER_YEAR, 2);
            paychecks.Add($"PayCheck {i + 1}: {paycheck}");
        }

        //await response.ShouldReturn(HttpStatusCode.NotFound);
        await response.ShouldReturn(HttpStatusCode.OK, paychecks);
    }
}

