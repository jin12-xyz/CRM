using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Contact>> GetByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> SearchAsync(string keyword)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Where(c =>
                    c.FirstName.Contains(keyword) ||
                    c.LastName.Contains(keyword) ||
                    c.Email.Contains(keyword))
                .ToListAsync();
        }
    }
}