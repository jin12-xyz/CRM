using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces
{
    public interface INoteRepository : IRepository<Note>
    {
        Task<IEnumerable<Note>> GetByContactIdAsync(int contactId);
    }
}
