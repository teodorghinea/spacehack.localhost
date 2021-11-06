using AutoMapper;
using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Dtos;
using Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services.Facebook
{

    public interface IFacebookService
    {
        Task<List<FacebookPostDto>> GetPostsAsync(int skip = 0, int take = 0);
        Task<bool> AddPostAsync(List<FacebookPostAddDto> request);
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

        public async Task<bool> AddPostAsync(List<FacebookPostAddDto> request)
        {
            foreach(var post in request)
            {
                var facebookPost = _mapper.Map<FacebookPost>(post);
                facebookPost.Date = DateTimeHelper.GetDateFromDummyRequest(post.Date);
                _unitOfWork.FacebookPosts.Insert(facebookPost);
            }
            return await _unitOfWork.SaveChangesAsync();
        } 
    }
}
