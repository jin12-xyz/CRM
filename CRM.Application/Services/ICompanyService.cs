using CRM.Application.DTOs.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
        Task<CompanyResponseDto?> GetByIdAsync(int id);
        Task<CompanyResponseDto> CreateAsync(CompanyRequestDto request);
        Task<CompanyResponseDto> UpdateAsync(int id, CompanyRequestDto request);
        Task<bool> DeleteAsync(int id);
    }
}
