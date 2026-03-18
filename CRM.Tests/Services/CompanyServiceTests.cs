using CRM.Application.DTOs.Companies;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CRM.Tests.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _companyService = new CompanyService(_companyRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCompanies()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Acme Corp" },
                new Company { Id = 2, Name = "Tech Inc" }
            };

            _companyRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(companies);

            // Act
            var result = await _companyService.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Acme Corp");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCompany()
        {
            // Arrange
            var request = new CompanyRequestDto
            {
                Name = "Acme Corp",
                Industry = "Technology"
            };

            _companyRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Company>()))
                .ReturnsAsync((Company c) => c);

            // Act
            var result = await _companyService.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Acme Corp");
            result.Industry.Should().Be("Technology");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenCompanyNotFound()
        {
            // Arrange
            _companyRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Company?)null);

            // Act
            var result = await _companyService.DeleteAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}