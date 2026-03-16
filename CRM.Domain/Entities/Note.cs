using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Content { get; set; } = string.Empty;

        //Foreign Keys
        public int ContactId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Contact Contact { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
