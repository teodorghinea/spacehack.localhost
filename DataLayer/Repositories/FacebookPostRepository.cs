using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{

    public interface IFacebookPostRepository : IRepositoryBase<FacebookPost>
    {
        Task<List<FacebookPost>> GetAllSpecifyListSizeAsync(int skip = 0, int take = 0);
    }

    public class FacebookPostRepository : RepositoryBase<FacebookPost>, IFacebookPostRepository
    {

        public FacebookPostRepository(EfDbContext context) : base(context)
        {

        }

        public async Task<List<FacebookPost>> GetAllSpecifyListSizeAsync(int skip = 0, int take = 0)
        {
            var result = GetRecords(true).OrderByDescending(p => p.Date).Skip(skip);
            if (take != 0) result = result.Take(take);

            return await result.ToListAsync();
        }
    }
}
