using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{

    public interface ICompetitorRepository : IRepositoryBase<Competitor>
    {

    }

    public class CompetitorRepository : RepositoryBase<Competitor>, ICompetitorRepository
    {
        public CompetitorRepository(EfDbContext context) : base(context) { }




    }
}
