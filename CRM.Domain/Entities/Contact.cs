using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        public string? JobTitle { get; set; }

        // Foreign Keys
        public int? CompanyId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Company? Company { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Note> Notes {get; set;} = new List<Note>();
    }
}
