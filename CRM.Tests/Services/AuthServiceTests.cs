using CRM.Application.DTOs.Auth;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CRM.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object);
        }

        // ── Register Tests ────────────────────────────────────────

        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_WhenEmailDoesNotExist()
        {
            // Arrange
            var request = new RegisterRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "Password123!"
            };

            _userRepositoryMock
                .Setup(r => r.EmailExistAsync(request.Email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(new User());

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFalse_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new RegisterRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "Password123!"
            };

            _userRepositoryMock
                .Setup(r => r.EmailExistAsync(request.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Should().BeFalse();
        }

        // ── Login Tests ───────────────────────────────────────────

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Email = "john@example.com",
                Password = "Password123!"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");

            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PasswordHash = hashedPassword
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _tokenServiceMock
                .Setup(t => t.GenerateToken(user))
                .Returns("fake-jwt-token");

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.Email.Should().Be("john@example.com");
            result.FullName.Should().Be("John Doe");
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Email = "notfound@example.com",
                Password = "Password123!"
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _authService.LoginAsync(request);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowUnauthorized_WhenPasswordIsWrong()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Email = "john@example.com",
                Password = "WrongPassword!"
            };

            var user = new User
            {
                Id = 1,
                Email = "john@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _authService.LoginAsync(request);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}