using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetByCompanyIdAsync(int companyId);
        Task<IEnumerable<Contact>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Contact>> SearchAsync(string keyword);
    }
}
