using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Contacts
{
    public class ContactResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FulllName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? JobTitle { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
