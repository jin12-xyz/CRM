using CRM.Application.DTOs.Companies;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        /* flow for GetAllAsync:
               * Query repository for all companies
                        ↓
                Map each Company entity to CompanyResponseDto
                        ↓
                Return list of CompanyResponseDto
         */
        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(MapToDto);
        }
        /* flow for GetByIdAsync:
               * Query repository for company by ID
                        ↓
                If not found, return null
                        ↓
                If found, map Company entity to CompanyResponseDto
                        ↓
                Return CompanyResponseDto
         */
        public async Task<CompanyResponseDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }
        /* flow for CreateAsync:
               * Receive CompanyRequestDto from caller
                        ↓
                Create new Company entity from request data
                        ↓
                Save new company to repository
                        ↓
                Map saved Company entity to CompanyResponseDto
                        ↓
                Return CompanyResponseDto
         */
        public async Task<CompanyResponseDto> CreateAsync(CompanyRequestDto request)
        {
            var company = new Company
            {
                Name = request.Name,
                Industry = request.Industry,
                Website = request.Website,
                Phone = request.Phone,
                Address = request.Address
            };

            var created = await _companyRepository.AddAsync(company);
            return MapToDto(created);
        }
        /* flow for UpdateAsync:
               * Receive company ID and CompanyRequestDto from caller
                        ↓
                Query repository for existing company by ID
                        ↓
                If not found, throw KeyNotFoundException
                        ↓
                If found, update company properties with request data
                        ↓
                Save updated company to repository
                        ↓
                Map updated Company entity to CompanyResponseDto
                        ↓
                Return CompanyResponseDto
         */
        public async Task<CompanyResponseDto> UpdateAsync(int id, CompanyRequestDto request)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) throw new KeyNotFoundException("Company not found.");

            company.Name = request.Name;
            company.Industry = request.Industry;
            company.Website = request.Website;
            company.Phone = request.Phone;
            company.Address = request.Address;
            company.UpdatedAt = DateTime.UtcNow;

            await _companyRepository.UpdateAsync(company);
            return MapToDto(company);
        }
        /* flow for DeleteAsync:
               * Receive company ID from caller
                        ↓
                Query repository for existing company by ID
                        ↓
                If not found, return false
                        ↓
                If found, delete company from repository
                        ↓
                Return true to indicate successful deletion
         */
        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;

            await _companyRepository.DeleteAsync(id);
            return true;
        }
        /* flow for MapToDto:
               * Receive Company entity
                        ↓
                Create new CompanyResponseDto and populate fields from Company entity
                        ↓
                Return CompanyResponseDto
        */
        private static CompanyResponseDto MapToDto(Company company) => new()
        {
            Id = company.Id,
            Name = company.Name,
            Industry = company.Industry,
            Website = company.Website,
            Phone = company.Phone,
            Address = company.Address,
            TotalContacts = company.Contacts?.Count ?? 0, // if Contacts is null, return 0
            CreatedAt = company.CreatedAt
        };
    }
}
