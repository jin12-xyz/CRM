using CRM.Application.DTOs.Auth;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        /* Flow for registration:
               * Receives email + password from user
                        ↓
                Check if email already exists in DB
                        ↓
                If exists → return false (don't register twice)
                        ↓
                If not → create new User object
                        ↓
                Hash the password (never store plain text)
                        ↓
                Save user to database
                        ↓
                Return true (success)
         */
        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var emailExists = await _userRepository.EmailExistAsync(request.Email);
            if (emailExists) return false;

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        /* Flow for login:
               * Receives email + password from user
                        ↓
                Look up user by email in DB
                        ↓
                If not found → throw error (invalid credentials)
                        ↓
                If found → verify password hash matches
                        ↓
                If password doesn't match → throw error (invalid credentials)
                        ↓
                If password matches → generate JWT token
                        ↓
                Return token + user info to client

        Why throw instead of return false?
        Login failure is an exceptional case — the caller (controller) needs to know it failed and return a 401 Unauthorized response.
         */
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = _tokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

    }
}
