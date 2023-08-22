using Api.Dtos.Dependent;
using Api.Models.DTOs;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Interfaces
{
    public interface IDependentService
    {
        Task<List<GetDependentDto>> GetAllDependents();
        Task<GetDependentDto> GetDependentById(int id);
        Task<ApiResponse<GetDependentDto>> AddDependent(CreateDependentRequest request);
    }
}
