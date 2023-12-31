﻿using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Models.Data;
using Api.Models.DTOs;
using Api.Repository.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Api.Services.Implementations
{
    public class DependentService : IDependentService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IRepository<Dependent> _dependentRepo;

        public DependentService(IMapper mapper, IEmployeeService employeeService, IRepository<Dependent> dependentRepo)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _dependentRepo = dependentRepo;
        }
        public async Task<ApiResponse<List<GetDependentDto>>> GetAllDependents()
        {
            var result = new ApiResponse<List<GetDependentDto>>();
            result.Success = true;
            result.Message = "Dependents Retrieved";
            result.Data = _mapper.Map<List<GetDependentDto>>(await _dependentRepo.GetAllAysnc());
            return result;
        }

        public async Task<ApiResponse<GetDependentDto>> GetDependentById(int id)
        {
            var result = new ApiResponse<GetDependentDto>();
            var dependent = _mapper.Map<GetDependentDto>(await _dependentRepo.GetByIdAsync(id));
            if (dependent is null)
            {
                result.Success = false;
                result.Error = "No Dependent Found";
                return result;
            }
            result.Success = true;
            result.Message = "Dependent Retrieved";
            result.Data = dependent;
            return result;
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
            foreach (var item in employee.Data.Dependents)
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
            await _dependentRepo.AddAysnc(model);
            result.Data = _mapper.Map<GetDependentDto>(model);
            result.Success = true;
            result.Message = $"Dependent successfully added to {request.EmployeeId}";
            return result;

        }
    }
}
