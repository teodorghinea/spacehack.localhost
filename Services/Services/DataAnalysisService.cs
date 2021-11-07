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
        private readonly bool _hardcoreMatching;

        public DataAnalysisService(IRequestService requestService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _requestService = requestService;
            _hardcoreMatching = false;
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
            var dto = await GetPostMetricsAsync(keys);
            dto.PostKeywords = keys;
            return dto;
        }


        public async Task<MatchingMetricsDto> GetPostMetricsAsync(List<string> keywords)
        {
            var allCompetitors = await _unitOfWork.Competitors.GetAllWithPostsAsync();
            var matchedCompetitors = GetCompetitorByKeys(keywords, allCompetitors);
            var ownAccount = allCompetitors.FirstOrDefault(c => c.Name == "Hootsuite");

            var result = new MatchingMetricsDto
            {
                Competitors = GetCompetitorMatchingPosts(keywords, matchedCompetitors),
            };
            result.OwnMatchingPostCount = result.Competitors.FirstOrDefault(c => c.Competitor.Id == ownAccount.Id)?.MatchingPostsCount ?? 0;
            result.Score = result.Competitors.Sum(c => c.CompetitorScore);
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
                        if (_hardcoreMatching ? 
                            competitor.KeyWords.ToLower().Contains(contentKey) :
                            competitor.KeyWords.ToLower().Contains(word.ToLower()))
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
                                if (_hardcoreMatching ?
                                    post.KeyWords.ToLower().Contains(contentKey) :
                                    post.KeyWords.ToLower().Contains(word.ToLower()))
                                {
                                    if (!competitorsWithPostsDto.Any(c => c.Competitor.Id == competitor.Id))
                                    { 
                                        var competitorDto = _mapper.Map<CompetitorWithPosts>(competitor);
                                        competitorDto.Posts.Add(GetArticleWithScore(post));
                                        competitorsWithPostsDto.Add(competitorDto);
                                    }
                                    else
                                    {
                                        var competitorDto = competitorsWithPostsDto.First(c => c.Competitor.Id == competitor.Id);
                                        if(!competitorDto.Posts.Any(p => p.Id == post.Id))
                                        {
                                            competitorDto.Posts.Add(GetArticleWithScore(post));
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
                competitor.MatchingPostsCount = competitor.Posts.Count();
                competitor.ArticlesKeywords = competitor.Posts.SelectMany(p => p.KeyWords.Split(",")).ToList();
                competitor.CompetitorScore = competitor.Posts.Sum(p => p.Score) / competitor.Posts.Count;
                competitor.Posts = competitor.Posts.OrderByDescending(p => p.Score).ToList();
            }
            return competitorsWithPostsDto.OrderByDescending(c => c.CompetitorScore).ToList();
        }

        private MetricsFacebookPost GetArticleWithScore(FacebookPost article)
        {
            var totalReactions = article.Likes + article.Comments + article.Reactions + article.Shares;

            var dto = _mapper.Map<MetricsFacebookPost>(article);

            dto.Score = totalReactions != 0 ? 
                article.Comments  * 0.15m / totalReactions +
                article.Likes     * 0.05m / totalReactions +
                article.Reactions * 0.1m  / totalReactions +
                article.Shares    * 0.2m  / totalReactions : 0;

            return dto;
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
