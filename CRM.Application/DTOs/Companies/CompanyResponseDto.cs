using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Companies
{
    public class CompanyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Industry { get; set; }
        public string? Website { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int TotalContacts { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
