using CRM.Application.DTOs.Contacts;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        /* Flow for GetAllAsync:
               * Receives userId (to get only that user's contacts)
                        ↓
                Query repository for contacts with matching userId
                        ↓
                Map each Contact entity to ContactResponseDto
                        ↓
                Return list of ContactResponseDto

            Why filter by userId? 
            Each sales person should only see their own contacts — not everyone else's.
         */
        public async Task<IEnumerable<ContactResponseDto>> GetAllAsync(int userId)
        {
            var contacts = await _contactRepository.GetByUserIdAsync(userId);
            return contacts.Select(MapToDto);
        }
        /* Flow for GetByIdAsync:
               * Receives contact id
                        ↓
                Query repository for contact with that id
                        ↓
                If not found, return null
                        ↓
                If found, map Contact entity to ContactResponseDto and return it
            Why return null if not found? 
           The controller decides what to do with null (usually returns 404).
         */
        public async Task<ContactResponseDto?> GetByIdAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            return contact == null ? null : MapToDto(contact);
        }

        /* Flow for CreateAsync:
               * Receives ContactRequestDto + userId
                        ↓
                Create new Contact entity and populate fields from request + userId
                        ↓
                Save new contact to database via repository
                        ↓
                Map created Contact entity to ContactResponseDto and return it
         */
        public async Task<ContactResponseDto> CreateAsync(ContactRequestDto request, int userId)
        {
            var contact = new Contact
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                JobTitle = request.JobTitle,
                CompanyId = request.CompanyId,
                UserId = userId
            };

            var created = await _contactRepository.AddAsync(contact);
            return MapToDto(created);
        }

        /* flow for UpdateAsync:
               * Receives contact id + ContactRequestDto
                        ↓
                Query repository for existing contact with that id
                        ↓
                If not found, throw exception (or return null)
                        ↓
                If found, update fields of Contact entity from request
                        ↓
                Save updated contact to database via repository
                        ↓
                Map updated Contact entity to ContactResponseDto and return it
         */
        public async Task<ContactResponseDto> UpdateAsync(int id, ContactRequestDto request)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null) throw new KeyNotFoundException("Contact not found.");

            contact.FirstName = request.FirstName;
            contact.LastName = request.LastName;
            contact.Email = request.Email;
            contact.Phone = request.Phone;
            contact.JobTitle = request.JobTitle;
            contact.CompanyId = request.CompanyId;
            contact.UpdatedAt = DateTime.UtcNow;

            await _contactRepository.UpdateAsync(contact);
            return MapToDto(contact);
        }
        /* Flow for DeleteAsync:
               * Receives contact id
                        ↓
                Query repository for existing contact with that id
                        ↓
                If not found, return false (indicating delete failed)
                        ↓
                If found, delete contact via repository and return true (indicating success)
         */
        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null) return false;

            await _contactRepository.DeleteAsync(id);
            return true;
        }
        /* Flow for SearchAsync:
               * Receives search keyword
                        ↓
                Query repository for contacts matching keyword (in name, email, etc.)
                        ↓
                Map matching Contact entities to ContactResponseDto and return list
         */
        public async Task<IEnumerable<ContactResponseDto>> SearchAsync(string keyword)
        {
            var contacts = await _contactRepository.SearchAsync(keyword);
            return contacts.Select(MapToDto);
        }
        /* flow for MapToDto:
               * Receives Contact entity
                        ↓
                Create new ContactResponseDto and populate fields from Contact entity
                        ↓
                Return ContactResponseDto

        Why private static?
        It is a pure utility — takes input, returns output, needs no class state.
        Making it static means it belongs to the class itself, not an instance. This is a small but clean SOLID practice.


        Why a separate method instead of doing it inline?
        Every method that needs to convert a Contact to a DTO calls MapToDto() instead of repeating the mapping code.
         */
        private static ContactResponseDto MapToDto(Contact contact) => new()
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Phone = contact.Phone,
            JobTitle = contact.JobTitle,
            CompanyId = contact.CompanyId,
            CompanyName = contact.Company?.Name,
            CreateAt = contact.CreatedAt
        };
    }
}
