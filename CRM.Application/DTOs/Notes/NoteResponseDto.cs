using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Notes
{
    public class NoteResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int ContactId { get; set; }
        public string ContactFullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
