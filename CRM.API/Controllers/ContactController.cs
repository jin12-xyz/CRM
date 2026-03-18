using CRM.Application.DTOs.Contacts;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactService.GetAllAsync(GetUserId());
            return Ok(contacts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var contacts = await _contactService.SearchAsync(keyword);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);

            if (contact == null)
                return NotFound("Contact not found.");

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ContactRequestDto request)
        {
            var contact = await _contactService.CreateAsync(request, GetUserId());
            return CreatedAtAction(nameof(GetById),
                new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] ContactRequestDto request)
        {
            try
            {
                var contact = await _contactService.UpdateAsync(id, request);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactService.DeleteAsync(id);

            if (!result)
                return NotFound("Contact not found.");

            return NoContent();
        }
    }
}