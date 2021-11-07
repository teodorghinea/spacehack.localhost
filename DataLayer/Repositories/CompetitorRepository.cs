using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{

    public interface ICompetitorRepository : IRepositoryBase<Competitor>
    {
        Task<Competitor> GetByNameWithPostsAsync(string name);
        Task<List<Competitor>> GetAllWithPostsAsync();
    }

    public class CompetitorRepository : RepositoryBase<Competitor>, ICompetitorRepository
    {
        public CompetitorRepository(EfDbContext context) : base(context) { }

        public async Task<Competitor> GetByNameWithPostsAsync(string name)
        {
            return await GetRecords(true)
                .Include(c => c.FacebookPosts)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<List<Competitor>> GetAllWithPostsAsync()
        {
            return await GetRecords(true)
                .Include(c => c.FacebookPosts)
                .ToListAsync();
        }

    }
}
