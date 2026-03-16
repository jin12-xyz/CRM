using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Industry { get; set; }
        public string? Website { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        //Navigation properties

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
