using CRM.Application.DTOs.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteResponseDto>> GetByContactIdAsync(int contactid);
        Task<NoteResponseDto> CreateAsync(NoteRequestDto request, int userId);
        Task<bool> DeleteAsync(int id);
    }
}
