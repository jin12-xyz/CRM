using CRM.Application.DTOs.Contacts;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CRM.Tests.Services
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactService = new ContactService(_contactRepositoryMock.Object);
        }

        // ── GetAll Tests ──────────────────────────────────────────

        [Fact]
        public async Task GetAllAsync_ShouldReturnContacts_ForGivenUser()
        {
            // Arrange
            var userId = 1;
            var contacts = new List<Contact>
            {
                new Contact
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    UserId = userId
                },
                new Contact
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Jones",
                    Email = "bob@example.com",
                    UserId = userId
                }
            };

            _contactRepositoryMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(contacts);

            // Act
            var result = await _contactService.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.First().Email.Should().Be("jane@example.com");
        }

        // ── GetById Tests ─────────────────────────────────────────

        [Fact]
        public async Task GetByIdAsync_ShouldReturnContact_WhenExists()
        {
            // Arrange
            var contact = new Contact
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(contact);

            // Act
            var result = await _contactService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("jane@example.com");
            result.FullName.Should().Be("Jane Smith");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Contact?)null);

            // Act
            var result = await _contactService.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // ── Create Tests ──────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedContact()
        {
            // Arrange
            var userId = 1;
            var request = new ContactRequestDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Phone = "123-456-7890",
                JobTitle = "CEO"
            };

            _contactRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Contact>()))
                .ReturnsAsync((Contact c) => c);

            // Act
            var result = await _contactService.CreateAsync(request, userId);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("Jane");
            result.Email.Should().Be("jane@example.com");
        }

        // ── Update Tests ──────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedContact_WhenExists()
        {
            // Arrange
            var contact = new Contact
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            var request = new ContactRequestDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.updated@example.com"
            };

            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(contact);

            _contactRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Contact>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _contactService.UpdateAsync(1, request);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("jane.updated@example.com");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Contact?)null);

            var request = new ContactRequestDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            // Act
            var act = async () => await _contactService.UpdateAsync(99, request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        // ── Delete Tests ──────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenContactExists()
        {
            // Arrange
            var contact = new Contact { Id = 1 };

            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(contact);

            _contactRepositoryMock
                .Setup(r => r.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _contactService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenContactNotFound()
        {
            // Arrange
            _contactRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Contact?)null);

            // Act
            var result = await _contactService.DeleteAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}