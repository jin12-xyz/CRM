using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Notes
{
    public class NoteRequestDto
    {
        public string Content { get; set; } = string.Empty;
        public int ContactId { get; set; }
    }
}
