using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Company>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
