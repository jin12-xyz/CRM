using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repositories
{
    public class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Note>> GetByContactIdAsync(int contactId)
        {
            return await _dbSet
                .Include(n => n.Contact)
                .Where(n => n.ContactId == contactId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}