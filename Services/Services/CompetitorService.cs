using AutoMapper;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ICompetitorService
    {
        Task<List<CompetitorDto>> GetAllCompetitorsAsync();
        Task<bool> AddNewCompetitorAsync(CompetitorDto newCompetitor);
        Task<CompetitorDto> GetCompetitorByIdAsync(Guid competitorId);
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

    }
}
