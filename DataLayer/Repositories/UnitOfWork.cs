using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
        IFacebookPostRepository FacebookPosts { get; }
        ICompetitorRepository Competitors { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _efDbContext;

        public IFacebookPostRepository FacebookPosts { get; }
        public ICompetitorRepository Competitors { get; }

        public UnitOfWork(EfDbContext efDbContext, 
            IFacebookPostRepository facebookPosts, 
            ICompetitorRepository competitors)
        {
            _efDbContext = efDbContext;
            FacebookPosts = facebookPosts;
            Competitors = competitors;
        }

        public async Task<bool> SaveChangesAsync()
        {
            DatabaseLogger();
            try
            {
                var savedChanges = await _efDbContext.SaveChangesAsync();
                return savedChanges >= 0;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw new BadRequestException("CANNOT_UPDATED_DATABASE");
            }
        }

        private void DatabaseLogger()
        {
            foreach (var entry in _efDbContext.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: { entry.State}");
            }
        }
    }
}
