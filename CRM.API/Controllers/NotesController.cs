using CRM.Application.DTOs.Notes;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("contact/{contactId}")]
        public async Task<IActionResult> GetByContact(int contactId)
        {
            var notes = await _noteService
                .GetByContactIdAsync(contactId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] NoteRequestDto request)
        {
            var note = await _noteService
                .CreateAsync(request, GetUserId());
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.DeleteAsync(id);

            if (!result)
                return NotFound("Note not found.");

            return NoContent();
        }
    }
}
