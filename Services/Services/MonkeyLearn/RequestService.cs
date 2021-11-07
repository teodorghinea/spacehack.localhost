

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Services.MonkeyLearnService
{
    public interface IRequestService
    {
        Task<List<KeyWordApiResponse>> GetPostKeywordsAsync(KeywordApiRequest payload);
    }

    public class RequestService : IRequestService
    {
        private readonly MonkeyLearnDto _monkeyLearnApi;

        public RequestService(IConfiguration configuration) 
        {
            _monkeyLearnApi = configuration.GetSection("MonkeyLearn").Get<MonkeyLearnDto>();
        }

        public async Task<List<KeyWordApiResponse>> GetPostKeywordsAsync(KeywordApiRequest payload)
        {
            var content = SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _monkeyLearnApi.KeyWordExtractorLink)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Token {_monkeyLearnApi.ApiKey}");
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            if(response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.ReasonPhrase);
            }

            return await GetResultAsync<List<KeyWordApiResponse>>(response);
        }

        private async Task<T> GetResultAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string SerializeObject(object payload)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(payload, setting);
        }
    }

    public class KeywordApiRequest
    {
        public List<string> Data {  get; set; } = new List<string>();
    }

    public class KeyWordApiResponse
    {
        public string text { get; set; }
        public string error { get; set; }

        public string external_id { get; set; }
        public List<ExtractionDto> extractions { get; set; } = new List<ExtractionDto>();

    }

    public class ExtractionDto
    {
        public string tag_name { get; set; }

        public string parsed_value { get; set; }

        public int count { get; set; }
        public string relevance { get; set; }
        public List<int> positions_in_text { get; set; } = new List<int>();
    }

    public class MonkeyLearnDto
    {
        public string ApiKey { get; set; }
        public string KeyWordExtractorLink { get; set; }
    }


}
