using AutoMapper;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Dtos;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ICompetitorService
    {
        Task<CompetitorDto> GetMyAccountAsync();
        Task<List<CompetitorDto>> GetAllCompetitorsAsync();
        Task<bool> AddNewCompetitorAsync(CompetitorDto newCompetitor);
        Task<CompetitorDto> GetCompetitorByIdAsync(Guid competitorId);
        Task<Dictionary<string, StatusCounter>> GetYearlyStatusAsync();
    }

    public class CompetitorService : ICompetitorService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public CompetitorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /* general */
        public async Task<List<CompetitorDto>> GetAllCompetitorsAsync()
        {
            var competitorList = await _unitOfWork.Competitors.GetAllAsync();
            return _mapper.Map<List<CompetitorDto>>(competitorList);
        } 

        public async Task<CompetitorDto> GetCompetitorByIdAsync(Guid competitorId)
        {
            var competitor = await _unitOfWork.Competitors.GetByIdAsync(competitorId);
            if (competitor == null) throw new BadRequestException("Account not found");
            return _mapper.Map<CompetitorDto>(competitor);
        }

        public async Task<bool> AddNewCompetitorAsync(CompetitorDto newCompetitor)
        {
            var competitor = _mapper.Map<Competitor>(newCompetitor);
            _unitOfWork.Competitors.Insert(competitor);
            return await _unitOfWork.SaveChangesAsync();
        }


        /* my account */
        public async Task<CompetitorDto> GetMyAccountAsync()
        {
            var allCompetitors = await GetAllCompetitorsAsync();
            return allCompetitors.FirstOrDefault(c => c.Name == "Hootsuite");
        }

        public async Task<Dictionary<string, StatusCounter>> GetYearlyStatusAsync()
        {
            var myAccount = await _unitOfWork.Competitors.GetByNameWithPostsAsync("Hootsuite");
            var postsByWeek = myAccount.FacebookPosts.Where(p => p.Date.Year == DateTime.UtcNow.Year)
                .OrderBy(p => p.Date)
                .GroupBy(p => DateTimeHelper.GetWeekNumber(p.Date))
                .ToList();

            var result = new Dictionary<string, StatusCounter>();
            foreach(var week in postsByWeek)
            {
                var status = new StatusCounter
                {
                    Likes = week.Sum(w => w.Likes),
                    Shares = week.Sum(w => w.Shares),
                    Comments = week.Sum(w => w.Comments),
                    Reactions = week.Sum(w => w.Reactions)
                };
                result.Add(week.Key.ToString(), status);
            }
            return result;
        }
    }
}
