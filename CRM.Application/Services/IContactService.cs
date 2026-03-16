using CRM.Application.DTOs.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactResponseDto>> GetAllAsync(int userId);
        Task<ContactResponseDto?> GetByIdAsync(int id);
        Task<ContactResponseDto> CreateAsync(ContactRequestDto request, int userId);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ContactResponseDto>> SearchAsync(string keyword);
    }
}
