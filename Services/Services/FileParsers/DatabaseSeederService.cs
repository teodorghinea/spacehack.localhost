using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Helpers;
using Services.Services.MonkeyLearnService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.DatabaseParser
{

    public interface IDatabaseSeederService
    {
        Task GetContentAsync(Type entityType, string filename);
        Task<bool> Facebook_KeywordseedAsync();
    }

    public class DatabaseSeederService : IDatabaseSeederService
    {

        private readonly IRequestService _requestService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _path;

        public DatabaseSeederService(IUnitOfWork unitOfWork, IRequestService requestService)
        {
            _unitOfWork = unitOfWork;
            _requestService = requestService;
            _path = Environment.CurrentDirectory + "\\hootsuiteDb";
        }

        public async Task GetContentAsync(Type entityType, string filename)
        {
            var text = (await File.ReadAllLinesAsync($@"{_path}\\{filename}")).ToList()
                .Skip(1).ToList();

            if (entityType == typeof(FacebookPost))
            {
                Console.WriteLine("Unpacking facebook-db tsv...");
                var result = await Facebook_DataseedAsync(text.ToList());
                Console.WriteLine(result ? "Done!" : "Something went wrong");
            }
        }

        public async Task<bool> Facebook_KeywordseedAsync()
        {
            var posts = await _unitOfWork.FacebookPosts.GetAllAsync();
            var results = new List<KeyWordApiResponse>();

            posts = posts.Where(p => p.KeyWords == null).ToList();

            for (int i = 0; i < posts.Count; i++)
            {

                var request = new KeywordApiRequest()
                {
                    Data = new List<string> { posts[i].Content },
                };

                var result = await _requestService.GetPostKeywordsAsync(request);

                var keywords = result.SelectMany(r => r.extractions
                  .Select(e => e?.parsed_value)).ToList();
                if (keywords.Any())
                {
                    posts[i].KeyWords = keywords.Aggregate((x, y) => x + "," + y);
                }
                else
                {
                    posts[i].KeyWords = "x";
                }
                _unitOfWork.FacebookPosts.Update(posts[i]);
                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine(i + 1 + " entities done" + posts[i].KeyWords);
                Console.WriteLine("--------------------------------------------------------------");
                Thread.Sleep(300);
                if (i % 25 == 0) Thread.Sleep(5000);

            }
            return true;
        }

        private async Task<bool> Facebook_DataseedAsync(List<string> dataset)
        {
            var facebookPosts = new List<FacebookPost>();
            foreach (var line in dataset)
            {
                var contents = line.Split("\t");
                var postDate = DateTimeHelper.GetUtcFromEpoch(int.Parse(contents[1]));
                var mediaLink = StringHelper.GetMediaUrl(contents[5]);
                var mediaType = StringHelper.GetMediaType(contents[5]);

                var post = new FacebookPost
                {
                    Date = postDate,
                    Content = contents[2],
                    MediaFile = mediaLink,
                    MediaType = mediaType,
                    Url = contents[4],
                    Comments = int.Parse(contents[6]),
                    Likes = int.Parse(contents[7]),
                    Shares = int.Parse(contents[8]),
                    Reactions = int.Parse(contents[9]),
                    PostNumber = int.Parse(contents[0])
                };

                facebookPosts.Add(post);
            }
            _unitOfWork.FacebookPosts.InsertRange(facebookPosts);
            return await _unitOfWork.SaveChangesAsync();
        }

    }
}
