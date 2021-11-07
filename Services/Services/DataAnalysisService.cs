using AutoMapper;
using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Dtos;
using Services.Services.MonkeyLearnService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IDataAnalysisService
    {
        Task<MatchingMetricsDto> ImportPostInformationAsync(PostForMetricsDto request);
    }

    public class DataAnalysisService : IDataAnalysisService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestService _requestService;

        public DataAnalysisService(IRequestService requestService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _requestService = requestService;
        }

        public async Task<MatchingMetricsDto> ImportPostInformationAsync(PostForMetricsDto request)
        {
            var keywordRequest = new KeywordApiRequest();
            keywordRequest.Data = new List<string> { request.Content };
            var result = await _requestService.GetPostKeywordsAsync(keywordRequest);
            var keys = result?.SelectMany(r => r.extractions.Select(e => e.parsed_value)).ToList();
            if (!keys.Any())
            {
                return new MatchingMetricsDto();
            }
            return await GetPostMetricsAsync(keys);
        }


        public async Task<MatchingMetricsDto> GetPostMetricsAsync(List<string> keywords)
        {
            var allCompetitors = await _unitOfWork.Competitors.GetAllWithPostsAsync();
            var matchedCompetitors = GetCompetitorByKeys(keywords, allCompetitors);
            var ownAccount = allCompetitors.FirstOrDefault(c => c.Name == "Hootsuite");

            var result = new MatchingMetricsDto
            {
                Competitors = GetCompetitorMatchingPosts(keywords, matchedCompetitors),
                Score = GetPostsScore(ownAccount.FacebookPosts),
            };
            result.OwnMatchingPostCount = result.Competitors.FirstOrDefault(c => c.Competitor.Id == ownAccount.Id)?.MatchingPostsCount ?? 0;

            return result;
        }

        public List<Competitor> GetCompetitorByKeys(List<string> keywords, List<Competitor> competitors)
        {
            var matchedCompetitors = new List<Competitor>();
            foreach (var competitor in competitors)
            {
                foreach (var contentKey in keywords)
                {
                    foreach (var word in contentKey.Split(" "))
                    {
                        if (competitor.KeyWords.ToLower().Contains(word.ToLower()))
                        {
                            if (!matchedCompetitors.Any(c => c.Id == competitor.Id))
                            {
                                matchedCompetitors.Add(competitor);
                            }
                        }
                    }
                }
            }
            return matchedCompetitors;
        }

        private List<CompetitorWithPosts> GetCompetitorMatchingPosts(List<string> keywords, List<Competitor> competitors)
        {
            var competitorsWithPostsDto = new List<CompetitorWithPosts>();
            foreach (var competitor in competitors)
            {
                var posts = competitor.FacebookPosts;
                foreach (var contentKey in keywords)
                {
                    foreach (var word in contentKey.Split(" "))
                    {
                        foreach (var post in posts)
                        {
                            if (post.KeyWords != null)
                            {
                                if (post.KeyWords.ToLower().Contains(word.ToLower()))
                                {
                                    if (!competitorsWithPostsDto.Any(c => c.Competitor.Id == competitor.Id))
                                    {
                                        var competitorDto = GetCompetitorMetric(competitor);
                                        competitorDto.Posts.Add(_mapper.Map<FacebookPostDto>(post));
                                        competitorsWithPostsDto.Add(competitorDto);
                                    }
                                    else
                                    {
                                        var competitorDto = competitorsWithPostsDto.First(c => c.Competitor.Id == competitor.Id);
                                        if(!competitorDto.Posts.Any(p => p.Id == post.Id))
                                        {
                                            competitorDto.Posts.Add(_mapper.Map<FacebookPostDto>(post));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var competitor in competitorsWithPostsDto)
            {
                competitor.CompetitorScore = GetPostsScore(competitor.Posts);
                competitor.MatchingPostsCount = competitor.Posts.Count();
            }
            return competitorsWithPostsDto;
        }

        private CompetitorWithPosts GetCompetitorMetric(Competitor competitor)
        {

            return new CompetitorWithPosts
            {
                Competitor = _mapper.Map<CompetitorDto>(competitor),
            };
        }

        private int GetPostsScore(List<FacebookPost> posts)
        {
            var likes = posts.Sum(p => p.Likes);
            var shares = posts.Sum(p => p.Shares);
            var comments = posts.Sum(p => p.Comments);
            var reactions = posts.Sum(p => p.Reactions);

            return likes     * 1 +
                   comments  * 1 +
                   reactions * 1 +
                   shares    * 1; 
        }

        private int GetPostsScore(List<FacebookPostDto> posts)
        {
            return GetPostsScore(_mapper.Map<List<FacebookPost>>(posts));
        }

        public List<string> GetKeyWords(string input)
        {
            var usedWords = input.Split(" ");
            var vocabulary = new Dictionary<string, int>();

            for (var i = 0; i < usedWords.Length; i++)
            {
                var word = usedWords[i].ToLower();
                if (!vocabulary.Keys.Any(k => k.ToLower() == word))
                {
                    vocabulary.Add(word, 1);
                }
                else
                {
                    vocabulary[word] = vocabulary[word] + 1;
                }
            }

            return vocabulary.OrderByDescending(v => v.Value)
                .Select(v => v.Key).Take(5)
                .ToList();
        }
    }
}
