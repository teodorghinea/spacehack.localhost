using System;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _efDbContext;
      

        public UnitOfWork(EfDbContext efDbContext)
        {
            _efDbContext = efDbContext;
          
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
