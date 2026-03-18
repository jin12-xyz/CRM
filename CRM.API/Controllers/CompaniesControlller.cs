using CRM.Application.DTOs.Companies;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
                return NotFound("Company not found.");

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CompanyRequestDto request)
        {
            var company = await _companyService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] CompanyRequestDto request)
        {
            try
            {
                var company = await _companyService.UpdateAsync(id, request);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _companyService.DeleteAsync(id);

            if (!result)
                return NotFound("Company not found.");

            return NoContent();
        }
    }
}