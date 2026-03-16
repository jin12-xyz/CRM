using CRM.Application.DTOs.Notes;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class NoteService : INoteServices
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<NoteResponseDto>> GetByContactIdAsync(int contactId)
        {
            var notes = await _noteRepository.GetByContactIdAsync(contactId);
            return notes.Select(MapToDto);
        }

        public async Task<NoteResponseDto> CreateAsync(NoteRequestDto request, int userId)
        {
            var note = new Note
            {
                Content = request.Content,
                ContactId = request.ContactId,
                UserId = userId
            };

            var created = await _noteRepository.AddAsync(note);
            return MapToDto(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return false;

            await _noteRepository.DeleteAsync(id);
            return true;
        }

        /* flow for mapping Note to NoteResponseDto:
               * Receives Note entity
                        ↓
                Create new NoteResponseDto and populate fields from Note
                        ↓
                For ContactFullName, if Note.Contact is not null, concatenate first and last name; otherwise use empty string
                        ↓
                Return NoteResponseDto
         */
        private static NoteResponseDto MapToDto(Note note) => new()
        {
            Id = note.Id,
            Content = note.Content,
            ContactId = note.ContactId,
            ContactFullName = note.Contact != null
                ? $"{note.Contact.FirstName} {note.Contact.LastName}"
                : string.Empty,
            CreatedAt = note.CreatedAt
        };
    }
}