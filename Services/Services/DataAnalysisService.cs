
using DataLayer.Repositories;
using Services.Dtos;
using Services.Services.MonkeyLearnService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IDataAnalysisService
    {
        Task<List<string>> ImportPostInformationAsync(PostForMetricsDto request);
    }

    public class DataAnalysisService : IDataAnalysisService
    {

        private readonly IRequestService _requestService;

        public DataAnalysisService(IRequestService requestService) 
        {
            _requestService = requestService;
        }
        
        public async Task<List<string>> ImportPostInformationAsync(PostForMetricsDto request)
        {
            var keywordRequest = new KeywordApiRequest();
            keywordRequest.Data = new List<string> { request.Content };
            var result = await _requestService.GetPostKeywordsAsync(keywordRequest);
            return result?.SelectMany(r => r.extractions.Select(e => e.parsed_value)).ToList();
        }

        public List<string> GetKeyWords(string input)
        {
            var usedWords = input.Split(" ");
            var vocabulary = new Dictionary<string, int>();
            
            for(var i = 0; i< usedWords.Length; i++)
            {
                var word = usedWords[i].ToLower();
                if(!vocabulary.Keys.Any(k => k.ToLower() == word))
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
