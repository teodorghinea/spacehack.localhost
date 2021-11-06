using AutoMapper;
using DataLayer.Repositories;
using Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services.Facebook
{

    public interface IFacebookService
    {
        Task<List<FacebookPostDto>> GetPostsAsync(int skip = 0, int take = 0);
    }

    public class FacebookService : IFacebookService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FacebookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FacebookPostDto>> GetPostsAsync(int skip = 0, int take = 0)
        {
            var facebookPosts = await _unitOfWork.FacebookPosts.GetAllSpecifyListSizeAsync(skip, take);
            return _mapper.Map<List<FacebookPostDto>>(facebookPosts);
        }

    }
}
